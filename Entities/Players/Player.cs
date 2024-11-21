using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Particles;
using static KirbyNightmareInDreamLand.Constants;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Projectiles;
using System.Collections.Generic;
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
        private int spriteDamageCounter;
        private Sprite[][] playerarrows;
        public PlayerAttack attack {get; private set;}
        public PlayerAttack starAttackOne {get; private set;}
        public PlayerAttack starAttackTwo {get; private set;}

        public int health { get; private set; }
        public int lives { get; private set; }
        private bool invincible = false;
        private double timer = 0;

        //others
        private string oldState;
        private KirbyPose oldPose;
        private int poseCounter;
        public bool attackIsActive{ get; private set; } = false;
        public bool CollisionActive { get; private set; } = true;
        public bool DEAD = false;
        public bool lifeLost = false;
        private KirbyType powerUp = KirbyType.Normal;

        //collision stuffs

        //constructor
        public Player(Vector2 pos, int playerIndex)
        {
            _game = Game1.Instance;
            this.playerIndex = playerIndex;
            state = new PlayerStateMachine(playerIndex);
            movement = new NormalPlayerMovement(pos);
            oldState = null;
            oldPose = state.GetPose();
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

        public CollisionType GetCollisionType()
        {
            return CollisionType.Player;
        }

        //changes kiry's texture if he is in a different state than before
        //only called by Draw
        private void UpdateTexture()
        {
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = SpriteFactory.Instance.CreateSprite(state.GetSpriteParameters());
            } 
        }

        #region KirbyState
        public void ChangePose(KirbyPose pose)
        {
            state.ChangePose(pose);
            if (oldPose != state.GetPose())
            {
                poseCounter = 0;
            }
            else
            {
                poseCounter++;
            }
        }
        public void ChangeMovement()
        {
            movement = new NormalPlayerMovement(movement.GetPosition());
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
            if(GetKirbyType().Equals("Mouthful"))
            {
                return "Normal";
            } else {
                return state.GetKirbyType().ToString();
            }
            
        }
        public bool IsLeft(){
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
            if(IsFloating()&& !IsFalling()){
                return "Puff";
            } else if (state.IsCrouching()){
                return "Slide";
            }else if (state.IsWithEnemy()){
                return "Star";
            }else{
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
            return powerUp;
        }
        #endregion
        #region direction
        public void SetDirectionLeft()
        {
            if(!state.IsAttacking()){
                state.SetDirectionLeft();
            }
        }
        public void SetDirectionRight()
        {
            if(!state.IsAttacking()){
                state.SetDirectionRight();
            }
        }
        #endregion

        #region health 
        public void RestartKirby()
        {
            //state.ChangeType(KirbyType.Dead);
            CollisionActive = true;
            movement.StopMovement();
            ChangePose(KirbyPose.Standing);
            invincible = false;
            timer = 0;
        } 
        public async void Death()
        {
            state.ChangePose(KirbyPose.Standing);
            StopMoving(); 
            ChangeToNormal();
            ChangeMovement();
            SoundManager.Play("kirbydeath");
            state.ChangePose(KirbyPose.DeathStun);
            await Task.Delay(Constants.WaitTimes.DELAY_1500);
            DeathSpin();
        }
        public void DeathSpin()
        {
            //wait a beat
            SoundManager.Play("deathjingle");
            state.ChangePose(KirbyPose.DeathSpin);
            movement.DeathSpin();
            CollisionActive = false;
        }
        public void FallOffScreenDeath()
        {
            health = 0;
            lives --;
            Game1.Instance.Level.ChangeToLifeLost();
            movement.DeathMovement();
            DeathSpin();
            if(lives == 0){
                //go to game over
                //Game1.Instance.Level.GameOver();
                DEAD = true;
            }else {
                FillHealth();
            }
        }
        private void DecreaseHealth(Rectangle intersection)
        {
            if(timer ==0) health --; //decrease health
            if(health == 0) //health decresed to 0 and lost life
            {
                lifeLost = true;
                lives--;
                Game1.Instance.Level.ChangeToLifeLost();
                movement.DeathMovement();
                Death();
                if(lives == 0){
                    //go to game over
                    //Game1.Instance.Level.GameOver();
                    DEAD = true;
                }else {
                    FillHealth();
                }
            } else { //health decreased,  but didnt loose life
                TakeDamageAnimation();
                movement.ReceiveDamage(intersection);
            }
            
        }

        //calls method to drecease health & changes kirby pose
        private async void TakeDamageAnimation()
        {
            if (invincible)
            {
                if (state.HasPowerUp())
                {
                    //starAttackTwo = new PlayerAttack(this, "BouncingStar");
                    DropAbility();
                    if (!state.IsCrouching())
                    {
                        AttackAnimation();
                    }
                    movement.Attack(this);
                }
                
                if (IsFloating())
                {
                    movement = new NormalPlayerMovement(GetKirbyPosition());
                }
                ChangePose(KirbyPose.Hurt);
                SoundManager.Play("kirbyhurt1");
                await Task.Delay(Constants.WaitTimes.DELAY_400);
                StopMoving();
            }
        }

        public void DropAbility()
        {
            if (true || state.HasPowerUp())
            {
                if (!IsWithEnemy())
                {
                    new KirbyBouncingStar(GetKirbyPosition(), IsLeft(), GetPowerUp());
                    new DropAbility(GetKirbyPosition());
                    ChangeToNormal();
                }
            }
        }

        public void ManualDropAbility()
        {
            if (true || state.HasPowerUp())
            {
                SoundManager.Play("dropability");
                DropAbility();
            }
        }

        private void EndInvinciblility(GameTime gameTime)
        {
            if(invincible){
                timer += gameTime.ElapsedGameTime.TotalSeconds; 
                if(timer > Constants.Kirby.INVINCIBLE_TIME){
                    invincible = false;
                    timer = 0;
                }
            }
        }
        public void TakeDamage(Rectangle intersection)
        {
            if(!invincible)
            {
                invincible = true;
                spriteDamageCounter = 0;
                DecreaseHealth(intersection);
            }
            
        }
        public void FillHealth()
        {
            health = Constants.Kirby.MAX_HEALTH;
        }
        public void FillFullHealth()
        {
            health = Constants.Kirby.MAX_HEALTH;
            lives = Constants.Kirby.MAX_LIVES;
        }
        #endregion

        #region Movement
        public void GoToRoomSpawn()
        {
            movement.GoToRoomSpawn();
        }
        
        public void MoveLeft()
        {   
            SetDirectionLeft();
            movement.Walk(state.IsLeft());
            //check if kirby should change pose
            if(state.CanMove()){
                ChangePose(KirbyPose.Walking);
            }
        }

        public void MoveRight()
        {
            SetDirectionRight();
            movement.Walk(state.IsLeft());
            //walk connot override walking, jumping, floating, crouching, and attack
            if(state.CanMove()){
                ChangePose(KirbyPose.Walking);
            }
        }
        public void StopMoving() 
        {
            movement.StopMovement();
            if(state.CanStand())
            {
                ChangePose(KirbyPose.Standing);
            }
        }
        #region running
        public void RunLeft()
        {
            SetDirectionLeft();
            movement.Run(state.IsLeft());
            if (state.CanMove())
            {
                // transitions from a brief state of standing that is not seen by the user to running
                if (state.GetPose() == KirbyPose.Standing || poseCounter != 0)
                {
                    // Play dash sound and create particle accordingly
                    DashEffects();
                }
                ChangePose(KirbyPose.Running);
            }
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run(state.IsLeft());
            if(state.CanMove())
            {
                // transitions from a brief state of standing that is not seen by the user to running
                if (state.GetPose() == KirbyPose.Standing || poseCounter != 0)
                {
                    // Play dash sound and create particle accordingly
                    DashEffects();
                }
                ChangePose(KirbyPose.Running);
            }
        }

        private void DashEffects()
        {
            // If on the first frame of the dash, play the dash sound
            if (poseCounter == 0 && oldPose != KirbyPose.Running)
            {
                SoundManager.Play("dash");
            }
            // If on one of the first n multiples of the dash cloud animation length, create a new dash cloud particle (create three back-to-back)
            if (poseCounter % Constants.Particle.DASH_CLOUD_FRAMES == 0 && poseCounter < Constants.Particle.DASH_CLOUD_FRAMES * Constants.Particle.DASH_CLOUD_LOOPS)
            {
                IParticle cloud = new DashCloud(this);
            }
        }
        #endregion

        #region jumping
        public void Jump()
        {
            if(state.CanJump()){ //not floating, not jumping, not crouching
                movement = new JumpMovement(movement.GetPosition());
                ChangePose(KirbyPose.JumpRising);
                SoundManager.Play("jump");
            }else if (state.IsJumping() && !state.IsFloating()){ //if jumping and x is pressed again
                //Float();
                movement.Jump(state.IsLeft());
            }
        }
        #endregion
        private async void StartFloating()
        {
            ChangePose(KirbyPose.FloatingStart);
            await Task.Delay(Constants.WaitTimes.DELAY_800);
        }
        public void Float()
        {
            //1 start floating
            //2 go up 
            //3 float again if its fallign
            //crouching and sliding cannot be overwritten by float 
            if (IsFloating() && !IsFalling()){ //covers 
                movement.Jump(state.IsLeft()); 
                ChangePose(KirbyPose.FloatingRising);
            } else if (state.CanFloat()){
                if(!movement.GetType().Equals(new FloatingMovement(movement.GetPosition())))
                {
                    movement = new FloatingMovement(movement.GetPosition());
                }
                StartFloating();
                ChangePose(KirbyPose.FloatingRising);
            }
        }

        #region crouch
        public void Crouch()
        {
            if(state.CanCrouch() && !state.IsCrouching() && !state.IsWithEnemy()){ //crouch does not overwrite jump and floating
                ChangePose(KirbyPose.Crouching);
                movement = new CrouchingMovement(movement.GetPosition());
            } 
            if(state.IsWithEnemy())
            {
                EndSwallow();
                SoundManager.Play("swallow");
            }
        }
        public void Slide()
        {
            if(!IsSliding()) // && attack != null)
            {
                ChangePose(KirbyPose.Sliding);
                //await Task.Delay(Constants.Physics.DELAY);
            }
        }
        public void EndSlide()
        {
            if(state.IsSliding()){
                movement.EndSlide();
                movement.StopMovement(); //set vel to 0
                ChangePose(KirbyPose.Crouching); //set back to crouching
                //ChangeAttackBool(false);  //stop attack mode
                
                attack?.EndAttack();
                attack = null;
            }
        }
        public void EndCrouch()
        {
            //if(state.IsCrouching()){
                EndSlide(); //if sliding changes to standin
                ChangeMovement(); //change to normal
                StopMoving(); //set vel to 0 and standing

                attack?.EndAttack();
                attack = null;
            //} 
        }
        #endregion
        
        #endregion //movement region
        #region Attack

        private async void AttackAnimation()
        {
            //beam float exhale mouthful exhale
            ChangePose(KirbyPose.Attacking);
            if(GetKirbyType().Equals("Beam")){
                await Task.Delay(Constants.WaitTimes.DELAY_500);
                //ChangeAttackBool(false);
                attack = null;
            } else {  //float and mouthful exhale
                await Task.Delay(Constants.WaitTimes.DELAY_200);
            }
            if (!state.IsFloating())
            {
                ChangePose(KirbyPose.Standing);
            }
            if (IsWithEnemy())
            {
                ChangeToNormal();
            }
        }
        public void Attack()
        {
            //mouthful exhale -- spits out star
            if(IsWithEnemy() && state.ShortAttack())
            {
                starAttackOne = new PlayerAttack(this, "Star");
                if (!state.IsCrouching())
                {
                    AttackAnimation();
                }
                movement.Attack(this);
                //ChangeAttackBool(true);
            }
            //slide beam float exhale
            else if (attack == null && state.ShortAttack())
            {
                attack = new PlayerAttack(this, AttackType());
                if (!state.IsCrouching())
                {
                    AttackAnimation();
                }
                movement.Attack(this);
            }
        }
        public void AttackPressed()
        {
            //flame spark inhale
            if(attack == null && state.LongAttack()){
                attack = new PlayerAttack(this, AttackType());
                ChangePose(KirbyPose.Attacking);
                movement.Attack(this);
                //ChangeAttackBool(true);
            }

        }
        public void StopAttacking() //long attacks
        {
            if(attack != null && (attack.IsDone() || state.LongAttack()))
            {
                StopMoving();
                attack.EndAttack();
                attack = null;
            }
            if (starAttackOne != null && starAttackOne.IsDone()){
                //StopMoving();
                starAttackOne.EndAttack();
                starAttackOne = null;
            }
            if (starAttackTwo != null && starAttackTwo.IsDone()){
                //StopMoving();
                starAttackTwo.EndAttack();
                starAttackTwo = null;
            }

        }
       

        #endregion

        #region Mouthful
        public bool IsWithEnemy()
        {
            return state.IsWithEnemy();
        }
        private async void SmallWait()
        {
            await Task.Delay(10000);
        }
        public void SwallowEnemy(KirbyType kirbyType) //changes to mouthful state
        {
            powerUp = kirbyType;
            SmallWait();
            SoundManager.Play("catch");
            StopAttacking();
            ChangeToMouthful();
        }
        private void SwallowAnimation()
        {
            state.ChangePose(KirbyPose.Swallow);
        }
        private void EndSwallow() //swallows enemy
        {
            SwallowAnimation();
            ChangePose(KirbyPose.Standing);
            state.ChangeType(powerUp);
        }
        #endregion


        bool TEMP = false;
        #region MoveKirby
        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            if (movement.GetVelocity().Y > 0)
            {
                ChangePose(KirbyPose.FreeFall);
            }
            movement.MovePlayer(this, gameTime);
            EndInvinciblility(gameTime);
            playerSprite.Update();
            spriteDamageCounter++;
            if (attack != null || starAttackOne != null || starAttackTwo != null)
            {
               //attack?.Update(gameTime, this);
               //starAttackOne?.Update(gameTime, this);
               //starAttackTwo?.Update(gameTime, this);
            }
            if (lifeLost)
            {
                Death();
                lifeLost = false;
            }

            TEMP = false;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateTexture();
            
            if(invincible){
                playerSprite.Draw(movement.GetPosition(), spriteBatch);
                // Draw a second, tinted, translucent copy of the sprite on top of itself to tint it brigher. Use of unpremultiplied color in a premultiplied environment to do this. Stupid
                if (spriteDamageCounter % 8 < 4)
                {
                    playerSprite.Draw(movement.GetPosition(), spriteBatch, Constants.Graphics.INVINCIBLE_COLOR);
                }
                
            } else {
                playerSprite.Draw(movement.GetPosition(), spriteBatch);
            }

            // Draw an arrow pointing to this player if off screen
            DrawArrow(spriteBatch);

            if (attack != null || starAttackOne != null || starAttackTwo != null)
            {
               //attack?.Draw(spriteBatch, this);
               //starAttackOne?.Draw(spriteBatch, this);
               //starAttackTwo?.Draw(spriteBatch, this);
            }

            // TEMPORARY, WILL DELETE SOON
            //Vector2 position = GetKirbyPosition() + new Vector2(-24, -32);
            //spriteBatch.DrawString(LevelLoader.Instance.Font, TEMP.ToString() + GetKirbyVelocity().X.ToString(), position, Color.Black);

            UpdateOldStates();
        }

        private void UpdateOldStates()
        {
            oldState = state.GetStateString();
            oldPose = state.GetPose();
        }
        public  Vector2 GetPosition()
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
            float x = pos.X - Constants.HitBoxes.ENTITY_WIDTH/2;
            float y = pos.Y - Constants.HitBoxes.ENTITY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint; 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(GetKirbyPosition());
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }
        public void BottomCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionBlock(intersection);
            TEMP = true;
            if (state.GetPose() == KirbyPose.FreeFall)
            {
                ChangePose(KirbyPose.Standing);
            }
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
            if ((oldPose == KirbyPose.Walking || oldPose == KirbyPose.Running) && state.GetPose() == KirbyPose.Standing )
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
            if ((oldPose == KirbyPose.Walking || oldPose == KirbyPose.Running) && state.GetPose() == KirbyPose.Standing)
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
            if (state.GetPose() == KirbyPose.FreeFall)
            {
                ChangePose(KirbyPose.Standing);
            }
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

        public void HandleSlopeFreeFall()
        {
            if (state.GetPose() == KirbyPose.FreeFall && movement.GetVelocity().X == 0)
            {
                ChangePose(KirbyPose.Standing);
            }
        }

        //slope collision
        public void CollisionWithGentle1LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
            TEMP = true;
            HandleSlopeFreeFall();
        }
        //slope collision
        public void CollisionWithGentle2LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
            TEMP = true;
            HandleSlopeFreeFall();
        }
        //slope collision
        public void CollisionWithSteepLeftSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
            TEMP = true;
            HandleSlopeFreeFall();
        }
        //slope collision
        public void CollisionWithGentle1RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
            TEMP = true;
            HandleSlopeFreeFall();
        }
        //slope collision
        public void CollisionWithGentle2RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
            TEMP = true;
            HandleSlopeFreeFall();
        }
        //slope collision
        public void CollisionWithSteepRightSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
            TEMP = true;
            HandleSlopeFreeFall();
        }
        #endregion
    }

}

