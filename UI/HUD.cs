using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;

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
        private readonly Player player;

        public HUD(Player player)
        {
            // Initialize player
            this.player = player;

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
            if (keyboardState.IsKeyDown(Keys.D8))
            {
                ActivatePowerup("ui_power_beam", gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D9))
            {
                ActivatePowerup("ui_power_spark", gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D0))
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

            // Activate the selected powerup and reset timer for new powerup
            powerupActive[powerupKey] = true;
            powerupTimers[powerupKey] = 0f;
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

        public void DrawScore(SpriteBatch spriteBatch)
        {
            int score = Game1.Instance.manager.Score;

            string scoreText = score.ToString().PadLeft(8, '0'); // Add padding to ensure it is always 8 digits

            // Draw the current score
            for (int i = 0; i < scoreText.Length; i++)
            {
                char digitChar = scoreText[i];
                int digit = int.Parse(digitChar.ToString());

                float xPosition = 176 + i * 8;
                hudElements[$"ui_{digit}"].Draw(new Vector2(xPosition, 147), spriteBatch);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var powerupKey in powerupPositions.Keys)
            {
                if (powerupActive[powerupKey])
                {
                    hudElements[powerupKey].Draw(powerupPositions[powerupKey], spriteBatch);
                }
            }

            // Draw lives
            hudElements["ui_lives"].Draw(new Vector2(57, 147), spriteBatch);

            int displayLives = player.lives - 1; // Adjust to show 02 for 3 lives, 01 for 2 lives, etc.
            string displayLivesText = displayLives.ToString().PadLeft(2, '0'); // Format as two digits

            int livesTens = int.Parse(displayLivesText[0].ToString());
            int livesOnes = int.Parse(displayLivesText[1].ToString());

            hudElements[$"ui_{livesTens}"].Draw(new Vector2(80, 147), spriteBatch);
            hudElements[$"ui_{livesOnes}"].Draw(new Vector2(88, 147), spriteBatch);

            // Draw health bar based on player.health
            int healthX = 104;
            for (int i = 0; i < Constants.Kirby.MAX_HEALTH; i++)
            {
                string healthSprite = i < player.health ? "ui_healthbar_1" : "ui_healthbar_0";
                hudElements[healthSprite].Draw(new Vector2(healthX, 146), spriteBatch);
                healthX += 8;
            }

            DrawScore(spriteBatch);
        }
    }
}
