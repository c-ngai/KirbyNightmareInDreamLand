using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using System.Diagnostics;
using KirbyNightmareInDreamLand.Entities.Enemies;

namespace KirbyNightmareInDreamLand.Particles
{
    public class EnemyExplode : IParticle
    {
        private IEnemy enemy; // the enemy to track to

        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private bool completed;

        public EnemyExplode(IEnemy _enemy)
        {
            enemy = _enemy;
            sprite = SpriteFactory.Instance.CreateSprite("particle_enemyexplode");
            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void Update()
        {
            if (frameCount < Constants.Particle.ENEMYEXPLODE_MAX_FRAMES)
            {
                position = enemy.GetPosition() + Constants.Particle.ENEMYEXPLODE_OFFSET;
                sprite.Update();
                frameCount++;
            }
            else
            {
                sprite = null;
                completed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(position, spriteBatch);
        }

        public bool IsDone()
        {
            return completed;
        }
    }
}
