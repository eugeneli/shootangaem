using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class PatternManager
    {
        public static Pattern Spiral = new Pattern();
        public static Pattern BackAndForth = new Pattern();

        public PatternManager()
        {
            //Set values for Spiral pattern
            Spiral.rotationAngle = (float)(Math.PI / 4);
            Spiral.rotationIncrement = (float)(Math.PI / 16);

            //Set values for BackAndForth pattern
            BackAndForth.rotationAngle = (float)(-3*Math.PI/4);
            BackAndForth.rotationIncrement = (float)(Math.PI / 16);
            BackAndForth.maxTick = 10;
        }

        //Should be better way of detecting which Pattern to do instead of nesting a bunch of conditionals :|
        public void applyPattern(Bullet b, Pattern p)
        {
            if (p.maxTick == -1) //Do this for Spiral pattern
            {
                b.rotationAngle = p.rotationAngle + (p.rotationIncrement * p.tick);
                b.direction.X = (float)Math.Cos(b.rotationAngle);
                b.direction.Y = (float)Math.Sin(b.rotationAngle);

                p.tick++;
            }
            else //Do back and forth pattern
            {
                if (p.tick >= 0 && p.tick < p.maxTick) //Shoot 10 bullets while rotating right
                {
                    b.rotationAngle = p.rotationAngle + (p.rotationIncrement * p.tick);
                    b.direction.X = (float)Math.Cos(b.rotationAngle);
                    b.direction.Y = (float)Math.Sin(b.rotationAngle);

                    p.tick++;
                    
                    if (p.tick >= p.maxTick)
                        p.tick *= -1;
                }
                else //Shoot 10 bullets while rotating left (and repeat)
                {
                    b.rotationAngle = p.rotationAngle - (p.rotationIncrement * p.tick);
                    b.direction.X = (float)Math.Cos(b.rotationAngle);
                    b.direction.Y = (float)Math.Sin(b.rotationAngle);

                    p.tick++;
                }
            }
        }
    }
}
