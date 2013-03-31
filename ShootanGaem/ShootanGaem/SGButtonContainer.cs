using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShootanGaem
{
    class SGButtonContainer
    {
        private List<SGButton> buttons = new List<SGButton>();
        private bool slidingDown, slidingLeft, slidingRight = false;
        private Vector2 goal; //The stopping position for the menu sliding

        private const int slideSpeed = 20;

        public SGButtonContainer() {}

        public void add(SGButton sg)
        {
            buttons.Add(sg);
        }

        public void slideDown()
        {
            slidingDown = true;
        }

        public void slideLeft()
        {
            slidingLeft = true;
        }

        public void slideRight()
        {
            slidingRight = true;
        }

        public void setPosition(int x, int y)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                //Console.WriteLine(x);
                buttons[i].setPosition(x, y + (i*80));
            }
        }

        public void setSlideGoal(int x, int y)
        {
            goal = new Vector2(x, y);
        }

        public void update(MouseState mouseState)
        {
            //Update all buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].update(mouseState);
            }

            //Do sliding, if any
            if (slidingDown && buttons[0].getPosition().Y < goal.Y)
            {
                setPosition((int)buttons[0].getPosition().X, (int)buttons[0].getPosition().Y + slideSpeed);

                if (buttons[0].getPosition().Y >= goal.Y)
                    slidingDown = false;
            }
            else
                slidingDown = false;

            if (slidingLeft && buttons[0].getPosition().X > goal.X)
            {
                setPosition((int)buttons[0].getPosition().X - slideSpeed, (int)buttons[0].getPosition().Y);

                if (buttons[0].getPosition().X <= goal.X)
                    slidingLeft = false;
            }
            else
                slidingLeft = false;

            if (slidingRight && buttons[0].getPosition().X < goal.X)
            {
                setPosition((int)buttons[0].getPosition().X + slideSpeed, (int)buttons[0].getPosition().Y);

                if (buttons[0].getPosition().X >= goal.X)
                    slidingRight = false;
            }
            else
                slidingRight = false;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].draw(spriteBatch);
            }
        }
    }
}