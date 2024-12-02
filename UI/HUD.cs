using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Linq;

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
                { "hud_card_beam", SpriteFactory.Instance.CreateSprite("hud_card_beam") },
                { "hud_card_spark", SpriteFactory.Instance.CreateSprite("hud_card_spark") },
                { "hud_card_fire", SpriteFactory.Instance.CreateSprite("hud_card_fire") },
                { "hud_card_professor", SpriteFactory.Instance.CreateSprite("hud_card_professor") },
                { "hud_card_wait", SpriteFactory.Instance.CreateSprite("hud_card_wait") },
                { "hud_lives", SpriteFactory.Instance.CreateSprite("hud_lives" + playerIndex) },
                { "hud_healthbar_1", SpriteFactory.Instance.CreateSprite("hud_healthbar_1") },
                { "hud_healthbar_0", SpriteFactory.Instance.CreateSprite("hud_healthbar_0") },
                { "hud_0", SpriteFactory.Instance.CreateSprite("hud_0") },
                { "hud_1", SpriteFactory.Instance.CreateSprite("hud_1") },
                { "hud_2", SpriteFactory.Instance.CreateSprite("hud_2") },
                { "hud_3", SpriteFactory.Instance.CreateSprite("hud_3") },
                { "hud_4", SpriteFactory.Instance.CreateSprite("hud_4") },
                { "hud_5", SpriteFactory.Instance.CreateSprite("hud_5") },
                { "hud_6", SpriteFactory.Instance.CreateSprite("hud_6") },
                { "hud_7", SpriteFactory.Instance.CreateSprite("hud_7") },
                { "hud_8", SpriteFactory.Instance.CreateSprite("hud_8") },
                { "hud_9", SpriteFactory.Instance.CreateSprite("hud_9") }
            };

            // Initialize powerup positions and active states
            powerupPositions = new Dictionary<string, Vector2>
            {
                { "hud_card_beam", Constants.HUD.POWERUP_INIT_POS },
                { "hud_card_spark", Constants.HUD.POWERUP_INIT_POS },
                { "hud_card_fire", Constants.HUD.POWERUP_INIT_POS },
                { "hud_card_professor", Constants.HUD.POWERUP_INIT_POS },
                { "hud_card_wait", Constants.HUD.POWERUP_INIT_POS }
            };

            powerupActive = new Dictionary<string, bool>
            {
                { "hud_card_beam", false },
                { "hud_card_spark", false },
                { "hud_card_fire", false },
                { "hud_card_professor", false },
                { "hud_card_wait", false }
            };

            powerupTimers = new Dictionary<string, float>
            {
                { "hud_card_beam", Constants.HUD.POWERUP_INIT_TIMER },
                { "hud_card_spark", Constants.HUD.POWERUP_INIT_TIMER },
                { "hud_card_fire", Constants.HUD.POWERUP_INIT_TIMER },
                { "hud_card_professor", Constants.HUD.POWERUP_INIT_TIMER },
                { "hud_card_wait", Constants.HUD.POWERUP_INIT_TIMER }
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
                // Check Kirby's powerup state
                string currentPower = targetPlayer.GetPowerUp().ToString().ToLower();

                // If this player has died and at least one other player is still alive
                if (!targetPlayer.IsActive && !ObjectManager.Instance.AllPlayersInactive())
                {
                    ActivatePowerup("hud_card_wait");
                }
                else if (targetPlayer.IsActive && (string.IsNullOrEmpty(currentPower) || currentPower == "normal"))
                {
                    // No powerup, deactivate all cards
                    DeactivateAllPowerups();
                }
                else if (Game1.Instance.Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GamePowerChangeState"))
                {
                    // Kirby has an active powerup
                    ActivatePowerup("hud_card_" + currentPower);
                }

                // Update positions for active powerup cards
                foreach (string powerupKey in powerupActive.Keys)
                {
                    UpdatePowerupPosition(powerupKey);
                }
            }
        }

        public void ActivatePowerup(string powerupKey)
        {
            string oldPowerUp = "nor";
            foreach (string key in powerupActive.Keys)
            {
                if (powerupActive[key] == true)
                {
                    oldPowerUp = key;
                }
            }

            if(!powerupKey.Equals(oldPowerUp) && powerupActive.Keys.Contains(powerupKey)) // be careful setting dictionary entries of keys you aren't sure exist!! it can make new entries!! this caused issues when a player got a power while another player was in mouthful state, because it made a new entry for mouthful but not in every dictionary
            {
                // Deactivate all powerups first
                foreach (string key in powerupActive.Keys)
                {
                    powerupActive[key] = false;
                }

                // Activate the selected powerup and reset timer for new powerup
                powerupActive[powerupKey] = true;
                powerupTimers[powerupKey] = Constants.HUD.POWERUP_INIT_TIMER;
            }
            
        }

        private void DeactivateAllPowerups()
        {
            foreach (string key in powerupActive.Keys)
            {
                powerupActive[key] = false;
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

                float xPosition = Constants.HUD.SCORE1 + i * Constants.HUD.SPRITE_GAP;
                hudElements[$"hud_{digit}"].Draw(new Vector2(xPosition, Constants.HUD.SPRITES_Y), spriteBatch);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the HUD if the level is not paused
            if (!Game1.Instance.Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GamePausedState"))
            {
                if (targetPlayer != null)
                {
                    foreach (string powerupKey in powerupPositions.Keys)
                    {
                        if (powerupActive[powerupKey])
                        {
                            hudElements[powerupKey].Draw(powerupPositions[powerupKey], spriteBatch);
                        }
                    }

                    // Draw lives
                    hudElements["hud_lives"].Draw(Constants.HUD.LIVES_ICON_POS, spriteBatch);

                    int displayLives = targetPlayer.lives;
                    string displayLivesText = displayLives.ToString().PadLeft(Constants.HUD.LIVES_PAD, '0'); // Format as two digits

                    int livesTens = int.Parse(displayLivesText[0].ToString());
                    int livesOnes = int.Parse(displayLivesText[1].ToString());

                    hudElements[$"hud_{livesTens}"].Draw(Constants.HUD.LIVES_TENS_POS, spriteBatch);
                    hudElements[$"hud_{livesOnes}"].Draw(Constants.HUD.LIVES_ONES_POS, spriteBatch);

                    // Draw health bar based on player.health
                    int healthX = Constants.HUD.HEALTH_INIT_X;
                    for (int i = 0; i < Constants.Kirby.MAX_HEALTH; i++)
                    {
                        string healthSprite = i < targetPlayer.health ? "hud_healthbar_1" : "hud_healthbar_0";
                        hudElements[healthSprite].Draw(new Vector2(healthX, Constants.HUD.HEALTH_Y), spriteBatch);
                        healthX += Constants.HUD.HEALTH_NEXT_X;
                    }

                    DrawScore(spriteBatch);
                }
            }
        }

    }
}
