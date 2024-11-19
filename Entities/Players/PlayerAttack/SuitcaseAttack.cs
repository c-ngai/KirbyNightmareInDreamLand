using System;
using System.Net;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Suitcase : IProjectile, ICollidable
    {
        public bool CollisionActive { get; private set;} = true;
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        private float startingX;
        private static int slideDistance = 70;
        private bool IsLeft;
        public Suitcase(Vector2 pos, bool isLeft)
        {
            IsLeft= isLeft;
            Position =pos;
            startingX = pos.X;
            ObjectManager.Instance.RegisterDynamicObject(this);
            SoundManager.Play("slide");
        }
        public void EndAttack()
        {
            CollisionActive = false;
        }
        public bool IsDone()
        {
            float distanceMoved = Math.Abs(Position.X - startingX);
            if(distanceMoved > slideDistance || !CollisionActive)
            {
                EndAttack();
                return true;
            }
            return false;
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
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
        public Vector2 GetPosition()
        {
            return Position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }


    }
}