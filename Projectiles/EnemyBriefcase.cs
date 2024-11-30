using System;
using System.Net;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyBriefcase : IProjectile, ICollidable, IExplodable
    {
        Sprite projectileSprite;
        Sprite windupSprite;
        Sprite openSprite;

        public IPlayer player { get; private set; }

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 enemyPosition;

        private bool IsActive;
        public bool CollisionActive { get; private set; }
        private bool IsLeft;

        private int timer = 0;

        private bool windup; // the first few frames when kirby is still throwing the suitcase and hasn't released it yet. the briefcase does not collide with tiles during this window
        private bool exploded;
        private Vector2 explodedPosition;

        public Vector2 Position
        {
            get => position;            // Return position of star
            set => position = value;    // Set the position of the star to the given value
        }

        public Vector2 Velocity
        {
            get => velocity;            // Return the current velocity of the star
            set => velocity = value;    // Set the velocity of the star to the given value
        }


        public EnemyBriefcase(Vector2 pos, bool isLeft)
        {
            player = ObjectManager.Instance.NearestPlayer(position);

            enemyPosition = pos;
            IsActive = true;
            CollisionActive = false;
            velocity = isLeft ? Constants.Briefcase.SUITCASE_VEL_LEFT :Constants.Briefcase.SUITCASE_VEL_RIGHT;
            IsLeft= isLeft;
            windup = true;
            exploded = false;

            projectileSprite = IsLeft
                ? SpriteFactory.Instance.CreateSprite("projectile_briefcase_left")
                : SpriteFactory.Instance.CreateSprite("projectile_briefcase_right");

            windupSprite = IsLeft
                ? SpriteFactory.Instance.CreateSprite("projectile_briefcasewindup_left")
                : SpriteFactory.Instance.CreateSprite("projectile_briefcasewindup_right");

            openSprite = IsLeft
                ? SpriteFactory.Instance.CreateSprite("projectile_briefcaseopen_left")
                : SpriteFactory.Instance.CreateSprite("projectile_briefcaseopen_right");

            ObjectManager.Instance.AddProjectile(this);

            SoundManager.Play("briefcasethrow");
        }

        public void Explode()
        {
            
        }

        public void EndAttack()
        {
            if (!windup && !exploded)
            {
                exploded = true;
                explodedPosition = position;
                velocity.X = 0;
                velocity.Y = -1.5f;
                timer = 0;
                SoundManager.Play("briefcaseopen");
            }
        }

        public bool IsDone()
        {
            return !IsActive;
        }

        public CollisionType GetCollisionType()
        {
            return CollisionType.EnemyAttack;
        }

        public void Update()
        {
            
            // End windup after # frames
            if (windup && timer >= Constants.Briefcase.ENEMY_BRIEFCASE_WINDUP_FRAMES)
            {
                windup = false;
                CollisionActive = true;
                position = IsLeft ?
                    enemyPosition + Constants.ProfessorKirby.BRIEFCASE_OFFSET_LEFT :
                    enemyPosition + Constants.ProfessorKirby.BRIEFCASE_OFFSET_RIGHT;
            }
            if (exploded && timer >= Constants.Briefcase.BRIEFCASE_EXPLODE_COLLISION_FRAMES)
            {
                CollisionActive = false;
            }

            // if in windup stage
            if (windup)
            {
                //windupSprite.Update();
                position = enemyPosition;
            }
            // if in midair stage
            else if (!exploded || windup)
            {
                projectileSprite.Update();
                velocity.Y += Constants.Physics.GRAVITY;
                position += velocity;
            }
            // if in exploded stage
            else
            {
                openSprite.Update();
                velocity.Y += Constants.Physics.GRAVITY;
                position += velocity;
                if (position.Y > Game1.Instance.Level.CurrentRoom.DeathBarrier)
                {
                    IsActive = false;
                }
            }

            
            timer++;
        }
        
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            if(exploded)
            {
                return pos + Constants.Briefcase.SUITCASE_EXPLODE_OFFSET; 
            } else {
                return pos + Constants.Briefcase.SUITCASE_OFFSET; 
            }
            
        }
        public Rectangle GetHitBox()
        {
            
            if(exploded)
            {
                // if exploded, calculate hitbox based on the position when the briefcase exploded, not its current position (because the hitbox is stationary while the briefcase falls off the screen)
                Vector2 rectPoint = CalculateRectanglePoint(explodedPosition);
                return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, 
                Constants.Briefcase.SUITCASE_EXPLODE_WIDTH, Constants.Briefcase.SUITCASE_EXPLODE_HEIGHT);
            } else {
                Vector2 rectPoint = CalculateRectanglePoint(position);
                return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, 
                Constants.Briefcase.SUITCASE_WIDTH, Constants.Briefcase.SUITCASE_HEIGHT);
            }  
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // if in windup stage
            if (windup)
            {
                // Setting the position inside draw is bad. dont have time to do this better. very jank. tldr it's because otherwise it draws one pixel lower than it should, because if it only matches kirby's position in Update(), that's happened AFTER gravity has been applied to kirby but BEFORE normal forces push him back up. so i set it here too. i'm evil
                position = enemyPosition;
            }
            // if in midair stage
            else if (!exploded)
            {
                projectileSprite.Draw(position, spriteBatch);
            }
            // if in exploded stage
            else
            {
                openSprite.Draw(position, spriteBatch);
            }
        }

    }
}