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

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class Player : IPlayer, ICollidable
    {
        //BSP trees for collision optimization
        //make a seperate class to hold all the objects --singleton
        //this class will be refactored in next sprint to make another class: State management
        // and movement management so it is not doing this much
        public PlayerStateMachine state { get; private set; }
        public PlayerMovement movement { get; private set; }
        private int playerIndex;
        private Sprite playerSprite;
        private int spriteDamageCounter;
        public PlayerAttack attack {get; private set;}
        public PlayerAttack starAttackOne {get; private set;}
        public PlayerAttack starAttackTwo {get; private set;}

        //health stuffs -- will be taken to another class connected to kirby in next sprint
        public int health = Constants.Kirby.MAX_HEALTH;
        public int lives = Constants.Kirby.MAX_LIVES;
        private bool invincible = false;
        private double timer = 0;

        //others
        private string oldState;
        public bool attackIsActive{get; private set; } = false;
        public bool CollisionActive { get; private set;} = true;
        public bool DEAD = false;
        public bool lifeLost = false;

        //collision stuffs

        //constructor
        public Player(Vector2 pos, int playerIndex)
        {
            this.playerIndex = playerIndex;
            state = new PlayerStateMachine(playerIndex);
            movement = new NormalPlayerMovement(pos);
            oldState = null;
            ObjectManager.Instance.RegisterDynamicObject(this);
            movement.ChangeKirbyLanded(false);
            UpdateTexture();
        }

        public string GetObjectType()
        {
            return Constants.CollisionObjectType.PLAYER;
        }
        //changes kiry's texture if he is in a different state than before
        //only called by Draw
        private void UpdateTexture()
        {
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = SpriteFactory.Instance.CreateSprite(state.GetSpriteParameters());
                oldState = state.GetStateString();
            } 
        }

        #region KirbyState
        public void ChangePose(KirbyPose pose)
        {
            state.ChangePose(pose);
        }
        public void ChangeMovement()
        {
            movement = new NormalPlayerMovement(movement.GetPosition());
        }
        public string GetKirbyPose()
        {
            return state.GetPose().ToString();
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
            state.ChangeType(KirbyType.Fire);
        }
        public void ChangeToSpark()
        {
            state.ChangeType(KirbyType.Spark);
        }
        public void ChangeToMouthful()
        {
            state.ChangeType(KirbyType.Mouthful);
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
            if(invincible){
                if(state.HasPowerUp())
                {
                    starAttackTwo = new PlayerAttack(this, "Star");
                    if(!state.IsCrouching())AttackAnimation();
                    movement.Attack(this);
                }
                if(!IsWithEnemy())ChangeToNormal();
                if(IsFloating()) movement = new NormalPlayerMovement(GetKirbyPosition());
                ChangePose(KirbyPose.Hurt);
                SoundManager.Play("kirbyhurt1");
                await Task.Delay(Constants.WaitTimes.DELAY_400);
                StopMoving();
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
            if (state.CanMove()){
                ChangePose(KirbyPose.Running);
            }
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run(state.IsLeft());
            if(state.CanMove()){
                ChangePose(KirbyPose.Running);
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
            if(!IsSliding() && attack != null){
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
                
                if(attack != null)// && attack.IsDone())
                {
                    attack.EndAttack();
                    attack = null;
                }
            }
        }
        public void EndCrouch()
        {
            //if(state.IsCrouching()){
                EndSlide(); //if sliding changes to standin
                ChangeMovement(); //change to normal
                StopMoving(); //set vel to 0 and standing
                if(attack != null)// && attack.IsDone())
                {
                    attack.EndAttack();
                    attack = null;
                }
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
            if(!state.IsFloating()) ChangePose(KirbyPose.Standing);
            if(IsWithEnemy())ChangeToNormal();
        }
        public void Attack()
        {
            //mouthful exhale -- spits out star
            if(IsWithEnemy() && state.ShortAttack()){
                starAttackOne = new PlayerAttack(this, "Star");
                if(!state.IsCrouching())AttackAnimation();
                movement.Attack(this);
                //ChangeAttackBool(true);
            } else if (attack == null && state.ShortAttack()) { //slide beam float exhale 
                attack = new PlayerAttack(this, AttackType());

                if(!state.IsCrouching())AttackAnimation();
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
            if(attack != null && attack.IsDone())
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
        public void SwallowEnemy()
        {
            SmallWait();
            SoundManager.Play("catch");
            StopAttacking();
            ChangeToMouthful();
        }
        private void SwallowAnimation()
        {
            state.ChangePose(KirbyPose.Swallow);
        }
        private void EndSwallow()
        {
            SwallowAnimation();
            ChangePose(KirbyPose.Standing);
            ChangeToNormal();
        }
        #endregion

        #region MoveKirby
        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            movement.MovePlayer(this, gameTime);
            EndInvinciblility(gameTime);
            playerSprite.Update();
            spriteDamageCounter++;
            GetHitBox();
            if(attack != null || starAttackOne != null || starAttackTwo != null){
                attack?.Update(gameTime, this);
                starAttackOne?.Update(gameTime, this);
                starAttackTwo?.Update(gameTime, this);
            }
            if(lifeLost)
            {
                Death();
                lifeLost = false;
            }
        }

        Color invincibleColor = new Color(255, 255, 0, 127);
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateTexture();
            
            if(invincible){
                playerSprite.Draw(movement.GetPosition(), spriteBatch);
                // Draw a second, tinted, translucent copy of the sprite on top of itself to tint it brigher. Use of unpremultiplied color in a premultiplied environment to do this. Stupid
                if (spriteDamageCounter % 8 < 4)
                {
                    playerSprite.Draw(movement.GetPosition(), spriteBatch, invincibleColor);
                }
                
            } else {
                playerSprite.Draw(movement.GetPosition(), spriteBatch);
            }

            if(attack != null || starAttackOne != null || starAttackTwo != null){
                attack?.Draw(spriteBatch, this);
                starAttackOne?.Draw(spriteBatch, this);
                starAttackTwo?.Draw(spriteBatch, this);
            }
        }
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

        public  Vector2 GetPosition()
        {
            return GetKirbyPosition();
        }
        #endregion

        #region Collisions
        //kirby collides with the top of a block
        public void BottomCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionBlock(intersection);
        }
        //kirby collides with the right side of a block
        public void RightCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromRightCollisionBlock(intersection);
        }
        //kirby collides with the left side of a block
        public void LeftCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromLeftCollisionBlock(intersection);
        }
        //kirby collides with the bottom of a plataform
        public void BottomCollisionWithPlatform(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionPlatform(intersection, state);
        }
        //kirby collision with air so he falls
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            //checking if kirby should be falling 
            if (!state.IsInAir() || state.ShouldFallThroughAirTile())
           {
                movement.ChangeKirbyLanded(false);
           }
           movement.ChangeKirbyLanded(false);
        }

        //slope collision
        public void CollisionWithGentle1LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
        }
        //slope collision
        public void CollisionWithGentle2LeftSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
        }
        //slope collision
        public void CollisionWithSteepLeftSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_LEFT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_LEFT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
        }
        //slope collision
        public void CollisionWithGentle1RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE1_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE1_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
        }
        //slope collision
        public void CollisionWithGentle2RightSlope(Tile tile)
        {
            float slope = Constants.Collision.GENTLE2_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.GENTLE2_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
        }
        //slope collision
        public void CollisionWithSteepRightSlope(Tile tile)
        {
            float slope = Constants.Collision.STEEP_SLOPE_RIGHT_M;
            float yIntercept = Constants.Collision.STEEP_SLOPE_RIGHT_YINTERCEPT;
            movement.AdjustOnSlopeCollision(state, tile, slope, yIntercept);
        }
        #endregion
    }

}

