
using System;
using System.Net;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Slide : IProjectile, ICollidable
    {
        public bool CollisionActive { get; private set;} = true;
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        private float startingX;
        private static int slideDistance = 70;
        private bool IsLeft;
        public Slide(Vector2 pos, bool isLeft)
        {
            IsLeft= isLeft;
            Position =pos;
            startingX = pos.X;
            ObjectManager.Instance.RegisterDynamicObject(this);
        }
        public string GetObjectType()
        {
            return "Projectile";
        }
        public void OnCollide()
        {
            //nothing
        }
        public void EndAttack()
        {
            CollisionActive = false;
        }
        public bool IsDone()
        {
            float distanceMoved = Math.Abs(Position.X - startingX);
            if(distanceMoved > slideDistance)
            {
                EndAttack();
                return true;
            }
            return false;
        }
        public void Update()
        {
            GetHitBox();
        }
        public void Update(Player kirby)
        {
            Position = kirby.GetKirbyPosition();
            GetHitBox();
        }

        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + (IsLeft ? Constants.HitBoxes.SLIDE_OFFSET_LEFT: Constants.HitBoxes.SLIDE_OFFSET_RIGHT); 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.SLIDE_WIDTH, Constants.HitBoxes.SLIDE_HEIGHT);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }


    }
}