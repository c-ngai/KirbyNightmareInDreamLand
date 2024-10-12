
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class Slide : IProjectile
    {
        private ICollidable collidable;
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}
        private int size =  36;
        public Slide(Vector2 pos, bool isLeft)
        {
            collidable = new PlayerAttackCollisionHandler(GetXPos(pos, isLeft), GetYPos(pos));
            Position = new Vector2 (GetXPos(pos, isLeft), GetYPos(pos));
        }
        public int GetYPos(Vector2 pos)
        {
            return (int)pos.Y - Constants.HitBoxes.ENTITY_HEIGHT; 
            
        }
        public int GetXPos(Vector2 pos, bool isLeft)
        {
            if(isLeft)
            {
                return (int)pos.X - Constants.HitBoxes.ENTITY_WIDTH  - 6;
            } else {
                return (int)pos.X +Constants.HitBoxes.ENTITY_WIDTH/2 + 3;
            }
            
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