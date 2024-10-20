using System;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace KirbyNightmareInDreamLand.Entities.PowerUps
{
    public class PowerUp : ICollidable
	{

        protected Vector2 position; //Where power up is drawn on screen
		protected string powerUpType; //What kind of power up are you? Currently only have 1, but #maintainablity 
        protected Sprite powerUpSprite; 

        public bool CollisionActive { get; set; } = true;


        public PowerUp(Vector2 startPosition, string type)
        {
            //Initialize all variables
            position = startPosition;
            powerUpType = type;
            powerUpSprite = SpriteFactory.Instance.CreateSprite(powerUpType);
            ObjectManager.Instance.RegisterDynamicObject(this);

        }
        public string GetObjectType()
        {
            return "PowerUp";
        }

        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.TILE_WIDTH / 2;
            float y = pos.Y - Constants.HitBoxes.TILE_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }

        public Vector2 Position
        {
            //Returns position on screen
            get { return position; }
            set { position = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw if power up hasn't been used yet
            if (CollisionActive)
            {
                powerUpSprite.Draw(position, spriteBatch);
            }
            else
            {
                ObjectManager.Instance.RemoveDynamicObject(this); // Deregister if item is used 
            }
        }

        public void Update()
        {
            if (CollisionActive)
            {
                powerUpSprite.Update();
            }
        }

        public void UsePowerUp()
        {
            CollisionActive = false;
            // more power up affect logic here 
        }

        public string GetCollisionType()
        {
            return "PowerUp";
        }
    }
}
