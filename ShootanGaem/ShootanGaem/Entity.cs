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

        public Entity(Texture2D spr, Vector2 pos)
        {
            health = 100;
            speed = 10;

            sprite = spr;

            position = pos;
            origin = new Vector2(spr.Width / 2, spr.Height / 2);
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public Vector2 getOrigin()
        {
            return origin;
        }

        public void setPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
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

    }
}
