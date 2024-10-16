
using System.Collections.Generic;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Inhale : IProjectile, ICollidable
    {
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        private bool IsLeft;
        public Inhale(Vector2 pos, bool isLeft)
        {
            Position = pos;
            IsLeft = isLeft;
            CollisionDetection.Instance.RegisterDynamicObject(this);
            
        }
        public void OnCollide()
        {
            //switch to mouthful kirby
        }
        public void EndAttack()
        {
            CollisionDetection.Instance.RemoveDynamicObject(this);
        }
        public bool IsDone()
        {
            return true;
        }
        public void Update()
        {
            GetHitBox();
        }
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + (IsLeft ? Constants.HitBoxes.NORMA_OFFSET_LEFT: Constants.HitBoxes.NORMAL_OFFSET_RIGHT); 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.NORMAL_SIZE, Constants.HitBoxes.NORMAL_SIZE);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }


    }
}