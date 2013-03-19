using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShootanGaem
{
    class Level
    {
        private Queue<Entity> enemies = new Queue<Entity>();
        private int enemySpawnDelay = 2000; //Delay between when new enemies are spawned
        
        public int currentTime;
        public int prevSpawnTime;

        public Level() { }

        public void addEnemy(Entity e)
        {
            enemies.Enqueue(e);
        }

        public Entity getEnemy()
        {
            return enemies.Dequeue();
        }

        public void setSpawnDelay(int ms)
        {
            enemySpawnDelay = ms;
        }
    }
}