using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ShootanGaem
{
    class ShootanEngine
    {
        GraphicsDevice graphicsDevice;
        ContentManager Content;
        KeyboardState keyboardState;
        KeyboardState prevKeyboardState;
        private static Random rand = new Random();

        private int GAME_WIDTH;
        private int GAME_HEIGHT;
        private bool gameRunning;

        private Level level;
        private Player player;

        private List<NPC> activeEnemies = new List<NPC>();
        private ParticleManager particleManager;

        public ShootanEngine(GraphicsDevice gd, int width, int height, ContentManager cm)
        {
            GAME_WIDTH = width;
            GAME_HEIGHT = height;
            Content = cm;
            graphicsDevice = gd;

            particleManager = new ParticleManager(gd, rand);
        }

        //Creates player object
        public void createPlayer(Texture2D sprite, Vector2 pos)
        {
            player = new Player(sprite, pos);
            //player.manageBullets.setDelay(100);
        }

        //Changes player's fire delay
        public void setPlayerDelay(float ms)
        {
            player.manageBullets.setDelay(ms);
        }

        //Changes player's speed
        public void setPlayerSpeed(int spd)
        {
            player.setSpeed(spd);
        }

        //Add bullets to player
        public void addPlayerBullets(Texture2D sprite, int numBullets, Color sprColor, int dmg, float spd)
        {
            player.addBullets(sprite, numBullets, sprColor, dmg, spd);
        }

        //Add bullet patterns to player
        public void addPlayerPattern(Pattern p)
        {
            player.addPattern(p);
        }

        //Load Level from file
        public void loadLevel(StreamReader newLevelData)
        {
            level = new Level(newLevelData, Content);

            //Populate activeEnemies for initial run
            while (!level.isCurrentWaveEmpty())
            {
                activeEnemies.Add(level.getEnemy());
            }

            level.discardCurrentWave();
        }

        //Check if game is currently running
        public bool gameIsRunning()
        {
            return gameRunning;
        }

        //Reset level
        public void reset()
        {
            level.reset();
            gameRunning = false;
            activeEnemies = new List<NPC>();
        }

        public void update(GameTime gameTime)
        {
            gameRunning = true;
            keyboardState = Keyboard.GetState();

            /* --------------
             * Player Updates
             * --------------
             */
            //Player movement
            if (keyboardState.IsKeyDown(Keys.Up) && player.getPosition().Y > 0 && !player.isDead())
            {
                player.moveUp();
            }
            if (keyboardState.IsKeyDown(Keys.Down) && player.getPosition().Y + player.getSprite().Height < GAME_HEIGHT && !player.isDead())
            {
                player.moveDown();
            }
            if (keyboardState.IsKeyDown(Keys.Left) && player.getPosition().X > 0 && !player.isDead())
            {
                player.moveLeft();
            }
            if (keyboardState.IsKeyDown(Keys.Right) && player.getPosition().X + player.getSprite().Width < GAME_WIDTH && !player.isDead())
            {
                player.moveRight();
            }

            //Player fire
            if (keyboardState.IsKeyDown(Keys.Z))
            {
                player.fire(gameTime.TotalGameTime.TotalMilliseconds);
            }

            //Handle Player bullet movement
            player.manageBullets.updatePosition();

            List<Bullet> bullets = player.manageBullets.getActiveBullets();
            for (int i = 0; i < bullets.Count; i++)
            {
                //If bullet is out of bounds, recycle it
                if (bullets[i].position.X < 0 || bullets[i].position.Y < 0 || bullets[i].position.X > GAME_WIDTH || bullets[i].position.Y > GAME_HEIGHT)
                    player.manageBullets.recycleBullet(bullets[i]);
                else
                {
                    //Handle collision with enemy
                    for (int c = 0; c < activeEnemies.Count && i < bullets.Count; c++)
                    {
                        if (!activeEnemies[c].isDead())
                        {
                            if (activeEnemies[c].isHitBy(bullets[i]))
                            {
                                activeEnemies[c].takeDamage(bullets[i].getDamage());
                                activeEnemies[c].hit = true;

                                //Spawn particles in middle of sprite
                                Vector2 particlePos = new Vector2(activeEnemies[c].getPosition().X + activeEnemies[c].getSprite().Width / 2, activeEnemies[c].getPosition().Y + activeEnemies[c].getSprite().Height / 2);

                                //Shoot particles down
                                Vector2 particleDir = new Vector2();
                                if (rand.NextDouble() > .5)
                                    particleDir.X = (float)rand.NextDouble() * 2;
                                else
                                    particleDir.X = (float)rand.NextDouble() * -2;

                                particleDir.Y = 1f;

                                //Generate particles
                                particleManager.generateParticles(activeEnemies[c], particlePos, particleDir, 5, 25);

                                //If enemy has <= 0 hp, mark as dead and create particle explosion
                                if (activeEnemies[c].getHealth() <= 0)
                                {
                                    activeEnemies[c].die();

                                    //Spawn particles in middle of sprite
                                    Vector2 deadParticlePos = new Vector2(activeEnemies[c].getPosition().X + activeEnemies[c].getSprite().Width / 2, activeEnemies[c].getPosition().Y + activeEnemies[c].getSprite().Height / 2);

                                    for (int currPart = 0; currPart < 50; currPart++)
                                    {
                                        //Shoot particles randomly
                                        Vector2 deadParticleDir = new Vector2();
                                        if (rand.NextDouble() > .5)
                                            deadParticleDir.X = (float)rand.NextDouble() * 2;
                                        else
                                            deadParticleDir.X = (float)rand.NextDouble() * -2;

                                        double randY = rand.NextDouble();
                                        if (randY > .33 && randY < .66)
                                            deadParticleDir.Y = (float)rand.NextDouble() * 2;
                                        else if (randY < .33)
                                            deadParticleDir.Y = 0;
                                        else
                                            deadParticleDir.Y = (float)rand.NextDouble() * -2;

                                        //Generate particles
                                        particleManager.generateParticles(activeEnemies[c], deadParticlePos, deadParticleDir, 1, 100);
                                    }
                                }

                                //Recycle bullet when it hits an enemy
                                player.manageBullets.recycleBullet(bullets[i]);
                            }
                        }
                    }
                }
            }


            /* --------------
             * Event Updates
             * --------------
             */
            string currentEvent = level.getCurrentEvent();
            if (currentEvent == "DIALOGUE")
            {
                Dialogue currentDialogue = level.getDialogue();

                if (!currentDialogue.isDialogueOver())
                {
                    currentDialogue.update();

                    if (keyboardState.IsKeyDown(Keys.Z) && prevKeyboardState.IsKeyUp(Keys.Z))
                    {
                        currentDialogue.nextLine();
                    }
                    prevKeyboardState = keyboardState;
                }
                else
                {
                    level.discardCurrentDialogue();

                    //Move on to next event
                    level.nextEvent();
                }
            }
            else if(currentEvent == "WAVE")
            {
                if (!level.isLevelOver() || activeEnemies.Count > 0)
                {
                    if (activeEnemies.Count == 0)
                    {
                        //If there are currently no enemies, spawn a new wave
                        while (!level.isCurrentWaveEmpty())
                        {
                            activeEnemies.Add(level.getEnemy());
                        }

                        //Since we've retrieved all the enemies in the wave, we can remove it from Level's queue
                        level.discardCurrentWave();

                        //Move on to next event
                        level.nextEvent();
                    }
                    else //If there are alive active enemies, let them do their actions
                    {
                        for (int i = 0; i < activeEnemies.Count; i++)
                        {
                            if (!activeEnemies[i].isDead())
                            {
                                //Target center of player
                                activeEnemies[i].setTarget(new Vector2(player.getPosition().X + player.getSprite().Width / 2, player.getPosition().Y + player.getSprite().Height / 2));
                                activeEnemies[i].doAction(gameTime.TotalGameTime.TotalMilliseconds);

                                //If NPC is out of window, mark as dead
                                if (activeEnemies[i].getPosition().X < 0 || activeEnemies[i].getPosition().X > GAME_WIDTH || activeEnemies[i].getPosition().Y < 0 || activeEnemies[i].getPosition().Y > GAME_HEIGHT)
                                    activeEnemies[i].die();
                            }

                            //Handle enemy bullet movement
                            activeEnemies[i].manageBullets.updatePosition();

                            List<Bullet> enemyBullets = activeEnemies[i].manageBullets.getActiveBullets();
                            for (int c = 0; c < enemyBullets.Count; c++)
                            {
                                //If bullet hits player, reset level
                                if (player.isDead() && particleManager.isEmpty())
                                    reset();

                                if (enemyBullets[c].getHitBox().Intersects(player.getHitBox()))
                                {
                                    //Spawn particles in middle of player
                                    Vector2 deadParticlePos = new Vector2(player.getPosition().X + player.getSprite().Width / 2, player.getPosition().Y + player.getSprite().Height / 2);

                                    for (int currPart = 0; currPart < 100; currPart++)
                                    {
                                        //Shoot particles randomly
                                        Vector2 deadParticleDir = new Vector2();
                                        if (rand.NextDouble() > .5)
                                            deadParticleDir.X = (float)rand.NextDouble() * 2;
                                        else
                                            deadParticleDir.X = (float)rand.NextDouble() * -2;

                                        double randY = rand.NextDouble();
                                        if (randY > .33 && randY < .66)
                                            deadParticleDir.Y = (float)rand.NextDouble() * 2;
                                        else if (randY < .33)
                                            deadParticleDir.Y = 0;
                                        else
                                            deadParticleDir.Y = (float)rand.NextDouble() * -2;

                                        //Generate particles
                                        particleManager.generateParticles(player, deadParticlePos, deadParticleDir, 1, 300);

                                        //Remove player from screen & mark as dead
                                        player.setPosition(-50, -50);
                                        player.die();
                                    }
                                }

                                //If bullet is out of bounds, recycle it
                                if (enemyBullets[c].position.X < 0 || enemyBullets[c].position.Y < 0 || enemyBullets[c].position.X > GAME_WIDTH || enemyBullets[c].position.Y > GAME_HEIGHT)
                                    activeEnemies[i].manageBullets.recycleBullet(enemyBullets[c]);
                            }

                            //If all of NPC's bullets are out of screen and it's dead, remove from activeEnemies
                            if (activeEnemies.Count > 0 && activeEnemies[i].manageBullets.getActiveBullets().Count == 0 && activeEnemies[i].isDead())
                            {
                                activeEnemies.Remove(activeEnemies[i]);
                                i--;
                            }
                        }
                    }
                }

                //Update particles, if any
                particleManager.updateParticles();
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            string currentEvent = level.getCurrentEvent();
            if (currentEvent == "DIALOGUE")
            {
                Dialogue currentDialogue = level.getDialogue();

                if(!currentDialogue.isDialogueOver())
                    currentDialogue.draw(spriteBatch);
            }
            else if (currentEvent == "WAVE")
            {

                //Draw player and their bullets
                spriteBatch.Draw(player.getSprite(), player.getPosition(), Color.White);

                List<Bullet> bullets = player.manageBullets.getActiveBullets();
                for (int i = 0; i < bullets.Count; i++)
                {
                    spriteBatch.Draw(bullets[i].sprite, bullets[i].position, null, bullets[i].spriteColor, bullets[i].rotationAngle, bullets[i].origin, 1.0f, SpriteEffects.None, 0f);
                }

                //Draw enemies and their bullets
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    //Gray the sprite if enemy was hit
                    if (!activeEnemies[i].isDead() && activeEnemies[i].hit)
                    {
                        spriteBatch.Draw(activeEnemies[i].getSprite(), activeEnemies[i].getPosition(), Color.Black);
                        activeEnemies[i].hit = false;
                    }
                    else if (!activeEnemies[i].isDead() && !activeEnemies[i].hit)
                        spriteBatch.Draw(activeEnemies[i].getSprite(), activeEnemies[i].getPosition(), Color.White);

                    List<Bullet> enemyBullets = activeEnemies[i].manageBullets.getActiveBullets();
                    for (int c = 0; c < enemyBullets.Count; c++)
                    {
                        spriteBatch.Draw(enemyBullets[c].sprite, enemyBullets[c].position, null, enemyBullets[c].spriteColor, enemyBullets[c].rotationAngle, enemyBullets[c].origin, 1.0f, SpriteEffects.None, 0f);
                    }
                }

                //Draw particles, if any
                List<ParticleManager.Particle> particles = particleManager.getParticles();
                for (int i = 0; i < particles.Count; i++)
                {
                    spriteBatch.Draw(particles[i].particleSprite, particles[i].position, Color.White);
                }
            }
        }

    }
}
