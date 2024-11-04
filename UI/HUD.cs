using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.UI
{
    public class HUD
    {
        private readonly Dictionary<string, Sprite> hudElements;

        public HUD()
        {
            // Initialize HUD elements
            hudElements = new Dictionary<string, Sprite>
            {
                { "ui_power_beam", SpriteFactory.Instance.CreateSprite("ui_power_beam") },
                { "ui_power_spark", SpriteFactory.Instance.CreateSprite("ui_power_spark") },
                { "ui_power_fire", SpriteFactory.Instance.CreateSprite("ui_power_fire") },
                { "ui_lives", SpriteFactory.Instance.CreateSprite("ui_lives") },
                { "ui_healthbar_1", SpriteFactory.Instance.CreateSprite("ui_healthbar_1") },
                { "ui_healthbar_0", SpriteFactory.Instance.CreateSprite("ui_healthbar_0") },
                { "ui_0", SpriteFactory.Instance.CreateSprite("ui_0") },
                { "ui_1", SpriteFactory.Instance.CreateSprite("ui_1") },
                { "ui_2", SpriteFactory.Instance.CreateSprite("ui_2") },
                { "ui_3", SpriteFactory.Instance.CreateSprite("ui_3") },
                { "ui_4", SpriteFactory.Instance.CreateSprite("ui_4") },
                { "ui_5", SpriteFactory.Instance.CreateSprite("ui_5") },
                { "ui_6", SpriteFactory.Instance.CreateSprite("ui_6") },
                { "ui_7", SpriteFactory.Instance.CreateSprite("ui_7") },
                { "ui_8", SpriteFactory.Instance.CreateSprite("ui_8") },
                { "ui_9", SpriteFactory.Instance.CreateSprite("ui_9") }
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            // Draw Powerup (Will add updating/sliding funcitonality later)
            hudElements["ui_power_beam"].Draw(new Vector2(0, 147), spriteBatch);

            // Draw Lives Icon and Number (WIll add updating funcitonality for numbers later)
            hudElements["ui_lives"].Draw(new Vector2(57, 147), spriteBatch);
            hudElements["ui_0"].Draw(new Vector2(80, 147), spriteBatch);
            hudElements["ui_2"].Draw(new Vector2(88, 147), spriteBatch);

            // Draw Health Bar (Will add updating functionality later)
            hudElements["ui_healthbar_1"].Draw(new Vector2(104, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(112, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(120, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(128, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(136, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(144, 146), spriteBatch);

            // Draw Score Numbers (will add updating functionality later)
            hudElements["ui_0"].Draw(new Vector2(176, 147), spriteBatch);
            hudElements["ui_0"].Draw(new Vector2(184, 147), spriteBatch); 
            hudElements["ui_0"].Draw(new Vector2(192, 147), spriteBatch); 
            hudElements["ui_0"].Draw(new Vector2(200, 147), spriteBatch); 
            hudElements["ui_0"].Draw(new Vector2(208, 147), spriteBatch); 
            hudElements["ui_0"].Draw(new Vector2(216, 147), spriteBatch);
            hudElements["ui_0"].Draw(new Vector2(224, 147), spriteBatch);
            hudElements["ui_0"].Draw(new Vector2(232, 147), spriteBatch);

        }
    }
}
