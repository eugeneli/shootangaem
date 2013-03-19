using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class BulletManager
    {
        private Queue<Bullet> bullets = new Queue<Bullet>();
        private List<Bullet> activeBullets = new List<Bullet>();
        private double fireDelay = 200;
        private double prevShootTime = 0;

        public PatternManager patternManager = new PatternManager();

        public BulletManager()
        {
        }

        public void addBullet(Bullet b)
        {
            bullets.Enqueue(b);
        }

        public void setDelay(double ms)
        {
            fireDelay = ms;
        }

        public int numActiveBullets()
        {
            return activeBullets.Count();
        }

        public bool fireDelayOver(double currentTime)
        {
            if (currentTime - prevShootTime > fireDelay || prevShootTime == 0)
            {
                prevShootTime = currentTime;
                return true;
            }
            else
                return false;
        }

        public List<Bullet> getActiveBullets()
        {
            return activeBullets;
        }

        public void updatePosition()
        {
            for (int i = 0; i < activeBullets.Count; i++)
            {
                if (activeBullets[i].isFired())
                {
                    activeBullets[i].move();
                }
            }
        }

        public void releaseBullet(Vector2 pos)
        {
            Bullet b = bullets.Dequeue();

            b.setPosition((int)pos.X, (int)pos.Y);
            b.setFired(true);

            //HARDCODING IN THE PATTERN IN HERE FOR TESTING!!!!!!!!!!!!!!!!!!!! CURRENTLY HAVE .Spiral AND .BackAndForth implemented! ALAN IMPLEMENT MORE
            patternManager.applyPattern(b, PatternManager.Spiral);

            activeBullets.Add(b);
        }

        public void recycleBullet(Bullet b)
        {
            activeBullets.Remove(b);
            b.setFired(false);
            b.setPosition(0, 0);
            bullets.Enqueue(b);
        }
    }
}