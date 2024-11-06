﻿using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace KirbyNightmareInDreamLand.UI
{
    public class HUD
    {
        private readonly Dictionary<string, Sprite> hudElements;
        private Dictionary<string, Vector2> powerupPositions;
        private Dictionary<string, bool> powerupActive;
        private Dictionary<string, float> powerupTimers;
        private const float slideSpeed = 1f; // Speed at which sprites slide up/down
        private const float stayTime = 2f; // Time in seconds to stay at position (0, 115)

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

            // Initialize powerup positions and active states
            powerupPositions = new Dictionary<string, Vector2>
            {
                { "ui_power_beam", new Vector2(0, 147) },
                { "ui_power_spark", new Vector2(0, 147) },
                { "ui_power_fire", new Vector2(0, 147) }
            };

            powerupActive = new Dictionary<string, bool>
            {
                { "ui_power_beam", false },
                { "ui_power_spark", false },
                { "ui_power_fire", false }
            };

            powerupTimers = new Dictionary<string, float>
            {
                { "ui_power_beam", 0f },
                { "ui_power_spark", 0f },
                { "ui_power_fire", 0f }
            };
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            // Handle key inputs for powerups
            if (keyboardState.IsKeyDown(Keys.D8)) // '8' key
            {
                ActivatePowerup("ui_power_beam", gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D9)) // '9' key
            {
                ActivatePowerup("ui_power_spark", gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D0)) // '0' key
            {
                ActivatePowerup("ui_power_fire", gameTime);
            }

            // Update powerup positions and timers
            foreach (var powerupKey in powerupPositions.Keys)
            {
                UpdatePowerupPosition(powerupKey, gameTime);
            }
        }

        private void ActivatePowerup(string powerupKey, GameTime gameTime)
        {
            // Deactivate all powerups first
            foreach (var key in powerupActive.Keys)
            {
                powerupActive[key] = false;
            }

            // Activate the selected powerup and reset its timer
            powerupActive[powerupKey] = true;
            powerupTimers[powerupKey] = 0f; // Reset timer for new powerup
        }

        private void UpdatePowerupPosition(string powerupKey, GameTime gameTime)
        {
            if (powerupActive[powerupKey])
            {
                // Timer logic for staying at (0, 115) for a few seconds
                powerupTimers[powerupKey] += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (powerupTimers[powerupKey] < stayTime)
                {
                    // Slide up from the bottom
                    if (powerupPositions[powerupKey].Y > 115)
                    {
                        powerupPositions[powerupKey] -= new Vector2(0, slideSpeed); // Move up
                    }
                }
                else
                {
                    // After stay time, slide down to (0, 147)
                    if (powerupPositions[powerupKey].Y < 147)
                    {
                        powerupPositions[powerupKey] += new Vector2(0, slideSpeed); // Move down
                    }
                }
            }
            else
            {
                // Slide back down if inactive
                if (powerupPositions[powerupKey].Y < 147)
                {
                    powerupPositions[powerupKey] += new Vector2(0, slideSpeed); // Move down
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw only the active powerup
            foreach (var powerupKey in powerupPositions.Keys)
            {
                if (powerupActive[powerupKey])
                {
                    hudElements[powerupKey].Draw(powerupPositions[powerupKey], spriteBatch);
                    break; // Only one powerup is visible at a time
                }
            }

            // Draw other HUD elements (Lives, Health, Score)
            hudElements["ui_lives"].Draw(new Vector2(57, 147), spriteBatch);
            hudElements["ui_0"].Draw(new Vector2(80, 147), spriteBatch);
            hudElements["ui_2"].Draw(new Vector2(88, 147), spriteBatch);

            hudElements["ui_healthbar_1"].Draw(new Vector2(104, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(112, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(120, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(128, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(136, 146), spriteBatch);
            hudElements["ui_healthbar_1"].Draw(new Vector2(144, 146), spriteBatch);

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
