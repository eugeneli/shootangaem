using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class Bullet : Entity
    {
        public float rotationAngle = 0;
        public Vector2 direction = new Vector2(0, -1);

        private int baseDamage = 10;
        private bool fired = false; //Determines of bullet should be drawn

        //Constructors for bullets
        public Bullet(Texture2D spr, Vector2 pos)
            : base(spr, pos)
        {
            sprite = spr;
            speed = 20;
        }

        public Bullet(Texture2D spr, Vector2 pos, int dmg, float spd)
            : base(spr, pos)
        {
            sprite = spr;
            baseDamage = dmg;
            speed = spd;
        }

        public void setFired(bool isFired)
        {
            fired = isFired;
        }

        public bool isFired()
        {
            return fired;
        }

        public int getDamage()
        {
            return baseDamage;
        }

        //Moves the bullet
        public void move()
        {
            //Need to get rid of the decimals or the bounding rectangle will get misplaced over time since Rectangles only take ints
            position.X += (int)(direction.X * speed);
            position.Y += (int)(direction.Y * speed);

            //boundingRect.X += (int)(direction.X * speed);
            //boundingRect.Y += (int)(direction.Y * speed);
        }

        //Reset bullet for future use
        public void reset()
        {
            fired = false;
        }
    }
}
