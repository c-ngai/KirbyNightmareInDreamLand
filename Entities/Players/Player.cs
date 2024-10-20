using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using KirbyNightmareInDreamLand.Levels;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class Player : IPlayer, ICollidable
    {
        //BSP trees for collision optimization
        //make a seperate class to hold all the objects --singleton
        //this class will be refactored in next sprint to make another class: State management
        // and movement management so it is not doing this much
        private PlayerStateMachine state;
        private PlayerMovement movement;
        private Sprite playerSprite ;
        public PlayerAttack attack {get; private set;}

        //health stuffs -- will be taken to another class connected to kirby in next sprint
        private int health = Constants.Kirby.MAX_HEALTH;
        private int lives = Constants.Kirby.MAX_LIVES;
        private bool invincible = false;
        private double timer = 0;

        //others
        private string oldState;
        public bool attackIsActive{get; private set; } = false;
        public bool CollisionActive { get; private set;} = true;

        //collision stuffs

        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new NormalPlayerMovement(pos);
            oldState = state.GetStateString();
            ObjectManager.Instance.RegisterDynamicObject(this);
            playerSprite = SpriteFactory.Instance.CreateSprite("kirby_normal_standing_right");
            movement.ChangeKirbyLanded(false);
        }

        public string GetObjectType()
        {
            return "Player";
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
        public String GetCollisionType()
        {
            return "Player";
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
        //health will be moved to another class
        public void Death() //does nothing this sprint
        {
            //state.ChangeType(KirbyType.Dead);
        }
        private void DecreaseHealth()
        {
            if(timer == 0)
            {
                health --;
            }
            if(health == 0)
            {
                health = Constants.Kirby.MAX_HEALTH;
                lives--;
            }
            if(lives == 0)
            {
                Death();
            }
        }
        //calls method to drecease health & changes kirby pose
        private async void TakeDamageAnimation()
        {
            if(invincible){
                if(!IsWithEnemy())ChangeToNormal();
                if(IsFloating()) movement = new NormalPlayerMovement(GetKirbyPosition());
                ChangePose(KirbyPose.Hurt);
                await Task.Delay(Constants.Physics.DELAY);
                StopMoving();
                invincible = true;
            }
        }
        private void EndInvinciblility(GameTime gameTime)
        {
            if(invincible){
                timer += gameTime.ElapsedGameTime.TotalSeconds; 
                if(timer > 5){
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
                TakeDamageAnimation();
                DecreaseHealth();
                movement.ReceiveDamage(intersection);
            }
            
        }
        public void FillHealth()
        {
            health = Constants.Kirby.MAX_HEALTH;
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
           if(state.CanMove()){
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
            }else if (state.IsJumping() && !state.IsFloating()){ //if jumping and x is pressed again
                //Float();
                movement.Jump(state.IsLeft());
            } else {
                //does nothing
            }
        }
        #endregion
        private async void StartFloating()
        {
            ChangePose(KirbyPose.FloatingStart);
            await Task.Delay(Constants.Physics.DELAY*2);
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
            if(state.IsCrouching()){
                EndSlide(); //if sliding changes to standin
                ChangeMovement(); //change to normal
                StopMoving(); //set vel to 0 and standing
                if(attack != null)// && attack.IsDone())
                {
                    attack.EndAttack();
                    attack = null;
                }
            } 
        }
        #endregion
        
        #endregion //movement region
        #region Attack

        private async void AttackAnimation()
        {
            //beam float exhale mouthful exhale
            ChangePose(KirbyPose.Attacking);
            if(GetKirbyType().Equals("Beam")){
                await Task.Delay(500);
                //ChangeAttackBool(false);
                attack = null;
            } else {  //float and mouthful exhale
                await Task.Delay(200);
            }
            if(!state.IsFloating()) ChangePose(KirbyPose.Standing);
            if(IsWithEnemy())ChangeToNormal();
        }
        public void Attack()
        {
            //slide beam float exhale mouthful exhale
            if(attack == null && state.ShortAttack()){
                attack = new PlayerAttack(this, AttackType());
                if(!state.IsCrouching())AttackAnimation();
                movement.Attack(this);
                //ChangeAttackBool(true);
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
        public void StopAttacking()
        {
            if(attack != null && attack.IsDone())
            {
                StopMoving();
                //ChangeAttackBool(false);
                attack.EndAttack();
                attack = null;
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
            await Task.Delay(1000);
        }
        public void SwallowEnemy()
        {
            StopAttacking();
            SmallWait();
            ChangeToMouthful();
        }
        private async void SwallowAnimation()
        {
            state.ChangePose(KirbyPose.Swallow);
            await Task.Delay(1000);
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
            GetHitBox();
            if(attack != null){
                attack.Update(gameTime, this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateTexture();
            
            if(invincible){
                playerSprite.DamageDraw(movement.GetPosition(), spriteBatch);
            } else {
                playerSprite.Draw(movement.GetPosition(), spriteBatch);
            }

            if(attack != null){
                attack.Draw(spriteBatch, this);
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
        #endregion

        #region Collisions
        public void BottomCollisionWithBlock(Rectangle intersection)
        {
            movement.ChangeKirbyLanded(true);
            movement.AdjustFromBottomCollisionBlock(intersection);
        }
        public void RightCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromRightCollisionBlock(intersection);
        }
        public void LeftCollisionWithBlock(Rectangle intersection)
        {
            movement.AdjustFromLeftCollisionBlock(intersection);
        }
        public void BottomCollisionWithPlatform(Rectangle intersection)
        {
            movement.AdjustFromBottomCollisionPlatform(intersection);
        }
        public void BottomCollisionWithAir(Rectangle intersection)
        {
            if (!state.IsInAir() || state.ShouldFallThroughAirTile())
           {
                movement.Fall();
                movement.ChangeKirbyLanded(false);
           }
        }

        public void CollisionWithGentle1LeftSlope(Tile tile)
        {
            movement.AdjustGentle1SlopeLeftCollision(tile);
        }

        public void CollisionWithGentle2LeftSlope(Tile tile)
        {
            movement.AdjustGentle2SlopeLeftCollision(tile);
        }

        public void CollisionWithSteepLeftSlope(Tile tile)
        {
            movement.AdjustSteepSlopeLeftCollision(tile);
        }

        public void CollisionWithGentle1RightSlope(Tile tile)
        {
            movement.AdjustGentle1SlopeRightCollision(tile);
        }

        public void CollisionWithGentle2RightSlope(Tile tile)
        {
            movement.AdjustGentle2SlopeRightCollision(tile);
        }

        public void CollisionWithSteepRightSlope(Tile tile)
        {
            movement.AdjustSteepSlopeRightCollision(tile);
        }
        #endregion
    }

}

