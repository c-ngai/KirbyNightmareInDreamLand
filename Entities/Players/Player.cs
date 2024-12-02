using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class Player : IPlayer, ICollidable
    {
        private Game1 _game;
        public PlayerStateMachine state { get; private set; }
        public PlayerMovement movement { get; private set; }
        private int playerIndex;
        private Sprite playerSprite;
        private Sprite[][] playerarrows;
        public PlayerAttack attack { get; private set; }

        public int health { get; private set; }
        public int lives { get; private set; }
        private bool invincible = false;
        private double invincibilityTimer = 0;
        private bool hurtStun = false;
        private int damageCounter;
        private bool isAttacking = false;
        private bool wantToStopAttacking;
        private double timer = 0;
        private int powerChangeTimer = 0;
        private bool isSwallowing = false;
        private int swallowTimer = 0;
        private bool shouldEnterBurnBounce = false;

        public bool powerChangeAnimation { get; set; } = false; // is this kirby currently in a power change animation?

        //others
        private string oldState;
        private KirbyPose oldPose;
        public int poseCounter { get; private set; }
        private int lastFramePoseChanged = 0;
        public bool attackIsActive { get; private set; } = false;
        public bool CollisionActive { get; private set; } = true;
        public bool DEAD { get; private set; } = false;
        private int deathCounter = 0;
        public bool facingLeftWall { get; private set; } = false;
        public bool facingRightWall { get; private set; } = false;
        // IsActive is false after a player's death animation finishes, and is true again any time they respawn
        public bool IsActive { get; private set; } = true;

        private KirbyType powerUp = KirbyType.Normal;

        //collision stuffs

        //constructor
        public Player(Vector2 pos, int playerIndex)
        {
            _game = Game1.Instance;
            this.playerIndex = playerIndex;
            state = new PlayerStateMachine(playerIndex);
            movement = new NormalPlayerMovement(pos, Vector2.Zero);
            oldState = null;
            oldPose = GetKirbyPose();
            poseCounter = 0;
            health = Constants.Kirby.MAX_HEALTH;
            lives = Constants.Kirby.MAX_LIVES;
            ObjectManager.Instance.RegisterDynamicObject(this);
            movement.ChangeKirbyLanded(false);
            UpdateTexture();
            CreatePlayerArrowSprites();
        }

        private void CreatePlayerArrowSprites()
        {
            playerarrows = new Sprite[Constants.Arrows.MAX_ARROWS][];
            // For each of the 9 sprites in the 3x3 arrow texture grid
            for (int x = 0; x < Constants.Arrows.MAX_ARROWS; x++)
            {
                playerarrows[x] = new Sprite[Constants.Arrows.MAX_ARROWS];
                for (int y = 0; y < Constants.Arrows.MAX_ARROWS; y++)
                {
                    // Create the sprite from the respective name
                    string spriteName = "playerarrow" + playerIndex + "_" + x + "," + y;
                    playerarrows[x][y] = SpriteFactory.Instance.CreateSprite(spriteName);
                }
            }
        }

        public void ResetAfterDoor()
        {
            if (state.IsFloating())
            {
                ChangePose(KirbyPose.FreeFall);
            }
            SetDirectionRight();
            attack?.EndAttack();
            attack = null;
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.Player;
        }

        //changes kiry's texture if he is in a different state than before
        //only called by Draw
        private void UpdateTexture()
        {
            if (!state.GetStateString().Equals(oldState))
            {
                playerSprite = SpriteFactory.Instance.CreateSprite(state.GetSpriteParameters());
                playerSprite.Update();
            }
        }

        #region KirbyState
        public void ChangePose(KirbyPose pose)
        {
            // debug
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (_game.UpdateCounter > lastFramePoseChanged)
                {
                    lastFramePoseChanged = _game.UpdateCounter;
                    Debug.WriteLine("\n##### UPDATE " + _game.UpdateCounter);
                }
                Debug.WriteLine("  ChangePose: " + pose.ToString());
            }

            state.ChangePose(pose);
            if (oldPose != state.GetPose())
            {
                poseCounter = 0;
            }
        }
        public void ChangeToNormalMovement()
        {
            movement = new NormalPlayerMovement(movement.GetPosition(), movement.GetVelocity());
        }
        public KirbyPose GetKirbyPose()
        {
            return state.GetPose();
        }
        public string GetKirbyType()
        {
            return state.GetKirbyType().ToString();
        }
        public string GetKirbyTypePause()
        {
            if (DEAD)
            {
                return "Dead";
            }
            else if (GetKirbyType().Equals("Mouthful"))
            {
                return "Normal";
            }
            else
            {
                return state.GetKirbyType().ToString();
            }

        }
        public bool IsLeft()
        {
            return state.IsLeft();
        }
        public bool IsFloating()
        {
            return state.IsFloating();
        }
        public bool IsFalling()
        {
            return state.IsFalling();
        }
        public bool IsSliding()
        {
            return state.IsSliding();
        }
        private String AttackType()
        {
            if (IsFloating() && !IsFalling())
            {
                return "Puff";
            }
            else if (state.IsCrouching())
            {
                return "Slide";
            }
            else if (state.EnemyInMouth())
            {
                return "Star";
            }
            else
            {
                return GetKirbyType();
            }
        }

        public void ChangeAttackBool(bool activate)
        {
            attackIsActive = activate;
        }
        public Vector2 GetKirbyPosition()
        {
            return movement.GetPosition();
        }
        public Vector2 GetKirbyVelocity()
        {
            return movement.GetVelocity();
        }
        public void ResetAtWall()
        {
            facingLeftWall = false;
            facingRightWall = false;
        }

        private bool CanControl()
        {
            return IsActive && !DEAD && !hurtStun && !isAttacking && !isSwallowing && !state.IsHurt() && !shouldEnterBurnBounce;
        }
        #endregion

        #region Power-Up
        public void ChangeToNormal()
        {
            state.ChangeType(KirbyType.Normal);
        }
        public void ChangeToBeam()
        {
            //attackSprite = new KirbyBeam(movement.GetPosition(), IsLeft());
            state.ChangeType(KirbyType.Beam);
        }
        public void ChangeToFire()
        {
            powerUp = KirbyType.Fire;
            state.ChangeType(KirbyType.Fire);
        }
        public void ChangeToSpark()
        {
            state.ChangeType(KirbyType.Spark);
        }
        public void ChangeToProfessor()
        {
            state.ChangeType(KirbyType.Professor);
        }
        public void ChangeToMouthful()
        {
            state.ChangeType(KirbyType.Mouthful);
        }
        public KirbyType GetPowerUp()
        {
            return state.GetKirbyType();
        }
        #endregion
        #region direction
        public void SetDirectionLeft()
        {
            if (!state.IsAttacking())
            {
                state.SetDirectionLeft();
            }
        }
        public void SetDirectionRight()
        {
            if (!state.IsAttacking())
            {
                state.SetDirectionRight();
            }
        }
        #endregion

        #region health 
        public void RestartKirby()
        {
            CollisionActive = true;
            invincible = false;
            invincibilityTimer = 0;
            isSwallowing = false;
            DEAD = false;
            IsActive = true;
            health = Constants.Kirby.MAX_HEALTH;
        }


        public void Die()
        {
            //lives--;
            health = 0;
            DEAD = true;
            deathCounter = 0;
            CollisionActive = false;

            hurtStun = true; // so that kirby does NOT flash yellow yet


            ChangeToNormal();
            ChangeToNormalMovement();
            movement.CancelVelocity();

            //state.ChangePose(KirbyPose.Standing);

            Debug.WriteLine("DEAD");
            // if all players are dead, stop the music
            if (ObjectManager.Instance.AllPlayersDead())
            {
                Debug.WriteLine("STOP SONG");
                SoundManager.PlaySong("");
            }

            SoundManager.Play("kirbydeath");
            ChangePose(KirbyPose.DeathStun);
        }
        public void DeathSpin()
        {
            invincible = true;
            hurtStun = false; // so that kirby flashes yellow like when invincible
            SoundManager.Play("deathjingle");
            ChangePose(KirbyPose.DeathSpin);
            movement.DeathSpin();

        }


        public void TakeDamage(ICollidable damageDealer, Rectangle intersection, Vector2 positionOfDamageSource)
        {
            if (!invincible)
            {
                invincible = true;
                hurtStun = true;
                isSwallowing = false; //taking damage cancels swallowing
                damageCounter = 0;

                health--; //decrease health

                attack?.EndAttack();
                attack = null;

                Debug.WriteLine("END ATTACK");
                if (health == 0) //health decresed to 0 and lost life
                {
                    Die();
                }
                else
                { //health decreased,  but didnt loose life
                    TakeDamageAnimation(damageDealer);
                    movement.ReceiveDamage(intersection);
                }
            }

        }
        // calls method to decrease health & changes kirby pose
        private void TakeDamageAnimation(ICollidable damageDealer)
        {
            if (invincible)
            {
                if (state.HasPowerUp())
                {
                    DropAbility();
                }

                if (IsFloating())
                {
                    movement = new NormalPlayerMovement(GetKirbyPosition(), movement.GetVelocity());
                }
                Debug.Write(damageDealer);

                // has different damage animations depending on what hit Kirby
                if (damageDealer is SparkyPlasma)
                {
                    ChangePose(KirbyPose.HurtSpark);
                }
                else if (damageDealer is EnemyFlameSegment || damageDealer is EnemyFireball)
                {
                    ChangePose(KirbyPose.HurtFire);
                }
                else
                {
                    ChangePose(KirbyPose.Hurt);
                }
                SoundManager.Play("kirbyhurt1");
            }
        }

        public void DropAbility()
        {
            if (state.HasPowerUp())
            {
                if (!EnemyInMouth())
                {
                    new KirbyBouncingStar(GetKirbyPosition(), IsLeft(), GetPowerUp());
                    new DropAbility(GetKirbyPosition());
                    powerUp = KirbyType.Normal;
                    ChangeToNormal();
                }
            }
        }

        public void ManualDropAbility()
        {
            if (state.HasPowerUp())
            {
                SoundManager.Play("dropability");
                DropAbility();
            }
        }

        private void EndInvinciblility(GameTime gameTime)
        {
            if (invincible)
            {
                invincibilityTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (invincibilityTimer > Constants.Kirby.INVINCIBLE_TIME)
                {
                    invincible = false;
                    invincibilityTimer = 0;
                }
            }
        }

        public void FillHealth()
        {
            health = Constants.Kirby.MAX_HEALTH;
        }
        public void FillLives()
        {
            health = Constants.Kirby.MAX_HEALTH;
            lives = Constants.Kirby.MAX_LIVES;
            DEAD = false;
        }
        #endregion

        #region Movement
        public void GoToRoomSpawn()
        {
            movement.GoToRoomSpawn(this, playerIndex);
            if (!IsActive && (lives > 0 || _game.Level.CurrentRoom.Name == "hub"))
            {
                if (DEAD && lives > 0)
                {
                    lives--;
                }
                RestartKirby();
                Debug.WriteLine("Kirby #" + playerIndex + ", RestartKirby() called");
            }
        }

        public void MoveLeft()
        {
            if (CanControl())
            {
                SetDirectionLeft();
                facingRightWall = false;
                movement.Walk(state.IsLeft());
                //check if kirby should change pose
                if (state.CanMove() && !facingLeftWall)
                {
                    ChangePose(KirbyPose.Walking);
                }
            }
        }

        public void MoveRight()
        {
            if (CanControl())
            {
                SetDirectionRight();
                movement.Walk(state.IsLeft());
                facingLeftWall = false;
                //walk connot override walking, jumping, floating, crouching, and attack
                if (state.CanMove() && !facingRightWall)
                {
                    ChangePose(KirbyPose.Walking);
                }
            }
        }

        public void HandleFalling()
        {
            // should kirby exhibit falling behavior
            if (movement.GetVelocity().Y > 0 && !movement.onSlope && !DEAD && !state.IsFloating() && !state.IsAttacking())
            {
                ResetAtWall();
                // if kirby was not falling enter freefall
                if (!state.IsFalling() && !state.IsHurt())
                {
                    ChangePose(KirbyPose.FreeFall);
                }
                // should kirby enter free fall far
                if (GetKirbyPose() == KirbyPose.FreeFall && poseCounter > Constants.Kirby.MIN_FREEFALL_FAR_FRAMES && state.GetKirbyType() != KirbyType.Mouthful)
                {
                    ChangePose(KirbyPose.FreeFallFar);
                }

                // changes bounce and falling poses to be named the same pose (they are the same)
                if (GetKirbyPose() == KirbyPose.JumpFalling && poseCounter > Constants.Kirby.JUMP_FREEFALL_START)
                {
                    ChangePose(KirbyPose.FreeFall);
                }
                if (GetKirbyPose() == KirbyPose.Bounce && poseCounter > Constants.Kirby.BOUNCE_FREEFALL_START)
                {
                    ChangePose(KirbyPose.FreeFall);
                }

                movement.ChangeKirbyLanded(false);
            }


            // changes kirby to freefall once stop floating should end
            if (GetKirbyPose() == KirbyPose.FloatingEnd && poseCounter > Constants.Kirby.STOP_FLOATING_TRANSITION_FRAME)
            {
                ChangePose(KirbyPose.FreeFall);
                movement.ChangeKirbyLanded(false);
            }
        }
        #region running
        public void RunLeft()
        {
            if (CanControl())
            {
                SetDirectionLeft();
                facingRightWall = false;
                movement.Run(state.IsLeft());
                if (state.CanMove() && !facingLeftWall)
                {
                    DashEffects();
                    ChangePose(KirbyPose.Running);
                }
            }
        }
        public void RunRight()
        {
            if (CanControl())
            {
                SetDirectionRight();
                facingLeftWall = false;
                movement.Run(state.IsLeft());
                if (state.CanMove() && !facingRightWall)
                {
                    DashEffects();
                    ChangePose(KirbyPose.Running);
                }
            }
        }

        private void DashEffects()
        {
            // if on the first frame of the dash, play the dash sound
            if (poseCounter == 1) // && oldPose != KirbyPose.Running)
            {
                SoundManager.Play("dash");
            }
            // if on one of the first n multiples of the dash cloud animation length, create a new dash cloud particle (create three back-to-back)
            if (poseCounter % Constants.Particle.DASH_CLOUD_FRAMES == 1 && poseCounter < Constants.Particle.DASH_CLOUD_FRAMES * Constants.Particle.DASH_CLOUD_LOOPS)
            {
                IParticle cloud = new DashCloud(this);
            }
        }
        #endregion

        #region jumping
        public void Jump()
        {
            if (CanControl())
            {
                ResetAtWall();
                if (state.CanJump())
                { //not floating, not jumping, not crouching
                    movement = new JumpMovement(movement.GetPosition(), movement.GetVelocity());
                    ChangePose(KirbyPose.JumpRising);
                    SoundManager.Play("jump");

                }
                else if (state.IsJumping() && !state.IsFloating()) //if jumping and x is pressed again
                {
                    movement.Jump(state.IsLeft());
                }
                else if (state.IsFloating())
                {
                    Float();
                }
            }
        }
        #endregion

        #region float
        private void StartFloating()
        {
            // enter floating movement
            if (!movement.GetType().Equals(typeof(FloatingMovement)))
            {
                movement = new FloatingMovement(movement.GetPosition(), movement.GetVelocity());
            }

            // floating start transition animation
            if (GetKirbyPose() != KirbyPose.FloatingStart)
            {
                ChangePose(KirbyPose.FloatingStart);
            }
            else if (GetKirbyPose() == KirbyPose.FloatingStart && poseCounter >= Constants.Kirby.STOP_FLOATING_TRANSITION_FRAME)
            {
                ChangePose(KirbyPose.FloatingRising);
            }
        }
        public void Float()
        {
            if (CanControl())
            {
                ResetAtWall();
                // play the floating sound every time the FloatingRising sprite loops
                if (state.GetPose() == KirbyPose.FloatingRising && poseCounter % Constants.Kirby.FLOATING_LOOP == 0)
                {
                    SoundManager.Play("float");
                }

                //1 start floating
                //2 go up 
                //3 float again if its falling
                //crouching and sliding cannot be overwritten by float 
                if (IsFloating() && GetKirbyPose() != KirbyPose.FloatingStart && GetKirbyPose() != KirbyPose.FloatingEnd && !IsFalling())
                {
                    movement.Jump(state.IsLeft());
                    ChangePose(KirbyPose.FloatingRising);
                }
                else if (state.CanFloat())
                {
                    StartFloating();
                }
            }
        }
        #endregion

        #region crouch
        public void Crouch()
        {
            if (CanControl())
            {
                if (state.CanCrouch() && !state.IsCrouching())
                { //crouch does not overwrite jump and floating
                    if (state.EnemyInMouth())
                    {
                        Swallow();
                        SoundManager.Play("swallow");
                    }
                    else
                    {
                        ChangePose(KirbyPose.Crouching);
                        movement = new CrouchingMovement(movement.GetPosition(), movement.GetVelocity());
                    }
                }
            }
        }
        public void Slide()
        {
            if (!IsSliding())
            {
                ChangePose(KirbyPose.Sliding);
            }
        }
        public void EndSlide()
        {
            movement.EndSlide();
            ChangePose(KirbyPose.Standing); //set back to standing

            ChangeToNormalMovement();
            attack?.EndAttack();
            attack = null;
        }
        public void EndCrouch()
        {
            if (CanControl())
            {
                if (state.IsCrouching())
                {
                    EndSlide(); //if sliding change to standing
                    ChangeToNormalMovement(); //change to normal
                    ChangePose(KirbyPose.Standing);

                    attack?.EndAttack();
                    attack = null;
                }
            }
        }

        public void EnterDoor()
        {
            if (CanControl())
            {
                if (_game.Level.atDoor(GetKirbyPosition()))
                {
                    ChangeToNormalMovement();
                    movement.StopMovement();
                    SoundManager.Play("enterdoor");
                    ChangePose(KirbyPose.EnterDoor);
                    _game.Level.EnterDoorAt(GetKirbyPosition());
                }
            }
        }
        #endregion

        #endregion //movement region
        #region Attack

        private void AttackAnimation()
        {
            if (attack.attackType == "Puff")
            {
                ChangePose(KirbyPose.FloatingEnd);
            }
            else
            {
                ChangePose(KirbyPose.Attacking);
            }

            isAttacking = true;
        }

        private void SetNewAttack(PlayerAttack playerAttack)
        {
            attack?.EndAttack();
            attack = playerAttack;
        }

        private int attackTimer;
        public void Attack()
        {
            if (CanControl())
            {
                wantToStopAttacking = false;
                attackTimer = 0;


                SetNewAttack(new PlayerAttack(this, AttackType()));
                AttackAnimation();
                movement.Attack(this);

            }
        }

        public void StopAttacking() //long attacks
        {
            wantToStopAttacking = true;
        }


        #endregion

        #region Mouthful
        public bool EnemyInMouth()
        {
            return state.EnemyInMouth();
        }
        public void EatEnemy(KirbyType kirbyType) //changes to mouthful state
        {
            if (kirbyType != KirbyType.Normal)
            {
                powerUp = kirbyType;
            }
            SoundManager.Play("catch");
            attack?.EndAttack();
            attack = null;
            ChangeToMouthful();
            ChangePose(KirbyPose.Catch);
        }

        private void Swallow() //swallows enemy
        {
            ChangePose(KirbyPose.Swallow);
            isSwallowing = true;
            swallowTimer = 0;
        }
        #endregion


        #region MoveKirby
        public void HandleMovementTransitions()
        {
            // continues start of Kirby's attack animations and ends it when counter is complete
            if (isAttacking && attack == null)
            {
                isAttacking = false;
            }
            else if (isAttacking && (
                   attack.attackType == "Normal" && attackTimer > Constants.Attack.END_INHALE && wantToStopAttacking
                || attack.attackType == "Beam" && attackTimer > Constants.Attack.END_BEAM
                || attack.attackType == "Spark" && attackTimer > Constants.Attack.END_SPARK && wantToStopAttacking
                || attack.attackType == "Fire" && attackTimer > Constants.Attack.END_FIRE && wantToStopAttacking
                || attack.attackType == "Professor" && attackTimer > Constants.Attack.END_PROFESSOR
                || attack.attackType == "Star" && attackTimer > Constants.Attack.END_STAR
                || attack.attackType == "Puff" && attackTimer > Constants.Attack.END_PUFF
                || attack.attackType == "Slide" && attackTimer > Constants.Attack.END_SLIDE
            ))
            {
                isAttacking = false;
                if (state.CanStand())
                {
                    ChangePose(KirbyPose.Standing);
                }
                // kirby mouthful attack, switch to normal attack
                if (attack.attackType == "Star")
                {
                    powerUp = KirbyType.Normal;
                    ChangeToNormal();
                }
                attack?.EndAttack();
                attack = null;
            }

            // finish swallow animation (this is important because !swallow is part of CanControl())
            if (isSwallowing)
            {
                swallowTimer++;
                if (swallowTimer > Constants.Kirby.STOP_SWALLOWING)
                {
                    isSwallowing = false;
                    ChangePose(KirbyPose.Standing);
                    state.ChangeType(powerUp);
                    if (powerUp != KirbyType.Normal)
                    {
                        Game1.Instance.Level.ChangeToPowerChangeState();
                        Attack();
                        powerChangeTimer = Constants.Transition.ATTACK_STATE_FRAMES;
                        powerChangeAnimation = true;
                        SoundManager.Play("powerup"); // must play the sound after switching the state because the state pauses all existing sounds
                    }
                }
            }

            // ends attack end animations
            if (GetKirbyPose() == KirbyPose.AttackingEnd &&
                (GetKirbyType().Equals("Normal") && poseCounter > Constants.Attack.END_ATTACK_INHALE_ANIMATION
                || GetKirbyType().Equals("Spark") && poseCounter > Constants.Attack.END_ATTACK_SPARK_ANIMATION
                || GetKirbyType().Equals("Fire") && poseCounter > Constants.Attack.END_ATTACK_FIRE_ANIMATION))
            {
                ChangePose(KirbyPose.Standing);
            }


            if (powerChangeTimer > 0)
            {
                powerChangeTimer--;
                if (powerChangeTimer == 0) {
                    StopAttacking();
                }
            }

            // ends wall squish animation
            if (GetKirbyPose() == KirbyPose.WallSquish && poseCounter >= Constants.Kirby.WALL_SQUISH_END)
            {
                ChangePose(KirbyPose.Standing);
            }

            // if kirby was still in the starting float sequence and the user stops pressing up, finish the sequence
            if (GetKirbyPose() == KirbyPose.FloatingStart)
            {
                StartFloating();
            }

            // end Kirby's hurt animation
            if (GetKirbyPose() == KirbyPose.Hurt && poseCounter >= Constants.Kirby.STOP_HURT_FRAME)
            {
                ChangePose(KirbyPose.Standing);
            }

            // on the first frame of spark and fire hurt animation launch Kirby into the air
            if ((GetKirbyPose() == KirbyPose.HurtSpark || GetKirbyPose() == KirbyPose.HurtFire) && poseCounter == 0)
            {
                movement.burnBounceJump();
            }

            // if Kirby was hurt by fire or spark and those animations should end, enter hurt bounce animation when Kirby next hits the ground
            if (GetKirbyPose() == KirbyPose.HurtFire && poseCounter >= Constants.Kirby.STOP_HURT_FIRE_FRAME
               || GetKirbyPose() == KirbyPose.HurtSpark && poseCounter >= Constants.Kirby.STOP_HURT_SPARK_FRAME)
            {
                shouldEnterBurnBounce = true;
            }

            // if Kirby is walking or running and has slowed all the way to a stop, then switch pose to standing. (this if check is messy, tidy up later)
            if ((GetKirbyPose() == KirbyPose.Walking || GetKirbyPose() == KirbyPose.Running) && GetKirbyVelocity().X == 0)
            {
                ChangePose(KirbyPose.Standing);
            }
        }

        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                HandleMovementTransitions();
                HandleFalling();
                if (!powerChangeAnimation)
                {
                    movement.MovePlayer(this, gameTime);
                }

                // start invincibility countdown after damage animations
                if (!state.IsHurt() && !shouldEnterBurnBounce)
                {
                    EndInvinciblility(gameTime);
                }

                playerSprite.Update();

                damageCounter++;

                // ensures player cannot move during damage animation
                if (!DEAD && hurtStun && damageCounter > Constants.Kirby.HURT_STUN_FRAMES) 
                {
                    hurtStun = false;
                    ChangePose(KirbyPose.FreeFall); 
                }

                if (DEAD)
                {
                    if (deathCounter == Constants.Kirby.START_DEATH_SPIN)
                    {
                        DeathSpin();
                    }
                    else if (deathCounter == Constants.Kirby.SET_DEATH_INACTIVE)
                    {
                        IsActive = false;
                    }
                    if (GetKirbyPose() == KirbyPose.DeathSpin && deathCounter % Constants.Kirby.DEATH_STAR_ANIMATION_LOOP == 0)
                    {
                        new CollisionStar(GetPosition());
                    }
                    deathCounter++;
                }

                movement.SetOnSlope(false);

                // update pose counter (number of updates since last pose change)
                if (oldPose != GetKirbyPose())
                {
                    poseCounter = 0;
                }
                else
                {
                    poseCounter++;
                }

                attackTimer++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                UpdateTexture();

                if (invincible)
                {
                    playerSprite.Draw(movement.GetPosition(), spriteBatch);
                    // Draw a second, tinted, translucent copy of the sprite on top of itself to tint it brigher. Use of unpremultiplied color in a premultiplied environment to do this
                    if (damageCounter % Constants.Kirby.INVINCIBLE_ANIMATION_LOOP < Constants.Kirby.INVINCIBLE_COLOR_CHANGE && !hurtStun)
                    {
                        playerSprite.Draw(movement.GetPosition(), spriteBatch, Constants.Graphics.INVINCIBLE_COLOR);
                    }

                }
                else
                {
                    playerSprite.Draw(movement.GetPosition(), spriteBatch);
                }

                // Draw an arrow pointing to this player if off screen IF this is not the player of the current view (and if not in a menu room)
                if (playerIndex != _game.CurrentCamera && !_game.Level.InMenuRoom())
                {
                    DrawArrow(spriteBatch);
                }

                // DEBUG TEXT (temporary so there are magic numbers)
                if (_game.DEBUG_TEXT_ENABLED)
                {
                    Vector2 position1 = GetKirbyPosition() + new Vector2(-24, -42);
                    Vector2 position2 = GetKirbyPosition() + new Vector2(-24, -32);
                    Vector2 position3 = GetKirbyPosition() + new Vector2(-24, -52);
                    position1.Floor();
                    position2.Floor();
                    position3.Floor();

                    spriteBatch.DrawString(LevelLoader.Instance.Font, movement.GetType().ToString().Substring(43), position3, Color.Black);
                    spriteBatch.DrawString(LevelLoader.Instance.Font, "poseCounter = " + poseCounter, position1, Color.Black);
                    spriteBatch.DrawString(LevelLoader.Instance.Font, GetKirbyPose().ToString(), position2, Color.Black);
                    //spriteBatch.DrawString(LevelLoader.Instance.Font, isTransitioningAttack.ToString(), position2, Color.Black);
                    //spriteBatch.DrawString(LevelLoader.Instance.Font, GetKirbyVelocity().X.ToString(), position1, Color.Black);
                    //spriteBatch.DrawString(LevelLoader.Instance.Font, GetKirbyVelocity().Y.ToString(), position2, Color.Black);
                    GameDebug.Instance.DrawPoint(spriteBatch, GetKirbyPosition() + GetKirbyVelocity() * 8, Color.Magenta, 1f);
                }


                UpdateOldStates();
            }
        }

        private void UpdateOldStates()
        {
            oldState = state.GetStateString();
            oldPose = GetKirbyPose();
        }
        public Vector2 GetPosition()
        {
            return GetKirbyPosition();
        }
        #endregion

        #region Arrow
        private void DrawArrow(SpriteBatch spriteBatch)
        {
            // Get camera bounds
            Rectangle cameraBounds = _game.cameras[_game.CurrentCamera].bounds;
            // Set bounds of which the arrow should draw if the player is outside
            int outset = Constants.Graphics.PLAYER_ARROW_VISIBILITY_BOUNDS_OUTSET;
            Rectangle visibilityBounds = new Rectangle(cameraBounds.X - outset, cameraBounds.Y - outset, cameraBounds.Width + outset * 2, cameraBounds.Height + outset * 2);
            // Set bounds to restrict the arrow to
            int inset = Constants.Graphics.PLAYER_ARROW_BOUNDS_INSET;
            int bottom_inset = Constants.Graphics.PLAYER_ARROW_BOUNDS_INSET;
            Rectangle arrowBounds = new Rectangle(cameraBounds.X + inset, cameraBounds.Y + inset, cameraBounds.Width - inset * 2, cameraBounds.Height - inset * 2 - bottom_inset);

            // Get the position of self
            Vector2 position = GetPosition();
            // Center the position to target vertically in the middle of Kirby's body (kirby's position is at his bottom-middle, at his feet)
            position.Y -= Constants.Kirby.KIRBY_VERTICAL_MIDDLE;

            // If my position is NOT in the current camera
            if (!visibilityBounds.Contains(position))
            {
                // Default arrow X to target X and sprite X to middle
                float x = position.X;
                int spriteX = 1;
                // If target is left of bounds, bound it
                if (arrowBounds.Left > x)
                {
                    x = arrowBounds.Left;
                    spriteX = 0;
                }
                // If target is right of bounds, bound it
                else if (x > arrowBounds.Right)
                {
                    x = arrowBounds.Right;
                    spriteX = 2;
                }

                // Default arrow Y to target Y and sprite Y to middle
                float y = position.Y;
                int spriteY = 1;
                // If target is above bounds, bound it
                if (arrowBounds.Top > y)
                {
                    y = arrowBounds.Top;
                    spriteY = 0;
                }
                // If target is below bounds, bound it
                else if (y > arrowBounds.Bottom)
                {
                    y = arrowBounds.Bottom;
                    spriteY = 2;
                }

                Vector2 arrowposition = new Vector2(x, y);
                // Offset arrow position based on player index to avoid arrows stacking and obscuring each other (especially in corners)
                switch (playerIndex)
                {
                    case 0:
                        arrowposition.X += -1;
                        arrowposition.Y += -2;
                        break;
                    case 1:
                        arrowposition.X += 2;
                        arrowposition.Y += -1;
                        break;
                    case 2:
                        arrowposition.X += -2;
                        arrowposition.Y += 1;
                        break;
                    case 3:
                        arrowposition.X += 1;
                        arrowposition.Y += 2;
                        break;
                }
                // Draw the arrow's corresponding sprite
                playerarrows[spriteX][spriteY].Draw(arrowposition, spriteBatch);
            }
        }
        #endregion

        #region Collisions
        //kirby collides with the top of a block
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.ENTITY_WIDTH / 2;
            float y = pos.Y - Constants.HitBoxes.ENTITY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(GetKirbyPosition());
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }

        public void HandleFreeFall()
        {
            // ensures the right animation of bounce, freefallfar, or freefall is executed
            if (GetKirbyPose() == KirbyPose.Bounce)
            {
                ChangePose(KirbyPose.Bounce);
                if (poseCounter == Constants.Kirby.BOUNCE_JUMP_FRAME)
                {
                    movement.bounceJump();
                }
                else if (poseCounter > Constants.Kirby.STOP_BOUNCE_FRAME)
                {
                    ChangePose(KirbyPose.Standing);
                    IParticle star = new CollisionStar(movement.GetPosition());
                }
            }
            else if (GetKirbyPose() == KirbyPose.FreeFallFar)
            {
                ChangePose(KirbyPose.Bounce);
                new CollisionStar(movement.GetPosition());
                SoundManager.Play("bounce");
            }
            // if Kirby was falling
            else if (GetKirbyPose() == KirbyPose.FreeFall || GetKirbyPose() == KirbyPose.JumpFalling)
            {
                ChangePose(KirbyPose.Standing);
                new CollisionStar(movement.GetPosition());
                movement.ChangeKirbyLanded(true);
            }
            else if (GetKirbyPose() == KirbyPose.FloatingFalling)
            {
                movement.ChangeKirbyLanded(true);
            }

            // complete burn animation sequence
            if (shouldEnterBurnBounce)
            {
                ChangePose(KirbyPose.BurnBounce);
                if (poseCounter == Constants.Kirby.BOUNCE_JUMP_FRAME)
                {
                    movement.burnBounceJump();
                }
                else if (poseCounter > Constants.Kirby.STOP_BURN_BOUNCE_FRAME)
                {
                    ChangePose(KirbyPose.Bounce);
                    shouldEnterBurnBounce = false;
                }
            }
        }

        public void BottomCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionBlock(intersection);
            HandleFreeFall();
        }

        //kirby collides with the right side of a block
        public void RightCollisionWithBlock(Rectangle intersection)
        {
            // ensures Kirby pose is unchanged when floating, jumping, and in the damage animations
            if (!state.IsFloating() && !state.IsJumping() && !state.IsFalling() && !isAttacking && !state.IsHurt())
            {
                ChangePose(KirbyPose.Standing);
            }
            // detects initial collision
            if ((oldPose == KirbyPose.Walking || oldPose == KirbyPose.Running) && GetKirbyPose() == KirbyPose.Standing)
            {
                IParticle star = new CollisionStar(movement.GetPosition());
                ChangePose(KirbyPose.WallSquish);
            }
            movement.AdjustFromRightCollisionBlock(intersection);
            if (!state.IsLeft())
            {
                facingRightWall = true;
            }
        }
        //kirby collides with the left side of a block
        public void LeftCollisionWithBlock(Rectangle intersection)
        {
            // ensures Kirby pose is unchanged when floating, jumping, and in the damage animations
            if (!state.IsFloating() && !state.IsJumping() && !state.IsFalling() && !isAttacking && !state.IsHurt())
            {
                ChangePose(KirbyPose.Standing);
            }
            // detects initial collision
            if ((oldPose == KirbyPose.Walking || oldPose == KirbyPose.Running) && GetKirbyPose() == KirbyPose.Standing)
            {
                IParticle star = new CollisionStar(movement.GetPosition());
                ChangePose(KirbyPose.WallSquish);
            }
            movement.AdjustFromLeftCollisionBlock(intersection);
            if (state.IsLeft())
            {
                facingLeftWall = true;
            }
        }

        public void TopCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromTopCollisionBlock(intersection);
        }
        //kirby collides with the bottom of a plataform
        public void BottomCollisionWithPlatform(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionPlatform(intersection, state);
            HandleFreeFall();
        }

        //slope collision
        public void CollisionWithGentle1LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
        }
        //slope collision
        public void CollisionWithGentle2LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
        }
        //slope collision
        public void CollisionWithSteepLeftSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
        }
        //slope collision
        public void CollisionWithGentle1RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
        }
        //slope collision
        public void CollisionWithGentle2RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
        }
        //slope collision
        public void CollisionWithSteepRightSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
        }
        #endregion
    }

}

