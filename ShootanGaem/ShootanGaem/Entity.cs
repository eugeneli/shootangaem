using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class Entity
    {
        public Texture2D sprite;

        protected int health;
        protected float speed;

        protected Vector2 position;
        protected Vector2 origin;

        public BulletManager manageBullets = new BulletManager();

        public Entity()
        {
            health = 100;
            speed = 10;
        }

        public Entity(Texture2D spr, Vector2 pos)
        {
            health = 100;
            speed = 10;

            sprite = spr;

            position = pos;
            origin = new Vector2(spr.Width / 2, spr.Height / 2);
        }

        public void setSprite(Texture2D spr)
        {
            sprite = spr;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public Vector2 getOrigin()
        {
            return origin;
        }

        public void setPosition(Vector2 pos)
        {
            position = pos;
        }

        public void setPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void setHP(int hp)
        {
            health = hp;
        }

        public void moveLeft()
        {
            position.X -= speed;
        }

        public void moveRight()
        {
            position.X += speed;
        }

        public void moveUp()
        {
            position.Y -= speed;
        }

        public void moveDown()
        {
            position.Y += speed;
        }

        //Bullet-related methods
        public void fire(double currentTime)
        {
            if (manageBullets.fireDelayOver(currentTime))
            {
                Vector2 middleOfSprite = new Vector2(position.X + sprite.Width / 2, position.Y);
                manageBullets.releaseBullet(middleOfSprite);
            }
        }

        public int getBulletCount()
        {
            return manageBullets.numActiveBullets();
        }

        public void addBullets(Texture2D sprite, int numBullets, Color sprColor)
        {
            for (int i = 0; i < numBullets; i++)
                manageBullets.addBullet(new Bullet(sprite, position, sprColor));
        }

        public void addPattern(Pattern p)
        {
            manageBullets.addPattern(p);
        }

    }
}
