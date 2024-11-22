using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using System.Runtime.Serialization;

namespace KirbyNightmareInDreamLand.UI
{
    public class HUD
    {
        private readonly Dictionary<string, Sprite> hudElements;
        private Dictionary<string, Vector2> powerupPositions;
        private Dictionary<string, bool> powerupActive;
        private Dictionary<string, float> powerupTimers;
        private int playerIndex;
        private IPlayer targetPlayer;

        public HUD(int playerIndex)
        {
            this.playerIndex = playerIndex;
            
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
                { "ui_power_beam", Constants.HUD.POWERUP_INIT_POS },
                { "ui_power_spark", Constants.HUD.POWERUP_INIT_POS },
                { "ui_power_fire", Constants.HUD.POWERUP_INIT_POS }
            };

            powerupActive = new Dictionary<string, bool>
            {
                { "ui_power_beam", false },
                { "ui_power_spark", false },
                { "ui_power_fire", false }
            };

            powerupTimers = new Dictionary<string, float>
            {
                { "ui_power_beam", Constants.HUD.POWERUP_INIT_TIMER },
                { "ui_power_spark", Constants.HUD.POWERUP_INIT_TIMER },
                { "ui_power_fire", Constants.HUD.POWERUP_INIT_TIMER }
            };
        }

        public void Update()
        {
            // If player of this Camera's index exists, target it and update the camera to track it
            if (playerIndex < ObjectManager.Instance.Players.Count)
            {
                targetPlayer = ObjectManager.Instance.Players[playerIndex];
            }

            if (targetPlayer != null)
            {
                if(Game1.Instance.Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GamePowerChangeState"))
                {
                    Console.WriteLine("here2");
                    string power = targetPlayer.GetPowerUp().ToString().ToLower();
                    ActivatePowerup("ui_power_" + power);
                }
               
            }
        }

        public void ActivatePowerup(string powerupKey)
        {
            string oldPowerUp = "nor";
            foreach (var key in powerupActive.Keys)
            {
                if (powerupActive[key] == true)
                {
                    oldPowerUp = key;
                }
            }

            if(!powerupKey.Equals(oldPowerUp))
            {
                // Deactivate all powerups first
                foreach (var key in powerupActive.Keys)
                {
                    powerupActive[key] = false;
                }

                // Activate the selected powerup and reset timer for new powerup
                powerupActive[powerupKey] = true;
                powerupTimers[powerupKey] = Constants.HUD.POWERUP_INIT_TIMER;
            }
            
        }

        private void UpdatePowerupPosition(string powerupKey)
        {
            if (powerupActive[powerupKey])
            {
                // Timer logic for staying at (0, 115) for a few seconds
                powerupTimers[powerupKey] += (float)Game1.Instance.time.ElapsedGameTime.TotalSeconds;

                if (powerupTimers[powerupKey] < Constants.HUD.STAY_TIME)
                {
                    // Slide up from the bottom
                    if (powerupPositions[powerupKey].Y > Constants.HUD.POWERUP_MAX_Y)
                    {
                        powerupPositions[powerupKey] -= new Vector2(0, Constants.HUD.SLIDE_SPEED); // Move up
                    }
                }
                else
                {
                    // After stay time, slide down to (0, 147)
                    if (powerupPositions[powerupKey].Y < Constants.HUD.SPRITES_Y)
                    {
                        powerupPositions[powerupKey] += new Vector2(0, Constants.HUD.SLIDE_SPEED); // Move down
                    }
                }
            }
            else
            {
                // Slide back down if inactive
                if (powerupPositions[powerupKey].Y < Constants.HUD.SPRITES_Y)
                {
                    powerupPositions[powerupKey] += new Vector2(0, Constants.HUD.SLIDE_SPEED); // Move down
                }
            }
        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            int score = Game1.Instance.manager.Score;

            string scoreText = score.ToString().PadLeft(Constants.HUD.SCORE_PAD, '0'); // Add padding to ensure it is always 8 digits

            // Draw the current score
            for (int i = 0; i < scoreText.Length; i++)
            {
                char digitChar = scoreText[i];
                int digit = int.Parse(digitChar.ToString());

                float xPosition = 176 + i * Constants.HUD.SPRITE_GAP;
                hudElements[$"ui_{digit}"].Draw(new Vector2(xPosition, Constants.HUD.SPRITES_Y), spriteBatch);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (targetPlayer != null)
            {
                foreach (var powerupKey in powerupPositions.Keys)
                {
                    if (powerupActive[powerupKey])
                    {
                        hudElements[powerupKey].Draw(powerupPositions[powerupKey], spriteBatch);
                    }
                }

                // Draw lives
                hudElements["ui_lives"].Draw(Constants.HUD.LIVES_ICON_POS, spriteBatch);

                int displayLives = targetPlayer.lives; // Adjust to show 02 for 3 lives, 01 for 2 lives, etc.
                string displayLivesText = displayLives.ToString().PadLeft(Constants.HUD.LIVES_PAD, '0'); // Format as two digits

                int livesTens = int.Parse(displayLivesText[0].ToString());
                int livesOnes = int.Parse(displayLivesText[1].ToString());

                hudElements[$"ui_{livesTens}"].Draw(Constants.HUD.LIVES_TENS_POS, spriteBatch);
                hudElements[$"ui_{livesOnes}"].Draw(Constants.HUD.LIVES_ONES_POS, spriteBatch);

                // Draw health bar based on player.health
                int healthX = Constants.HUD.HEALTH_INIT_X;
                for (int i = 0; i < Constants.Kirby.MAX_HEALTH; i++)
                {
                    string healthSprite = i < targetPlayer.health ? "ui_healthbar_1" : "ui_healthbar_0";
                    hudElements[healthSprite].Draw(new Vector2(healthX, Constants.HUD.HEALTH_Y), spriteBatch);
                    healthX += Constants.HUD.HEALTH_NEXT_X;
                }

                DrawScore(spriteBatch);
            }
        }
    }
}
