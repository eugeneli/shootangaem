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
        public Color spriteColor;

        private int baseDamage = 10;
        private bool fired = false; //Determines of bullet should be drawn
        
        //Constructors for bullets
        public Bullet(Texture2D spr, Vector2 pos, Color sprColor) : base(spr, pos)
        {
            sprite = spr;
            speed = 20;
            spriteColor = sprColor;
        }

        public Bullet(Texture2D spr, Vector2 pos, Color sprColor, int dmg, float spd) : base(spr, pos)
        {
            sprite = spr;
            baseDamage = dmg;
            speed = spd;
            spriteColor = sprColor;
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
            //position.X += (float)(direction.X * speed);
            //position.Y += (float)(direction.Y * speed);

            position += (speed*direction);

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
