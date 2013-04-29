using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class Dialogue
    {
        public class Speaker
        {
            public bool left;
            public Texture2D sprite;
            public Queue<string> lines = new Queue<string>();
        }

        private ContentManager Content;

        private SpriteFont textFont;
        private List<Speaker> speakers = new List<Speaker>();
        private Queue<int> script = new Queue<int>();
        private Texture2D textbox;

        public Dialogue(StreamReader dialogueData, SpriteFont sf, ContentManager cm)
        {
            Content = cm;
            textFont = sf;
            textbox = Content.Load<Texture2D>(@"sprites\textbox");

            bool readingSpeakers = true;
            string line;
            while ((line = dialogueData.ReadLine()) != null)
            {
                if (readingSpeakers && line == "DIALOGUE_BEGIN")
                {
                    readingSpeakers = false;
                    line = dialogueData.ReadLine();
                }

                if (readingSpeakers)
                {
                    char[] spaceDelim = new char[] { ' ' };
                    string[] lineParts = line.Split(spaceDelim);

                    if(lineParts[0] == "SPEAKER")
                    {
                        Speaker aSpeaker = new Speaker();

                        if (lineParts[1] == "LEFT")
                            aSpeaker.left = true;
                        else
                            aSpeaker.left = false;

                        aSpeaker.sprite = Content.Load<Texture2D>(@"sprites\" + lineParts[2]);

                        speakers.Add(aSpeaker);
                    }
                }
                else
                {
                    double dummy;
                    Double.TryParse(line.Substring(0,1), out dummy);
                    int speakerID = (int)dummy;

                    script.Enqueue(speakerID);
                    speakers[speakerID].lines.Enqueue(line.Substring(1, line.Length - 1));
                }
            }
        }

        //Check if all lines of the script have been displayed
        public bool isDialogueOver()
        {
            return (script.Count == 0);
        }

        //Move on to next line
        public void nextLine()
        {
            int prevSpeaker = script.Dequeue();
            speakers[prevSpeaker].lines.Dequeue();
        }

        public void update()
        {
            
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Speaker currentSpeaker = speakers[script.Peek()];
            Vector2 leftSpeakerPos = new Vector2(0,200);
            Vector2 rightSpeakerPos = new Vector2(794,200);
            Vector2 textPos = new Vector2(5,550);

            //Draw speaker
            if(currentSpeaker.left)
                spriteBatch.Draw(currentSpeaker.sprite, leftSpeakerPos, Color.White);
            else
                spriteBatch.Draw(currentSpeaker.sprite, rightSpeakerPos, Color.White);

            //Draw textbox
            spriteBatch.Draw(textbox, new Vector2(0, 540), Color.White);
            
            //Draw text
            spriteBatch.DrawString(textFont, currentSpeaker.lines.Peek(), textPos, Color.White, 0f, new Vector2(0,0), 2, SpriteEffects.None, 0f);
        }
    }
}