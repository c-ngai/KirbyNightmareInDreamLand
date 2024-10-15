
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class ElectricAttack : IProjectile
    {
        private ICollidable collidable;
        
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}

        public ElectricAttack(Vector2 pos, bool isLeft)
        {
            collidable = new PlayerAttackCollisionHandler(pos, "Spark", isLeft);
            Position = pos;
        }
        public void EndAttack()
        {
            collidable.DestroyHitBox();
        }
        public bool IsDone()
        {
            return true;
        }

        public void Update()
        {
           collidable.UpdateBoundingBox(Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }
    }
}