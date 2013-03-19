using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShootanGaem
{
    class ShootanEngine
    {
        KeyboardState keyboardState;
        SpriteBatch spriteBatch;

        private int GAME_WIDTH;
        private int GAME_HEIGHT;

        private Player player;
        private List<Entity> enemies = new List<Entity>();

        public ShootanEngine(int width, int height, SpriteBatch sb)
        {
            GAME_WIDTH = width;
            GAME_HEIGHT = height;
            spriteBatch = sb;
        }

        //Creates player object
        public void createPlayer(Texture2D sprite, Vector2 pos)
        {
            player = new Player(sprite, pos);
            player.manageBullets.setDelay(100);
        }

        public void setPlayerDelay(float ms)
        {
            player.manageBullets.setDelay(ms);
        }

        //Add bullets to player
        public void addPlayerBullets(Texture2D sprite, int numBullets, Color sprColor)
        {
            for (int i = 0; i < numBullets; i++)
                player.manageBullets.addBullet(new Bullet(sprite, player.getPosition(), sprColor));
        }

        //Generic add bullets to entity
        public void addBullets(Entity ent, Texture2D sprite, int numBullets, Color sprColor)
        {
            for (int i = 0; i < numBullets; i++)
                ent.manageBullets.addBullet(new Bullet(sprite, ent.getPosition(), sprColor));
        }

        public void update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

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

            //Handle bullet movement
            player.manageBullets.updatePosition();

            List<Bullet> bullets = player.manageBullets.getActiveBullets();
            for (int i = 0; i < bullets.Count; i++)
            {
                //If bullet is out of bounds, recycle it
                if (bullets[i].getPosition().X < 0 || bullets[i].getPosition().Y < 0 || bullets[i].getPosition().X > GAME_WIDTH || bullets[i].getPosition().Y > GAME_HEIGHT)
                    player.manageBullets.recycleBullet(bullets[i]);
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
                spriteBatch.Draw(bullets[i].sprite, bullets[i].getPosition(), null, bullets[i].spriteColor, bullets[i].rotationAngle, bullets[i].getOrigin(), 1.0f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
        }

    }
}
