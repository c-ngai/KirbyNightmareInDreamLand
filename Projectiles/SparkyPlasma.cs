using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using System.Runtime.InteropServices;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class SparkyPlasma : IProjectile, ICollidable
    {
        public IPlayer player { get => null; } // this projectile never originates from a player
        //private readonly Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private int framesActive;
        public bool CollisionActive { get; set; } = true;
        public bool IsActive { get; private set; } = true;

        public Vector2 Position
        {
            get => position;  
            set => position = value; 
        }

        public Vector2 Velocity
        {
            get => velocity;           
            set => velocity = value;   
        }

        public SparkyPlasma(Vector2 startPosition)
        {
            Position = startPosition;
            framesActive = 0;
            //projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_sparky_plasma");
            ObjectManager.Instance.AddProjectile(this);
        }

        public CollisionType GetCollisionType()
        {
            return CollisionType.EnemyAttack;
        }

        public void Update()
        {
            if (IsActive)
            {
                //projectileSprite.Update();
                framesActive++;

                if (framesActive >= Constants.Sparky.ATTACK_TIME)
                {
                    EndAttack();
                }
            }
            else
            {
                CollisionActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public bool IsDone()
        {
            return !IsActive;
        }

        

        public virtual Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.SPARKY_ATTACK_WIDTH / 2;
            float y = pos.Y - Constants.HitBoxes.SPARKY_ATTACK_HEIGHT + Constants.HitBoxes.SPARKY_ATTACK_OFFSET;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }

        public virtual Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.SPARKY_ATTACK_WIDTH, Constants.HitBoxes.SPARKY_ATTACK_HEIGHT);
        }
        public Vector2 GetPosition()
        {
            return Position;
        }

        public void EndAttack()
        {
            IsActive = false;  
            CollisionActive = false;    
        }
    }
}
