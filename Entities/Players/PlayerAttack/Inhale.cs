
using System;
using System.Collections.Generic;
using System.Net;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Inhale : IProjectile, ICollidable
    {
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        public bool CollisionActive { get; private set;} = true;
        private bool IsLeft;
        private Player player;
        public Inhale(Vector2 pos, bool isLeft, Player kirby)
        {
            Position = pos;
            IsLeft = isLeft;
            player = kirby;
            ObjectManager.Instance.RegisterDynamicObject(this);
            
        }
        public void OnCollide()
        {
           player.SwallowEnemy();
        }
        public void EndAttack()
        {
            CollisionActive = false;
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
            return pos + (IsLeft ? Constants.HitBoxes.NORMAL_OFFSET_LEFT: Constants.HitBoxes.NORMAL_OFFSET_RIGHT); 
        }
        public string GetObjectType()
        {
            return "PlayerAttack";
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.NORMAL_SIZE, Constants.HitBoxes.NORMAL_SIZE);
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