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
        private double timer = 0;

        public bool powerChangeAnimation { get; set; } = false; // is this kirby currently in a power change animation?

        //others
        private string oldState;
        private KirbyPose oldPose;
        public int poseCounter { get; private set; }
        public bool attackIsActive { get; private set; } = false;
        public bool CollisionActive { get; private set; } = true;
        public bool DEAD { get; private set; } = false;
        private int deathCounter = 0;
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
            playerarrows = new Sprite[3][];
            // For each of the 9 sprites in the 3x3 arrow texture grid
            for (int x = 0; x <= 2; x++)
            {
                playerarrows[x] = new Sprite[3];
                for (int y = 0; y <= 2; y++)
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
            ChangeToNormalMovement();
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
        private int lastFramePoseChanged = 0;
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

        private bool CanControl()
        {
            return IsActive && !DEAD && !hurtStun && !isAttacking && GetKirbyPose() != KirbyPose.Swallow;
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
            //wait a beat
            invincible = true;
            hurtStun = false; // so that kirby flashes yellow like when invincible
            SoundManager.Play("deathjingle");
            ChangePose(KirbyPose.DeathSpin);
            movement.DeathSpin();

        }


        public void TakeDamage(Rectangle intersection, Vector2 positionOfDamageSource)
        {
            if (!invincible)
            {
                invincible = true;
                hurtStun = true;
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
                    TakeDamageAnimation();
                    movement.ReceiveDamage(intersection);
                }
            }

        }
        //calls method to drecease health & changes kirby pose
        private void TakeDamageAnimation()
        {
            if (invincible)
            {
                if (state.HasPowerUp())
                {
                    DropAbility();
                    // I have no idea why this code below was here -mark
                    //if (!state.IsCrouching())
                    //{
                    //    AttackAnimation();
                    //}
                    //movement.Attack(this);
                }

                if (IsFloating())
                {
                    movement = new NormalPlayerMovement(GetKirbyPosition(), movement.GetVelocity());
                }
                ChangePose(KirbyPose.Hurt);
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
            movement.GoToRoomSpawn();
            ChangePose(KirbyPose.Standing);
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
                movement.Walk(state.IsLeft());
                //check if kirby should change pose
                if (state.CanMove())
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
                //walk connot override walking, jumping, floating, crouching, and attack
                if (state.CanMove())
                {
                    ChangePose(KirbyPose.Walking);
                }
            }
        }

        // TODO: clean up how ugly this method is
        public void Fall()
        {
            // sould kirby exhibit falling behavior
            if (movement.GetVelocity().Y > 0 && !movement.onSlope && !DEAD && !state.IsFloating() && !state.IsAttacking())
            {
                // if kirby was not falling enter freefall
                if (!state.IsFalling() && !hurtStun)
                {
                    ChangePose(KirbyPose.FreeFall);
                }
                // should kirby enter free fall far
                if (GetKirbyPose() == KirbyPose.FreeFall && poseCounter > Constants.Kirby.MINFREEFALLFARFRAMES && state.GetKirbyType() != KirbyType.Mouthful)
                {
                    ChangePose(KirbyPose.FreeFallFar);
                }

                // changes bounce and falling poses to be named the same pose (they are the same)
                if (GetKirbyPose() == KirbyPose.JumpFalling && poseCounter > 12)
                {
                    ChangePose(KirbyPose.FreeFall);
                }
                if (GetKirbyPose() == KirbyPose.Bounce && poseCounter > 16)
                {
                    ChangePose(KirbyPose.FreeFall);
                }

                movement.ChangeKirbyLanded(false);
            }


            // changes kirby to freefall once stop floating should end
            if (GetKirbyPose() == KirbyPose.FloatingEnd && poseCounter > Constants.Kirby.STOPFLOATINGTRANSITIONFRAME)
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
                movement.Run(state.IsLeft());
                if (state.CanMove())
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
                movement.Run(state.IsLeft());
                if (state.CanMove())
                {
                    DashEffects();
                    ChangePose(KirbyPose.Running);
                }
            }
        }

        private void DashEffects()
        {
            // If on the first frame of the dash, play the dash sound
            if (poseCounter == 1) // && oldPose != KirbyPose.Running)
            {
                SoundManager.Play("dash");
            }
            // If on one of the first n multiples of the dash cloud animation length, create a new dash cloud particle (create three back-to-back)
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
            if (GetKirbyPose() != KirbyPose.FloatingStart)
            {
                ChangePose(KirbyPose.FloatingStart);
            }
            else if (GetKirbyPose() == KirbyPose.FloatingStart && poseCounter >= Constants.Kirby.STOPFLOATINGTRANSITIONFRAME)
            {
                ChangePose(KirbyPose.FloatingRising);
            }
        }
        public void Float()
        {
            if (CanControl())
            {
                // Play the floating sound every time the FloatingRising sprite loops
                if (state.GetPose() == KirbyPose.FloatingRising && poseCounter % 16 == 0)
                {
                    SoundManager.Play("float");
                }

                //1 start floating
                //2 go up 
                //3 float again if its fallign
                //crouching and sliding cannot be overwritten by float 
                if (IsFloating() && GetKirbyPose() != KirbyPose.FloatingStart && GetKirbyPose() != KirbyPose.FloatingEnd && !IsFalling())
                {
                    if (!movement.GetType().Equals(typeof(FloatingMovement)))
                    {
                        movement = new FloatingMovement(movement.GetPosition(), movement.GetVelocity());
                    }
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
            if (!IsSliding()) // && attack != null)
            {
                ChangePose(KirbyPose.Sliding);
                //await Task.Delay(Constants.Physics.DELAY);
            }
        }
        public void EndSlide()
        {
            movement.EndSlide();
            ChangePose(KirbyPose.Standing); //set back to crouching
                                                //ChangeAttackBool(false);  //stop attack mode
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
                    EndSlide(); //if sliding changes to standin
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
                    SoundManager.Play("enterdoor");
                    ChangePose(KirbyPose.EnterDoor);
                }
                _game.Level.EnterDoorAt(GetKirbyPosition());
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
        public void AttackPressed()
        {
            if (CanControl())
            {
                //flame spark inhale
                //if (attack == null && state.LongAttack())
                //{
                //    SetNewAttack(new PlayerAttack(this, AttackType()));
                //    ChangePose(KirbyPose.Attacking);
                //    movement.Attack(this);
                //    Debug.WriteLine("ATTACKPRESSED");
                //}
            }
        }
        private bool wantToStopAttacking;
        public void StopAttacking() //long attacks
        {
            wantToStopAttacking = true;
            //if (CanControl())
            //{
            //    if (attack != null && (attack.IsDone() || state.LongAttack()))
            //    {
            //        //StopMoving();
            //        if (state.CanStand())
            //        {
            //            ChangePose(KirbyPose.Standing);
            //        }
            //        attack?.EndAttack();
            //        attack = null;
            //    }
            //}
        }


        #endregion

        #region Mouthful
        public bool EnemyInMouth()
        {
            return state.EnemyInMouth();
        }
        public void EatEnemy(KirbyType kirbyType) //changes to mouthful state
        {
            powerUp = kirbyType;
            SoundManager.Play("catch");
            attack?.EndAttack();
            attack = null;
            ChangeToMouthful();
            ChangePose(KirbyPose.Catch);
        }
        private void SwallowAnimation()
        {
            ChangePose(KirbyPose.Swallow);
        }
        private void Swallow() //swallows enemy
        {
            SwallowAnimation();
            //if (powerUp != KirbyType.Normal)
            //{
            //    Game1.Instance.Level.ChangeToPowerChangeState();
            //}
            //ChangePose(KirbyPose.Standing);

            // moved second part to HandleMovementTransitions
        }
        #endregion


        bool TEMP = false;
        private int powerChangeTimer = 0;
        #region MoveKirby
        public void HandleMovementTransitions()
        {
            // continues start of Kirby's attack animations and ends it when counter is complete
            if (isAttacking && attack == null)
            {
                isAttacking = false;
            }
            else if (isAttacking && (
                   attack.attackType == "Normal"    && attackTimer > 30 && wantToStopAttacking
                || attack.attackType == "Beam"      && attackTimer > 42
                || attack.attackType == "Spark"     && attackTimer > 20 && wantToStopAttacking
                || attack.attackType == "Fire"      && attackTimer > 30 && wantToStopAttacking
                || attack.attackType == "Professor" && attackTimer > 26
                || attack.attackType == "Star"      && attackTimer > 15
                || attack.attackType == "Puff"      && attackTimer > 9
                || attack.attackType == "Slide"     && attackTimer > 40
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
            if (GetKirbyPose() == KirbyPose.Swallow && poseCounter > 25)
            {
                ChangePose(KirbyPose.Standing);
                state.ChangeType(powerUp);
                if (powerUp != KirbyType.Normal)
                {
                    Attack();
                    powerChangeTimer = Constants.Transition.ATTACK_STATE_FRAMES;
                    SoundManager.Play("powerup");
                    powerChangeAnimation = true;
                    Game1.Instance.Level.ChangeToPowerChangeState();
                }
            }

            if (powerChangeTimer > 0)
            {
                powerChangeTimer--;
                if (powerChangeTimer == 0) {
                    StopAttacking();
                }
            }

            // if kirby was still in the starting float sequence and the user stops pressing up, finish the sequence
            if (GetKirbyPose() == KirbyPose.FloatingStart)
            {
                StartFloating();
            }

            // end Kirby's hurt animation
            if (GetKirbyPose() == KirbyPose.Hurt && poseCounter >= Constants.Kirby.STOPHURTFRAME)
            {
                ChangePose(KirbyPose.Standing);
            }

            // if Kirby is walking or running and has slowed all the way to a stop, then switch pose to standing. (this if check is messy, tidy up later)
            if ((GetKirbyPose() == KirbyPose.Walking || GetKirbyPose() == KirbyPose.Running) && GetKirbyVelocity().X == 0)
            {
                ChangePose(KirbyPose.Standing);
            }
        }

        string test1;
        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                HandleMovementTransitions();
                Fall();
                if (!powerChangeAnimation)
                {
                    movement.MovePlayer(this, gameTime);
                }
                EndInvinciblility(gameTime);
                test1 = state.GetStateString();
                //UpdateTexture();
                playerSprite.Update();

                damageCounter++;
                if (!DEAD && hurtStun && damageCounter > Constants.Kirby.HURT_STUN_FRAMES) // THIS IS MESSY
                {
                    hurtStun = false;
                    ChangePose(KirbyPose.FreeFall); // THIS IS DUMB AND WRONG CHANGE IT LATER
                }

                if (DEAD)
                {
                    if (deathCounter == 90)
                    {
                        DeathSpin();
                    }
                    else if (deathCounter == 240)
                    {
                        IsActive = false;
                    }
                    if (GetKirbyPose() == KirbyPose.DeathSpin && deathCounter % 8 == 0)
                    {
                        new CollisionStar(GetPosition());
                    }
                    deathCounter++;
                }

                movement.SetOnSlope(false);

                // Update pose counter (number of updates since last pose change)
                if (oldPose != GetKirbyPose())
                {
                    poseCounter = 0;
                }
                else
                {
                    poseCounter++;
                }

                attackTimer++;
                testint = 0;
                TEMP = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                UpdateTexture();

                //string test2 = state.GetStateString();
                //if (test1 != test2)
                //{
                //    Debug.WriteLine("SPRITE MISMATCH, " + test1 + ", " + test2);
                //}

                if (invincible)
                {
                    playerSprite.Draw(movement.GetPosition(), spriteBatch);
                    // Draw a second, tinted, translucent copy of the sprite on top of itself to tint it brigher. Use of unpremultiplied color in a premultiplied environment to do this. Stupid
                    if (damageCounter % 8 < 4 && !hurtStun)
                    {
                        playerSprite.Draw(movement.GetPosition(), spriteBatch, Constants.Graphics.INVINCIBLE_COLOR);
                    }

                }
                else
                {
                    playerSprite.Draw(movement.GetPosition(), spriteBatch);
                }

                // Draw an arrow pointing to this player if off screen IF this is not the player of the current view
                if (playerIndex != _game.CurrentCamera)
                {
                    DrawArrow(spriteBatch);
                }

                // TEMPORARY, WILL DELETE SOON
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
            position.Y -= 8;

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

        public int testint = 0;
        public void HandleFreeFall()
        {
            // ensures the right animation of bounce, freefallfar, or freefall is executed
            if (GetKirbyPose() == KirbyPose.Bounce)
            {
                ChangePose(KirbyPose.Bounce);
                if (poseCounter == Constants.Kirby.BOUNCEJUMPFRAME)
                {
                    testint = poseCounter;
                    movement.bounceJump();
                }
                else if (poseCounter > Constants.Kirby.STOPBOUNCEFRAME)
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
        }

        public void BottomCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionBlock(intersection);
            HandleFreeFall();
            TEMP = true;
        }

        //kirby collides with the right side of a block
        public void RightCollisionWithBlock(Rectangle intersection)
        {
            // ensures Kirby pose is unchanged when floating and jumping
            if (!state.IsFloating() && !state.IsJumping())
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
        }
        //kirby collides with the left side of a block
        public void LeftCollisionWithBlock(Rectangle intersection)
        {
            // ensures Kirby pose is unchanged when floating and jumping
            if (!state.IsFloating() && !state.IsJumping())
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
            TEMP = true;
        }
        //kirby collision with air so he falls
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            if (oldPose == KirbyPose.FreeFall)
            {
                ChangePose(KirbyPose.Standing);
            }
        }

        //slope collision
        public void CollisionWithGentle1LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
            TEMP = true;
        }
        //slope collision
        public void CollisionWithGentle2LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
            TEMP = true;
        }
        //slope collision
        public void CollisionWithSteepLeftSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
            TEMP = true;
        }
        //slope collision
        public void CollisionWithGentle1RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
            TEMP = true;
        }
        //slope collision
        public void CollisionWithGentle2RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
            TEMP = true;
        }
        //slope collision
        public void CollisionWithSteepRightSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept, this);
            TEMP = true;
        }
        #endregion
    }

}

