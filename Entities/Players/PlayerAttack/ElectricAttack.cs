
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class ElectricAttack : IProjectile
    {
        private ICollidable collidable;
        
        private int size =  36;
        public Vector2 Position {get; private set;}
        public Vector2 Velocity {get; private set;}

        public ElectricAttack(Vector2 pos, bool isLeft)
        {
            // System.Console.WriteLine("-------elecric-------");
            // System.Console.WriteLine(pos);
            // System.Console.WriteLine(GetYPos(pos));
            // System.Console.WriteLine(GetXPos(pos));
            collidable = new PlayerAttackCollisionHandler((int) pos.X, GetYPos(pos));
            Position = new Vector2 (GetXPos(pos), GetYPos(pos));
        }
        public int GetYPos(Vector2 pos)
        {
            return (int)pos.Y - Constants.HitBoxes.ENTITY_HEIGHT / 2; 
            
        }
        public int GetXPos(Vector2 pos)
        {
            return (int)pos.X - (int)(Constants.HitBoxes.ENTITY_HEIGHT * 1.5); 
            
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