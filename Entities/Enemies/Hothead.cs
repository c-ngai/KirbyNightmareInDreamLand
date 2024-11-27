using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Hothead : Enemy
    {

        // All fireballs and flamethrower
        private readonly List<IProjectile> fireballs;
        private EnemyFlamethrower flamethrower;

        //Checks if flamethrower attack is active
        private bool isFlamethrowerActive;
        private int flamethrowerFrameCounter;

        public Hothead(Vector2 startPosition) : base(startPosition, EnemyType.Hothead)
        {
            //Initializes attacks and pose for enemy
            fireballs = new List<IProjectile>();
            //stateMachine.IsLeft() ? Constants.Hothead.FLAMETHROWER_LEFT : Constants.Hothead.FLAMETHROWER_RIGHT
            isFlamethrowerActive = false;
            flamethrowerFrameCounter = 0;

            affectedByGravity = true;
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Walking);
            currentState = new HotheadWalkingState(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (active)
            {
                UpdateFireballs();

                // Update flamethrower if active
                if (isFlamethrowerActive)
                {
                    flamethrower.Update(gameTime);
                    flamethrowerFrameCounter++;

                    if (flamethrowerFrameCounter >= Constants.Hothead.ATTACK_FRAMES)
                    {
                        isFlamethrowerActive = false; // Deactivate flamethrower
                        flamethrower.ClearSegments(); // Clear fire
                    }
                }
                else
                {
                    flamethrower?.ClearSegments(); // Clear segments when not active
                }
            }
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust flamethrower position based on Hothead's facing direction
            //no magic numbers
            return stateMachine.IsLeft() ? new Vector2(position.X - Constants.Hothead.FLAMETHROWER_X_OFFSET, position.Y - Constants.Hothead.FLAMETHROWER_Y_OFFSET) : new Vector2(position.X + Constants.Hothead.FLAMETHROWER_X_OFFSET, position.Y - Constants.Hothead.FLAMETHROWER_Y_OFFSET);
        }

        public void Flamethrower(/*GameTime gameTime*/)
        {
            //Shoots flamethrower if called while inactive
            if (!isFlamethrowerActive)
            {
                flamethrower = new EnemyFlamethrower(ProjectilePosition(), stateMachine.IsLeft());
                isFlamethrowerActive = true;

                //need to loop
                //SoundManager.Play("hotheadflamethrowerattack");

                // Set the start position for the flamethrower
                flamethrowerFrameCounter = 0; 
            }
        }

        public override void Attack()
        {
            SoundManager.Play("hotheadfireballattack");
            //Shoots fireball projectile attack
            Vector2 projectileDirection = stateMachine.IsLeft() ? Constants.Hothead.FIREBALL_LEFT : Constants.Hothead.FIREBALL_RIGHT;
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
            if (active)
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

        public override KirbyType PowerType()
        {
            return KirbyType.Fire;
        }

    }
}