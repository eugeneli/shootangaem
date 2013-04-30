using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class Player : Entity
    {
        bool AWAKENMYCHILDANDBEHOLDTHEGLORYTHATISYOURBIRTHRIGHT = false;

        public Player(Texture2D spr, Vector2 pos) : base(spr, pos)
        {
        }

        public void setSpeed(int spd)
        {
            speed = spd;
        }

        public bool isGod()
        {
            return AWAKENMYCHILDANDBEHOLDTHEGLORYTHATISYOURBIRTHRIGHT;
        }

        public void toggleGod()
        {
            AWAKENMYCHILDANDBEHOLDTHEGLORYTHATISYOURBIRTHRIGHT = !AWAKENMYCHILDANDBEHOLDTHEGLORYTHATISYOURBIRTHRIGHT;
        }
    }
}
