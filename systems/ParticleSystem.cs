using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BuildingFood.systems
{
    public class ParticleSystem : ISystem
    {
        public BuildingFoodGame ParentGame { get; set; }

        private List<Particle> particles;
        private Random particleRandom;
        
        public ParticleSystem(BuildingFoodGame parentGame)
        {
            ParentGame = parentGame;
            particles = new List<Particle>();
            particleRandom = new Random();
        }

        public void Update(GameTime gameTime)
        {
            List<Particle> particlesToRemove = new List<Particle>();
            
            for (var index = 0; index < particles.Count; index++)
            {
                var particle = particles[index];
                
                particle.LifeSpan -= (float) (gameTime.ElapsedGameTime.TotalSeconds);
                if (particle.LifeSpan <= 0.0f)
                {
                    particlesToRemove.Add(particle);
                }
                else
                {
                    var particlePosition = particle.Position;
                    var particleVelocity = particle.Velocity;
                    var particleRotation = particle.Rotation;

                    particleVelocity.Y += particle.Weight * 98.0f * (float) (gameTime.ElapsedGameTime.TotalSeconds);
                    particlePosition.Y += particleVelocity.Y * (float) (gameTime.ElapsedGameTime.TotalSeconds);
                    particlePosition.X += particleVelocity.X * (float) (gameTime.ElapsedGameTime.TotalSeconds);
                    particleRotation += particle.RotationDirection * (float) (gameTime.ElapsedGameTime.TotalSeconds);

                    particle.Position = particlePosition;
                    particle.Rotation = particleRotation;
                }

                particles[index] = particle;
            }

            foreach (var particle in particlesToRemove)
            {
                particles.Remove(particle);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var particle in particles)
            {
                var particleTexture = ParentGame.Texture[particle.TextureName]; 
                batch.Draw(ParentGame.Texture[particle.TextureName], particle.Position, null, Color.White, particle.Rotation, new Vector2(particleTexture.Width / 2, particleTexture.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
        }

        private void AddParticle(Particle particle)
        {
            particles.Add(particle);
        }
        
        public void AddRandomParticle(Vector2 position, ParticleTypes type, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Particle particle = new Particle();
                int textureIndex = Math.Clamp(particleRandom.Next(0, 3), 0, 2);
                particle.Position = position;
                switch (type)
                {
                    case ParticleTypes.Fire:
                        particle.RotationDirection = 0.0f;
                        particle.Weight = (float) particleRandom.NextDouble() * 100.0f + 100.0f;
                        particle.LifeSpan = (float) particleRandom.NextDouble() * 2.0f + 1.0f;
                        particle.Velocity = new Vector2((float) particleRandom.NextDouble() * 300.0f - 150.0f, (float) particleRandom.NextDouble() * 300.0f - 300.0f);
                        particle.TextureName = "particle_fire" + (textureIndex + 1);
                            break;
                    case ParticleTypes.Tools:
                        particle.RotationDirection = (float) particleRandom.NextDouble() * 5.0f;
                        particle.Weight = (float) particleRandom.NextDouble() * 100.0f + 100.0f;
                        particle.LifeSpan = (float) particleRandom.NextDouble() * 2.0f + 1.0f;
                        particle.Velocity = new Vector2((float) particleRandom.NextDouble() * 300.0f - 150.0f, (float) particleRandom.NextDouble() * 300.0f - 300.0f);
                        particle.TextureName = "particle_knife";
                        if (textureIndex == 1) particle.TextureName = "particle_whisk";
                        if (textureIndex == 2) particle.TextureName = "particle_fork";
                        break;
                    case ParticleTypes.Money:
                        particle.RotationDirection = (float) particleRandom.NextDouble() * 5.0f;
                        particle.Weight = (float) particleRandom.NextDouble() * 100.0f + 100.0f;
                        particle.LifeSpan = (float) particleRandom.NextDouble() * 2.0f + 1.0f;
                        particle.Velocity = new Vector2((float) particleRandom.NextDouble() * 300.0f - 150.0f, (float) particleRandom.NextDouble() * 300.0f - 300.0f);
                        particle.TextureName = "particle_coin";
                        if (textureIndex == 1) particle.TextureName = "particle_note";
                        if (textureIndex == 2) particle.TextureName = "particle_bag";
                        break;
                    default:
                        return;
                }
                AddParticle(particle);
            }
        }
    }

    public struct Particle
    {
        public string TextureName;
        public Vector2 Position;
        public Vector2 Velocity;
        public float Rotation;
        public float RotationDirection;
        public float Weight;
        public float LifeSpan;
    }

    public enum ParticleTypes
    {
        Tools, Fire, Money
    }
}