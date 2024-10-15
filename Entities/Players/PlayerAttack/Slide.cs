
using System;
using System.Net;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Slide : IProjectile
    {
        private ICollidable collidable;
        public Vector2 Position {get;}
        public Vector2 Velocity {get; private set;}

        private Vector2 position;
        private float startingX;
        private static int slideDistance = 70;
        public Slide(Vector2 pos, bool isLeft)
        {
            collidable = new PlayerAttackCollisionHandler(pos, "Slide", isLeft);
            position =pos;
            startingX = pos.X;
        }
        public void OnCollide()
        {
            //nothing
        }
        public void EndAttack()
        {
            collidable.DestroyHitBox();
        }
        public bool IsDone()
        {
            float distanceMoved = Math.Abs(Position.X - startingX);
            if(distanceMoved > slideDistance)
            {
                collidable.DestroyHitBox();
                return true;
            }
            return false;
        }
        public void Update()
        {
            collidable.UpdateBoundingBox(position);
        }
        public void Update(Player kirby)
        {
            position = kirby.GetKirbyPosition();
            collidable.UpdateBoundingBox(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }


    }
}