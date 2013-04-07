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
        private double fireDelay = 20;
        private double prevShootTime = 0;

        private List<Pattern> attackPatterns = new List<Pattern>();
        private int currentPattern = 0;

        public PatternManager patternManager = new PatternManager();

        public BulletManager()
        {
        }

        public void addBullet(Bullet b)
        {
            bullets.Enqueue(b);
        }

        public void addPattern(Pattern p)
        {
            attackPatterns.Add(p);
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

        //Release bullet with pattern
        public void releaseBullet(Vector2 pos)
        {
            if (bullets.Count > 0)
            {
                Bullet b = bullets.Dequeue();

                b.position = new Vector2((int)pos.X, (int)pos.Y);
                b.setFired(true);

                patternManager.applyPattern(b, attackPatterns[currentPattern], 0f);
                
                activeBullets.Add(b);
            }
            
        }

        //Release tracking bullet
        public void releaseBullet(Vector2 pos, float rotAngle)
        {
            if (bullets.Count > 0)
            {
                Bullet b = bullets.Dequeue();

                b.position = new Vector2((int)pos.X, (int)pos.Y);
                b.setFired(true);

                b.rotationAngle = rotAngle;

                patternManager.applyPattern(b, attackPatterns[currentPattern], rotAngle, true);

                activeBullets.Add(b);
            }

        }

        public void recycleBullet(Bullet b)
        {
            activeBullets.Remove(b);
            b.setFired(false);
            b.position = new Vector2(0, 0);
            bullets.Enqueue(b);
        }
    }
}