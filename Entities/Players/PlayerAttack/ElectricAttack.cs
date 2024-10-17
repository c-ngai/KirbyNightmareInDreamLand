
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class ElectricAttack : IProjectile, ICollidable
    {
        
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        public bool CollisionActive { get; private set;} = true;
        private bool IsLeft;
        public ElectricAttack(Vector2 pos, bool isLeft)
        {
            IsLeft = isLeft;
            Position = pos;
            CollisionDetection.Instance.RegisterDynamicObject(this);
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
            return pos + Constants.HitBoxes.SPARK_OFFSET; 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.SPARK_SIZE, Constants.HitBoxes.SPARK_SIZE);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }

        public void EndAttack()
        {
            CollisionActive = false;
        }
    }
}