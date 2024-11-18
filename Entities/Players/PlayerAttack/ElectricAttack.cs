
using System;
using System.Net;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class ElectricAttack : IProjectile, ICollidable
    {
        
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        public bool CollisionActive { get; private set;} = true;
        private SoundInstance sound;

        public ElectricAttack(Vector2 pos, bool isLeft)
        {
            Position = pos;
            ObjectManager.Instance.AddProjectile(this);
            ObjectManager.Instance.RegisterDynamicObject(this);
            sound = SoundManager.CreateInstance("kirbysparkattack");
            sound.Play();
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
        public Vector2 GetPosition()
        {
            return Position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //uneeded
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
        }

        public void EndAttack()
        {
            CollisionActive = false;
            sound.Stop();
        }
    }
}