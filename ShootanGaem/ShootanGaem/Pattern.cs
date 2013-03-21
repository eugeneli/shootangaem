using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShootanGaem
{
    class Pattern
    {
        public int tick = 0;
        public int maxTick = -1;

        public bool swerve = false;
        public bool spaz = false;
        public bool once = false;

        public int patternDelay = 200;

        public float rotationAngle;
        public float rotationIncrement;
    }
}