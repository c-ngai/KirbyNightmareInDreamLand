using System;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace KirbyNightmareInDreamLand.Entities.PowerUps
{
    public class PowerUp
	{

        protected Vector2 position; //Where power up is drawn on screen
		protected string powerUpType; //What kind of power up are you? Currently only have 1, but #maintainablity 
        protected Sprite powerUpSprite; 
        protected bool notUsed; // has the power up been used by a player yet? 

        public PowerUp(Vector2 startPosition, string type)
        {
            //Initialize all variables
            position = startPosition;
            notUsed = true;
            powerUpType = type;
            powerUpSprite = SpriteFactory.Instance.CreateSprite(powerUpType);
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
            if (notUsed)
            {
                powerUpSprite.Draw(position, spriteBatch);
            }
        }

        public void Update()
        {
            if (notUsed)
            {
                powerUpSprite.Update();
            }
        }

        public void UsePowerUp()
        {
            notUsed = false;
            // more power up affect logic here 
        }
    }
}
