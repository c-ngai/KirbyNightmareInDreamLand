
using System;
using System.Net;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Slide : IProjectile, ICollidable
    {
        IPlayer player;
        public bool CollisionActive { get; private set;} = true;
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        private float startingX;
        private bool IsLeft;
        private double timer = 0;
        public Slide(Vector2 pos, bool isLeft, IPlayer player)
        {
            this.player = player;
            IsLeft= isLeft;
            Position =pos;
            startingX = pos.X;
            ObjectManager.Instance.AddProjectile(this);
            ObjectManager.Instance.RegisterDynamicObject(this);
            SoundManager.Play("slide");
        }
        public void EndAttack()
        {
            CollisionActive = false;
        }
        public bool IsDone()
        {
            timer += Game1.Instance.time.ElapsedGameTime.TotalSeconds; 
            if(timer > Constants.Kirby.SLIDE_TIME)
            {
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
            Position = player.GetKirbyPosition();
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