
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Inhale : IProjectile
    {
        private ICollidable collidable;
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        public Inhale(Vector2 pos, bool isLeft)
        {
            collidable = new PlayerAttackCollisionHandler(pos, "Normal", isLeft);
            Position = pos;
        }
        public void OnCollide()
        {
            //switch to mouthful kirby
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