using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using System;
using System.Runtime.CompilerServices;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class Player : IPlayer, ICollidable
    {
        //no axis aligned collison?? 
        //BSP trees for collision optimization
        //make a seperate class to hold all the objects --singleton
        //this class will be refactored in next sprint to make another class: State management
        // and movement management so it is not doing this much
        private PlayerStateMachine state;
        private PlayerMovement movement;
        private Sprite playerSprite ;
        private PlayerAttack attack;


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
        }
        public Sprite PlayerSprite
        {
            //change it so it cannot be changed by cgame aka delete this

            set{playerSprite = value;}
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
            return GetKirbyPose().Equals(KirbyPose.FreeFall);
        }
        public String AttackType()
        {
            if(IsFloating()&& !IsFalling()){
                return "Puff";
            } else if (state.IsCrouching()){
                return "Slide";
            }else {
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
        //health will be moved to another class
        public void Death() //does nothing this sprint
        {
            //state.ChangeType(KirbyType.Dead);
        }
        public void DecreaseHealth()
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
        public async void TakeDamageAnimation()
        {
            ChangePose(KirbyPose.Hurt);
            await Task.Delay(Constants.Physics.DELAY);
            StopMoving();
        }
        public void EndInvinciblility(GameTime gameTime)
        {
            if(invincible){
                timer += gameTime.ElapsedGameTime.TotalSeconds; 
                if(timer > 5){
                    invincible = false;
                    timer = 0;
                }
            }
        }
        public void TakeDamage()
        {
            invincible = true;
            DecreaseHealth();
            TakeDamageAnimation();
            movement.ReceiveDamage(state.IsLeft());
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
                //does nothing: sprint 3 will have a controller refactor 
                //that will allow for a use of this spot
            }
        }
        #endregion
        public void Float()
        {
            //1 start floating
            //2 go up 
            //3 float again if its fallign
            //crouching and sliding cannot be overwritten by float 
            if (IsFloating() && !IsFalling()){
                movement.Jump(state.IsLeft()); 
                ChangePose(KirbyPose.FloatingRising);
            } else if (state.CanFloat()){
                movement.StartFloating(this);
                movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.FloatingRising);
                //change this to flowting geenral movement
                return;
            }
        }

        #region crouch
        public void Crouch()
        {
            if(state.CanCrouch() && !state.IsCrouching()){ //crouch does not overwrite jump and floating
                ChangePose(KirbyPose.Crouching);
                movement = new CrouchingMovement(movement.GetPosition());
            } 
        }
        public void Slide()
        {
            if(!state.IsSliding()){
                ChangePose(KirbyPose.Sliding);
                movement.Slide(state.IsLeft());
                //await Task.Delay(Constants.Physics.DELAY);
            }
        }
        public void EndSlide()
        {
            if(state.IsSliding()){
                ChangePose(KirbyPose.Crouching);
                movement.StopMovement();
                StopAttacking();
                ChangeAttackBool(false);
                if(attack != null) attack.EndAttack();
                //await Task.Delay(Constants.Physics.DELAY);
            }
        }
        public void EndCrouch()
        {
            if(state.IsCrouching()){
                if(attack != null) attack.EndAttack();
                EndSlide();
                ChangeAttackBool(false);
                ChangeMovement();
                StopMoving(); 
            } 
        }
        #endregion
        
        #endregion //movement region
        #region Attack
        public void Attack()
        {
            // if(attack != null && !attackIsActive){
            //     attack.EndAttack(this);
            //     StopAttacking();
            //     attack = null;
            // }
            //start a new attack if another one isnt happening
            if(attack == null){
                attack = new PlayerAttack(this, AttackType());
                movement.Attack(this);
            }
            ChangeAttackBool(true);
        }
        public void AttackPressed()
        {
            if(!attackIsActive){
                attack = new PlayerAttack(this, AttackType());
                movement.Attack(this);
                ChangeAttackBool(true);
            }
            //ChangeAttackBool(true);

        }
        public void StopAttacking()
        {
            // if(!IsFloating()){
            //     StopMoving();
            // }
            if(attack != null && attack.IsDone())
            {
                StopMoving();
                ChangeAttackBool(false);
                attack.EndAttack();
                attack = null;
            }
        }

        #endregion

        #region Mouthful
        public void SwallowEnemy()
        {
            ChangeToMouthful();
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
            if(attackIsActive){
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
            if(attackIsActive){
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
            if (state.ShouldFallThroughTile())
            {
                movement.ChangeKirbyLanded(false);
                movement.Fall();
            }
        }

        #endregion
    }

}

