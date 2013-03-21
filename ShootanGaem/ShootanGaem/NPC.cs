using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class NPC : Entity
    {
        private Queue<string> actions = new Queue<string>(); //actions to reach goal
        private string prevAction;
        private int timesRedoAction = 0;

        private Vector2 target;

        public NPC() : base() { }

        public NPC(Texture2D spr, Vector2 pos) : base(spr, pos)
        {
        }

        public void setActions(Queue<string> actionsQueue)
        {
            actions = new Queue<string>(actionsQueue);
        }

        public void setTarget(Vector2 tar)
        {
            target = tar;
        }

        public void processCommand(string command, double currentTime)
        {
            switch (command)
            {
                case "LEFT":
                    moveLeft();
                    break;
                case "RIGHT":
                    moveRight();
                    break;
                case "UP":
                    moveUp();
                    break;
                case "DOWN":
                    moveDown();
                    break;
                case "DOWNLEFT":
                    moveDown();
                    moveLeft();
                    break;
                case "DOWNRIGHT":
                    moveDown();
                    moveRight();
                    break;
                case "FIRE":
                    fire(currentTime);
                    break;
            }
        }

        public void doAction(double currentTime)
        {
            if (actions.Count > 0)
            {
                double dummy = 0; //Use dummy double to check if next command is a number. If it is, repeat previous command
                if (timesRedoAction > 0) //Check if the string in queue represents an integer
                {
                    processCommand(prevAction, currentTime);
                    timesRedoAction--;
                }
                else if (Double.TryParse(actions.Peek(), out dummy))
                {
                    timesRedoAction = (int)dummy;
                    actions.Dequeue();
                }
                else
                {
                    string currentAction = actions.Dequeue();
                    processCommand(currentAction, currentTime);
                    prevAction = currentAction;
                }
            }
        }
        
    }
}