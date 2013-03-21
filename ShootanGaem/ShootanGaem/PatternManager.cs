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
        public static Pattern Cross = new Pattern();
        public static Pattern Spaz = new Pattern();
        public static Pattern Mix = new Pattern();
        public static Pattern CrossRotate = new Pattern();
        public static Pattern BladeRotate = new Pattern();
        public static Pattern Test = new Pattern();

        public PatternManager()
        {
            //Set values for Spiral pattern
            Spiral.rotationAngle = (float)(Math.PI / 4);
            Spiral.rotationIncrement = (float)(Math.PI / 16);

            //Set values for BackAndForth pattern
            BackAndForth.rotationAngle = (float)(Math.PI / 4);
            BackAndForth.rotationIncrement = (float)(Math.PI / 16);
            BackAndForth.maxTick = 10;

            //Set values for Cross pattern
            Cross.rotationAngle = (float)(Math.PI / 4);
            Cross.rotationIncrement = (float)(Math.PI / 2);

            //Set values for Spaz pattern
            Spaz.rotationAngle = (float)(Math.PI / 4);
            Spaz.rotationIncrement = (float)(Math.PI / 128);
            Spaz.spaz = true;

            //Set values for Mix pattern
            Mix.rotationAngle = (float)(Math.PI / 4);
            Mix.rotationIncrement = (float)(Math.PI / 2);
            Mix.swerve = true;

            //Set values for CrossRotate
            CrossRotate.rotationAngle = (float)(Math.PI / 4);
            CrossRotate.rotationIncrement = (float)(Math.PI / 2);
            CrossRotate.once = true;

            //Set values for BladeRotate
            BladeRotate.rotationAngle = (float)(Math.PI / 4);
            BladeRotate.rotationIncrement = (float)(Math.PI);
            BladeRotate.once = true;

            //Set values for Test pattern
            Test.rotationAngle = (float)(Math.PI / 4);
            Test.rotationIncrement = (float)(Math.PI);
            Test.once = true;
        }

        //Should be better way of detecting which Pattern to do instead of nesting a bunch of conditionals :|
        public void applyPattern(Bullet b, Pattern p)
        {
            if (p.once == true)
            {
                //Increments the rotation increment once to mess with bullet pattern
                p.rotationIncrement += (float)0.05;
                p.once = false;
            }
            if (p.swerve == true)
            {
                //Increments the rotation increment to mess with bullet pattern
                p.rotationIncrement += (float)0.001;
               // Console.WriteLine(p.rotationIncrement);
                //b.rotationAngle = p.rotationAngle + (p.rotationIncrement * p.tick);
            }
            if (p.spaz == true)
            {
                //Increments the rotation increment to mess with bullet pattern
                p.rotationIncrement += (float)1.5;
                //b.rotationAngle = p.rotationAngle + (p.rotationIncrement * p.tick);
            }
            if (p.maxTick == -1) //Do this for Spiral/Cross pattern
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