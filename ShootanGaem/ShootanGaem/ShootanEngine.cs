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
        ContentManager Content;
        KeyboardState keyboardState;
        SpriteBatch spriteBatch;

        private int GAME_WIDTH;
        private int GAME_HEIGHT;

        private Level level;
        private Player player;

        private List<NPC> activeEnemies = new List<NPC>();

        public ShootanEngine(int width, int height, SpriteBatch sb, ContentManager cm)
        {
            GAME_WIDTH = width;
            GAME_HEIGHT = height;
            spriteBatch = sb;
            Content = cm;
        }

        //Creates player object
        public void createPlayer(Texture2D sprite, Vector2 pos)
        {
            player = new Player(sprite, pos);
            player.manageBullets.setDelay(100);
        }

        //Changes player's fire delay
        public void setPlayerDelay(float ms)
        {
            player.manageBullets.setDelay(ms);
        }

        //Add bullets to player
        public void addPlayerBullets(Texture2D sprite, int numBullets, Color sprColor)
        {
            player.addBullets(sprite, numBullets, sprColor);
        }

        public void addPlayerPattern(Pattern p)
        {
            player.addPattern(p);
        }

        //Load Level from file
        public void loadLevel(StreamReader levelData)
        {
            level = new Level(levelData, Content);
        }

        public void update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            /* --------------
             * Player Updates
             * --------------
             */
            //Player movement
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                player.moveUp();
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                player.moveDown();
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.moveLeft();
            }
            if (keyboardState.IsKeyDown(Keys.Right))
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
            }


            /* --------------
             * NPC Updates
             * --------------
             */
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
                }
                else //If there are active enemies, let them do their stuff
                {
                    for (int i = 0; i < activeEnemies.Count; i++)
                    {
                        activeEnemies[i].doAction(gameTime.TotalGameTime.TotalMilliseconds);

                        //Handle enemy bullet movement
                        activeEnemies[i].manageBullets.updatePosition();

                        List<Bullet> enemyBullets = activeEnemies[i].manageBullets.getActiveBullets();
                        for (int c = 0; c < enemyBullets.Count; c++)
                        {
                            //If bullet is out of bounds, recycle it
                            if (enemyBullets[c].position.X < 0 || enemyBullets[c].position.Y < 0 || enemyBullets[c].position.X > GAME_WIDTH || enemyBullets[c].position.Y > GAME_HEIGHT)
                                activeEnemies[i].manageBullets.recycleBullet(enemyBullets[c]);
                        }
                    }
                }
            }
        }

        public void draw()
        {
            spriteBatch.Begin();

            //Draw player and their bullets
            spriteBatch.Draw(player.sprite, player.getPosition(), Color.White);

            List<Bullet> bullets = player.manageBullets.getActiveBullets();
            for (int i = 0; i < bullets.Count; i++)
            {
                spriteBatch.Draw(bullets[i].sprite, bullets[i].position, null, bullets[i].spriteColor, bullets[i].rotationAngle, bullets[i].origin, 1.0f, SpriteEffects.None, 0f);
            }

            //Draw enemies and their bullets
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                spriteBatch.Draw(activeEnemies[i].sprite, activeEnemies[i].getPosition(), Color.White);

                List<Bullet> enemyBullets = activeEnemies[i].manageBullets.getActiveBullets();
                for (int c = 0; c < enemyBullets.Count; c++)
                {
                    spriteBatch.Draw(enemyBullets[c].sprite, enemyBullets[c].position, null, enemyBullets[c].spriteColor, enemyBullets[c].rotationAngle, enemyBullets[c].origin, 1.0f, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.End();
        }

    }
}
