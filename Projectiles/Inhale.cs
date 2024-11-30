
using System;
using System.Collections.Generic;
using System.Net;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Inhale : IProjectile, ICollidable
    {
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        public bool CollisionActive { get; private set;} = true;
        private bool IsActive;
        private bool IsLeft;
        public IPlayer player { get; private set; }
        private SoundInstance sound;
        public Inhale(Vector2 pos, bool isLeft, Player kirby)
        {
            Position = pos;
            IsLeft = isLeft;
            player = kirby;
            IsActive = true;
            ObjectManager.Instance.AddProjectile(this);
            //ObjectManager.Instance.RegisterDynamicObject(this);
            sound = SoundManager.CreateInstance("inhale");
            sound.Play();
        }
        public void OnCollide(KirbyType kirbyType)
        {
           player.EatEnemy(kirbyType);
        }
        public void EndAttack()
        {
            CollisionActive = false;
            IsActive = false;
            sound.Stop();
        }
        public bool IsDone()
        {
            return !IsActive;
        }
        public void Update()
        {
            Position = player.GetKirbyPosition();
        }
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + (IsLeft ? Constants.HitBoxes.NORMAL_OFFSET_LEFT: Constants.HitBoxes.NORMAL_OFFSET_RIGHT); 
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
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