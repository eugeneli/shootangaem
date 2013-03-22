using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShootanGaem
{
    class SGButton
    {
        public string buttonText = "Default text";
        private Texture2D currentTexture;
        private Texture2D buttonUpTexture;
        private Texture2D buttonDownTexture;
        private Rectangle buttonRectangle;
        private MouseState prevMouseState;

        private SpriteFont textFont;
        private Vector2 textPos;

        public bool clicked;

        public SGButton(Texture2D unclicked, Texture2D clicked, Rectangle rect, SpriteFont font)
        {
            currentTexture = unclicked;
            buttonUpTexture = unclicked;
            buttonDownTexture = clicked;

            buttonRectangle = rect;

            textFont = font;
            textPos = new Vector2(rect.X, rect.Y);
        }

        public int getWidth()
        {
            return buttonRectangle.Width;
        }

        public int getHeight()
        {
            return buttonRectangle.Height;
        }

        public void setText(string text)
        {
            buttonText = text;
        }

        public void setPosition(int x, int y)
        {
            buttonRectangle.X = x;
            buttonRectangle.Y = y;

            Vector2 textDimensions = textFont.MeasureString(buttonText);
            textPos.X = (buttonRectangle.X + buttonRectangle.Width / 2) - textDimensions.X;
            textPos.Y = (buttonRectangle.Y + buttonRectangle.Height / 2) - textDimensions.Y;
        }

        public Vector2 getPosition()
        {
            return new Vector2(buttonRectangle.X, buttonRectangle.Y);
        }

        public void update(MouseState mouseState)
        {
            Point mousePoint = new Point(mouseState.X, mouseState.Y);
            if (buttonRectangle.Contains(mousePoint))
            {
                currentTexture = buttonDownTexture;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (prevMouseState.LeftButton == ButtonState.Released)
                        clicked = true;
                }
            }
            else
                currentTexture = buttonUpTexture;

            prevMouseState = mouseState;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Draw string in center of button
            Vector2 textDimensions = textFont.MeasureString(buttonText);

            textPos.X = (buttonRectangle.X + buttonRectangle.Width / 2) - textDimensions.X;
            textPos.Y = (buttonRectangle.Y + buttonRectangle.Height / 2) - textDimensions.Y;

            spriteBatch.Draw(currentTexture, buttonRectangle, Color.White);
            spriteBatch.DrawString(textFont, buttonText, textPos, Color.White, 0f, new Vector2(0,0), 2, SpriteEffects.None, 0f);
        }
    }
}