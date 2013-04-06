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
        private Random rand = new Random();
        private List<Particle> particles = new List<Particle>();

        public ParticleManager(GraphicsDevice g)
        {
            graphicsDevice = g;
        }

        public void generateParticles(Entity ent)
        {
            Color[] colorData = new Color[ent.getSprite().Width * ent.getSprite().Height];
            ent.getSprite().GetData(colorData);

            //Select 4 random pixels to create particle with
            Color[] randColors = new Color[4];
            randColors[0] = colorData[rand.Next(ent.getSprite().Width * ent.getSprite().Height - 1)];
            randColors[1] = colorData[rand.Next(ent.getSprite().Width * ent.getSprite().Height - 1)];
            randColors[2] = colorData[rand.Next(ent.getSprite().Width * ent.getSprite().Height - 1)];
            randColors[3] = colorData[rand.Next(ent.getSprite().Width * ent.getSprite().Height - 1)];

            //Spawn particles at base of sprite
            Vector2 pos = new Vector2(ent.getPosition().X+ent.getSprite().Width/2, ent.getPosition().Y+ent.getSprite().Height/2);

            for (int i = 0; i < 5; i++)
                particles.Add(createParticle(randColors, pos));
        }

        public Particle createParticle(Color[] colors, Vector2 pos)
        {
            Texture2D tex = new Texture2D(graphicsDevice, 2, 2);
            Particle p = new Particle();
            
            //Put pixel data into particle sprite
            tex.SetData(colors);
            p.particleSprite = tex;

            //Set life of particle
            p.life = 50;

            //Set speed of particle
            p.speed = rand.Next(5, 10);

            //Set spawn of particle
            p.position = pos;

            //Set direction
            p.direction = new Vector2();
            if(rand.NextDouble() > .5)
                p.direction.X = (float)rand.NextDouble() * 2;
            else
                p.direction.X = (float)rand.NextDouble() * -2;

            p.direction.Y = 1f;

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