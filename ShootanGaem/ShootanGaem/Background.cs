using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootanGaem
{
    class Background
    {
        private Rectangle viewPort;
        private Rectangle origViewPort;
        private Texture2D background;
        private Vector2 direction; //Scrolling direction
        private int screenWidth;
        private int screenHeight;
        private int scrollSpeed = 1;
        private bool reverse = true;

        private double prevUpdate = 0;
        private double updateDelay = 30;
        
        //generic background
        public Background(Texture2D bg, int width, int height)
        {
            background = bg;
            screenWidth = width;
            screenHeight = height;

            viewPort = new Rectangle(0, 0, width, height);
            origViewPort = new Rectangle(0, 0, width, height);
            direction = new Vector2(0, 1);
        }

        //Level background
        public Background(Texture2D bg, int width, int height, bool level)
        {
            background = bg;
            screenWidth = width;
            screenHeight = height;

            viewPort = new Rectangle(0, 0, width, height);
            origViewPort = new Rectangle(0, 0, width, height);
            direction = new Vector2(0, 1);

            setScroll("UP");
            setScrollSpeed(10);
            setReverse(false);
        }

        public void update(GameTime gametime)
        {
            double currentTime = gametime.TotalGameTime.TotalMilliseconds;

            if (currentTime - prevUpdate > updateDelay || prevUpdate == 0)
            {
                viewPort.X += (int)direction.X * scrollSpeed;
                viewPort.Y += (int)direction.Y * scrollSpeed;

                prevUpdate = currentTime;

                if (reverse)
                {
                    if (viewPort.X + screenWidth > background.Width || viewPort.Y + screenHeight > background.Height || viewPort.X < 0 || viewPort.Y < 0)
                        direction *= -1;
                }
                else //If don't reverse viewport, then loop
                {
                    if (viewPort.Y <= 0)
                        viewPort = origViewPort;
                }
            }

            
        }

        public void setScroll(string direc)
        {
            switch (direc)
            {
                case "DOWN":
                    direction.X = 0;
                    direction.Y = 1;
                    break;
                case "UP":
                    direction.X = 0;
                    direction.Y = -1;
                    break;
                case "DOWNLEFT":
                    direction.X = 1;
                    direction.Y = 1;
                    break;
            }
        }

        public void setScrollSpeed(int spd)
        {
            scrollSpeed = spd;
        }

        public void setViewport(Rectangle rect)
        {
            viewPort = rect;
            origViewPort = viewPort;
        }

        public void setReverse(bool doRev)
        {
            reverse = doRev;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), viewPort, Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0);
        }
    }
}
