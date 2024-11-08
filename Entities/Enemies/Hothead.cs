using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState;
using KirbyNightmareInDreamLand.Audio;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Hothead : Enemy
    {

        // All fireballs and flamethrower
        private readonly List<IProjectile> fireballs;
        private readonly EnemyFlamethrower flamethrower;

        //Checks if flamethrower attack is active
         private bool isFlamethrowerActive;
         private int flamethrowerFrameCounter;

        public Hothead(Vector2 startPosition) : base(startPosition, EnemyType.Hothead)
        {
            //Initializes attacks and pose for enemy
            fireballs = new List<IProjectile>();
            flamethrower = new EnemyFlamethrower();
            isFlamethrowerActive = false;
            flamethrowerFrameCounter = 0;
            currentState = new HotheadWalkingState(this);
            //TO-DO: spawn facing the direction kirby is in
            yVel = 0;
            xVel = Constants.Hothead.MOVE_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                IncrementFrameCounter();
                currentState.Update();
                UpdateTexture();
                // Update the sprite and fireballs
                Fall();

                enemySprite.Update();
                UpdateFireballs();
                GetHitBox();

                // Update flamethrower if active
                if (isFlamethrowerActive)
                {
                    flamethrower.Update(gameTime, ProjectilePosition(), stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0));
                    flamethrowerFrameCounter++;

                    if (flamethrowerFrameCounter >= Constants.Hothead.ATTACK_FRAMES)
                    {
                        isFlamethrowerActive = false; // Deactivate flamethrower
                        flamethrower.ClearSegments(); // Clear fire
                    }
                }
                else
                {
                    flamethrower.ClearSegments(); // Clear segments when not active
                }
            }
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust flamethrower position based on Hothead's facing direction
            //no magic numbers
            return stateMachine.IsLeft() ? new Vector2(position.X - 18, position.Y - 7 ) : new Vector2(position.X + 18, position.Y - 7);
        }

        public void Flamethrower(/*GameTime gameTime*/)
        {
            //Shoots flamethrower if called while inactive
            if (!isFlamethrowerActive)
            {
                isFlamethrowerActive = true;

                //need to loop
                //SoundManager.Play("hotheadflamethrowerattack");

                // Set the start position for the flamethrower
                Vector2 flameDirection = stateMachine.IsLeft() ? new Vector2(-1, 0) : new Vector2(1, 0);
                flamethrowerFrameCounter = 0; 
            }
        }

        public override void Attack()
        {
            SoundManager.Play("hotheadfireballattack");
            //Shoots fireball projectile attack
            Vector2 projectileDirection = stateMachine.IsLeft() ? new Vector2(-1, -0.5f) : new Vector2(1, -0.5f);
            IProjectile newFireball = new EnemyFireball(ProjectilePosition(), projectileDirection);
            fireballs.Add(newFireball);
        }

        private void UpdateFireballs()
        {
            //Updates all fireballs on list
            foreach (var fireball in fireballs)
            {
                fireball.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy, flamethrower, and projectile depending if alive or active
            if (!isDead)
            {
                if (isFlamethrowerActive)
                {
                    flamethrower.Draw(spriteBatch);
                }

                enemySprite.Draw(position, spriteBatch);

                foreach (var fireball in fireballs)
                {
                    fireball.Draw(spriteBatch);
                }
            }
        }

    }
}