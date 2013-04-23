using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class Level
    {
        struct Wave
        {
            public Queue<NPC> waveEnemies;
            public int waveCount;
        }

        private ContentManager Content;

        private Queue<Wave> waves = new Queue<Wave>();
        private int enemySpawnDelay = 2000; //Delay between when new enemies are spawned
        private int numWaves;

        //used to temporarily hold wave data before enqueuing
        private Wave temp_wave;

        public Level(StreamReader levelData, ContentManager cm)
        {
            Content = cm;

            string line;
            while ((line = levelData.ReadLine()) != null)
            {
                if (line == "NEW_WAVE")
                {
                    temp_wave = new Wave();
                    temp_wave.waveEnemies = new Queue<NPC>();
                }
                else if (line == "END_WAVE")
                {
                    waves.Enqueue(temp_wave);
                }
                else
                {
                    StreamReader sr = new StreamReader(@"monsters\" + line);
                    temp_wave.waveEnemies.Enqueue(createNPC(sr));
                }
            }
        }

        public int getNumWaves()
        {
            return waves.Count;
        }

        private NPC createNPC(StreamReader npcData)
        {
            NPC npc = new NPC();
            string line;
            double dummy;

            //npc data
            Vector2 pos = new Vector2();
            Queue<string> actions = new Queue<string>();

            while ((line = npcData.ReadLine()) != null)
            {
                char[] spaceDelim = new char[] { ' ' };
                string[] lineParts = line.Split(spaceDelim);

                switch (lineParts[0])
                {
                    case "SPRITE":
                        npc.setSprite(Content.Load<Texture2D>(@"sprites\" + lineParts[1]));
                        break;
                    case "XPOS":
                        Double.TryParse(lineParts[1], out dummy);
                        pos.X = (float)dummy;
                        break;
                    case "YPOS":
                        Double.TryParse(lineParts[1], out dummy);
                        pos.Y = (float)dummy;
                        break;
                    case "HP":
                        Double.TryParse(lineParts[1], out dummy);
                        npc.setHP((int)dummy);
                        break;
                    case "PATTERN":
                        for (int i = 1; i < lineParts.Count(); i++)
                        {
                            switch (lineParts[i])
                            {
                                case "BackAndForth":
                                    npc.manageBullets.addPattern(PatternManager.BackAndForth);
                                    break;
                                case "Cross":
                                    npc.manageBullets.addPattern(PatternManager.Cross);
                                    break;
                                case "Spaz":
                                    npc.manageBullets.addPattern(PatternManager.Spaz);
                                    break;
                                case "Mix":
                                    npc.manageBullets.addPattern(PatternManager.Mix);
                                    break;
                                case "CrossRotate":
                                    npc.manageBullets.addPattern(PatternManager.CrossRotate);
                                    break;
                                case "BladeRotate":
                                    npc.manageBullets.addPattern(PatternManager.BladeRotate);
                                    break;
                                case "Straight":
                                    npc.manageBullets.addPattern(PatternManager.Straight);
                                    break;
                                case "TightBackAndForth":
                                    npc.manageBullets.addPattern(PatternManager.TightBackAndForth);
                                    break;
                            }
                        }
                        break;
                    case "TARGETS":
                        switch (lineParts[1])
                        {
                            case "1":
                                npc.targetsPlayer = true;
                                break;
                            case "0":
                                npc.targetsPlayer = false;
                                break;
                        }
                        break;
                    case "FIREDELAY":
                        Double.TryParse(lineParts[1], out dummy);
                        npc.manageBullets.setDelay(dummy);
                        break;
                    case "ACTIONS":
                        for (int i = 1; i < lineParts.Count(); i++)
                        {
                            actions.Enqueue(lineParts[i]);
                        }
                        break;
                }
            }

            npc.setPosition(pos);
            npc.setActions(actions);
            npc.addBullets(Content.Load<Texture2D>(@"sprites\round_bullet"), 100, Color.Orange);

            return npc;
        }

        public bool isLevelOver()
        {
            return (waves.Count == 0);
        }

        //Returns npc in wave
        public NPC getEnemy()
        {
            return waves.Peek().waveEnemies.Dequeue();
        }

        //Push current wave out of queue
        public void discardCurrentWave()
        {
            if (waves.Count > 0)
            {
                waves.Dequeue();
                numWaves--;
            }
        }

        //Returns number of waves remaining in queue
        public int wavesRemaining()
        {
            return waves.Count;
        }

        //Check if wave is over
        public bool isCurrentWaveEmpty()
        {
            return (waves.Peek().waveEnemies.Count == 0);
        }

        //Monster spawning delays
        public void setSpawnDelay(int ms)
        {
            enemySpawnDelay = ms;
        }

        public double getSpawnDelay()
        {
            return enemySpawnDelay;
        }
    }
}