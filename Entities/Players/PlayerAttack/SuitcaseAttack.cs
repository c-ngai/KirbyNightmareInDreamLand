using System;
using System.Net;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Suitcase : IProjectile, ICollidable, IExplodable
    {
        public bool CollisionActive { get; private set;} = true;
        public Vector2 Position {get; private set;}
        public Vector2 KirbyPosition {get; private set;}
        public Vector2 Velocity {get; private set;}
        private float startingX;
        private bool IsLeft;
        private int[][] StageOnePositionsRight = new int[3][]{new int[]{-8, -6},new int[]{-4, -12},new int[]{-15, 0}};
        private int stage =1;
        private int substage = 0;
        private int timer = 0;
        public Suitcase(Vector2 pos, bool isLeft)
        {
            Velocity = isLeft ? Constants.KirbySuitcase.SUITCASE_VEL_LEFT :Constants.KirbySuitcase.SUITCASE_VEL_RIGHT;
            IsLeft= isLeft;
            KirbyPosition =pos;
            startingX = pos.X;
            ObjectManager.Instance.AddProjectile(this);
            ObjectManager.Instance.RegisterDynamicObject(this);
        }
        public void Explode()
        {
            //CollisionActive = false;
        }
        public void EndAttack()
        {
            stage = 3;
            //CollisionActive = false;
        }
        public bool IsDone()
        {
            if(Position.Y > Game1.Instance.Level.CurrentRoom.Height)
            {
                return true;
            }
            if(!CollisionActive)
            {
                return true;
            }
            return false;
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerSuitcase;
        }
        public void Update()
        {
            if(stage == 1)
            {
                Vector2 pos = new Vector2(KirbyPosition.X + StageOnePositionsRight[substage][0],KirbyPosition.Y + StageOnePositionsRight[substage][01] );
                Position = pos;
                substage++;
                if(substage == 2)
                {
                    stage ++;
                }
            } else if (stage == 2)
            {
                Vector2 tempVelocity = Velocity;
                tempVelocity.Y += Constants.Physics.GRAVITY;
                Velocity = tempVelocity;
                Position += Velocity;
            } else 
            {
                timer ++;
                if(timer > 100)
                {
                    CollisionActive = false;
                }   
            }
            GetHitBox();
        }
        
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + (IsLeft ? Constants.HitBoxes.SLIDE_OFFSET_LEFT: Constants.HitBoxes.SLIDE_OFFSET_RIGHT); 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            if(stage == 3)
            {
                return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, 
                Constants.KirbySuitcase.SUITCASE_EXPLODE_WIDTH, Constants.KirbySuitcase.SUITCASE_EXPLODE_HEIGHT);
            } else {
                return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, 
                Constants.KirbySuitcase.SUITCASE_WIDTH, Constants.KirbySuitcase.SUITCASE_HEIGHT);
            }  
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