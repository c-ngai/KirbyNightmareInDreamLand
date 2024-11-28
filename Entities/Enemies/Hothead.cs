using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.HotheadState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class Hothead : Enemy
    {

        // All fireballs and flamethrower
        private EnemyFlamethrower flamethrower;

        private SoundInstance flamethrowerSound;

        //Checks if flamethrower attack is active
        private int attackTimer;

        public Hothead(Vector2 startPosition) : base(startPosition, EnemyType.Hothead)
        {
            attackTimer = 0;
            flamethrowerSound = SoundManager.CreateInstance("hotheadflamethrowerattack");

            affectedByGravity = true;
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Walking);
            currentState = new HotheadWalkingState(this);
        }


        public override void TakeDamage(Rectangle intersection, Vector2 positionOfDamageSource)
        {
            base.TakeDamage(intersection, positionOfDamageSource);
            flamethrower?.EndAttack();
        }

        public override void GetInhaled(Rectangle intersection, IPlayer player)
        {
            base.GetInhaled(intersection, player);
            flamethrower?.EndAttack();
        }

        public override void Move()
        {
            base.Move();
            velocity.X = stateMachine.IsLeft() ? -Constants.Hothead.MOVE_SPEED : Constants.Hothead.MOVE_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (active)
            { 

                // Update flamethrower if active
                if (currentState.GetType().Equals(typeof(HotheadAttackingState)))
                {
                    attackTimer--;

                    if (attackTimer <= 0)
                    {
                        flamethrower?.EndAttack();
                        ChangeState(new HotheadWalkingState(this));
                    }
                }

            }
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust flamethrower position based on Hothead's facing direction
            //no magic numbers
            return stateMachine.IsLeft() ? new Vector2(position.X - Constants.Hothead.FLAMETHROWER_X_OFFSET, position.Y - Constants.Hothead.FLAMETHROWER_Y_OFFSET) : new Vector2(position.X + Constants.Hothead.FLAMETHROWER_X_OFFSET, position.Y - Constants.Hothead.FLAMETHROWER_Y_OFFSET);
        }

        public void FlamethrowerAttack()
        {
            flamethrower = new EnemyFlamethrower(ProjectilePosition(), stateMachine.IsLeft());

            //need to loop
            //SoundManager.Play("hotheadflamethrowerattack");

            // Set the start position for the flamethrower
            attackTimer = Constants.Hothead.FLAMETHROWER_ATTACK_FRAMES;
        }

        public void FireballAttack()
        {
            SoundManager.Play("hotheadfireballattack");
            //Shoots fireball projectile attack
            Vector2 projectileDirection = stateMachine.IsLeft() ? Constants.Hothead.FIREBALL_LEFT : Constants.Hothead.FIREBALL_RIGHT;
            new EnemyFireball(ProjectilePosition(), projectileDirection);

            attackTimer = Constants.Hothead.FIREBALL_ATTACK_FRAMES;
        }

        public override void Attack()
        {
            IPlayer nearestPlayer = ObjectManager.Instance.NearestPlayer(position);
            float distance = Vector2.Distance(position, nearestPlayer.GetKirbyPosition());
            // If a player is close, use flamethrower attack
            if (distance < Constants.Hothead.FLAMETHROWER_RANGE)
            {
                FlamethrowerAttack();
            }
            // If no player is close, use the fireball
            else
            {
                FireballAttack();
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy, flamethrower, and projectile depending if alive or active
            if (active)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }

        public override KirbyType PowerType()
        {
            return KirbyType.Fire;
        }

        public override void Dispose()
        {
            base.Dispose();
            flamethrowerSound.Dispose();
        }

    }
}