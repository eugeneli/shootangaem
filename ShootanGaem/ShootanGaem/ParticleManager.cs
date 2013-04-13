using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootanGaem
{
    class ParticleManager
    {
        public class Particle
        {
            public Texture2D particleSprite;
            public int life;
            public int speed;
            public Vector2 position = new Vector2();
            public Vector2 direction = new Vector2();
        }

        private GraphicsDevice graphicsDevice;
        private Random rand;
        private List<Particle> particles = new List<Particle>();

        public ParticleManager(GraphicsDevice g, Random r)
        {
            graphicsDevice = g;
            rand = r;
        }

        public void generateParticles(Entity ent, Vector2 particleSource, Vector2 particleDir, int numParticles, int particleLife)
        {
            Color[] colorData = new Color[ent.getSprite().Width * ent.getSprite().Height];
            ent.getSprite().GetData(colorData);

            //Select random pixels to create particle with
            Color[] randColors = new Color[4];
            for(int i = 0; i < 4; i++)
                randColors[i] = colorData[rand.Next(ent.getSprite().Width * ent.getSprite().Height - 1)];

            for (int i = 0; i < numParticles; i++)
                particles.Add(createParticle(randColors, particleSource, particleDir, particleLife));
        }

        public Particle createParticle(Color[] colors, Vector2 pos, Vector2 particleDir, int particleLife)
        {
            Texture2D tex = new Texture2D(graphicsDevice, 2, 2);
            Particle p = new Particle();
            
            //Put pixel data into particle sprite
            tex.SetData(colors);
            p.particleSprite = tex;

            //Set life of particle
            p.life = particleLife;

            //Set speed of particle
            p.speed = rand.Next(5, 10);

            //Set spawn of particle
            p.position = pos;

            //Set direction
            p.direction = particleDir;

            return p;
        }

        public void updateParticles()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].position += particles[i].speed*particles[i].direction;

                particles[i].life--;

                if (particles[i].life <= 0)
                {
                    particles.Remove(particles[i]);
                    i--;
                }
            }
        }

        public List<Particle> getParticles()
        {
            return particles;
        }
    }
}