using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Transactions;

namespace MasterGame
{
    public class Player : IPlayer
    {
        //this class will be refactored in next sprint to make another class: State management
        // and movement management so it is not doing this much
        // TODO: Is it possible to make this a public property so commands can access it?
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
        private bool attackIsActive = false;

        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new NormalPlayerMovement(pos);
            oldState = state.GetStateString();
        }
        public Sprite PlayerSprite
        {
            set{playerSprite = value;}
        }

        //changes kiry's texture if he is in a different state than before
        //only called by Draw
        public void UpdateTexture()
        {
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = SpriteFactory.Instance.createSprite(state.GetSpriteParameters());
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
            return movement.floating;
        }
        public bool NotAttacking() //checks if kirby is not attacking
        {
            return !GetKirbyPose().Equals("Attack") && !GetKirbyPose().Equals("Inhaling");
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
        #endregion
        #region direction
        public void SetDirectionLeft()
        {
            if(NotAttacking()){
                state.SetDirectionLeft();
            }
        }
        public void SetDirectionRight()
        {
            if(NotAttacking()){
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
            health --;
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
        }
        public void TakeDamage()
        {
            TakeDamageAnimation();
            movement.ReceiveDamage(state.IsLeft());
            invincible = true;
            //DecreaseHealth();
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
        public void Attack()
        {
            ChangeAttackBool(true);
            attack = new PlayerAttack(this);
            movement.Attack(this);
        }
        #endregion

        #region Movement
        public void MoveLeft()
        {   
            SetDirectionLeft();
            movement.Walk(state.IsLeft());
            //walk connot override walking, jumping, floating, crouching, and attack
            if(!movement.jumping && !movement.crouching&& !movement.floating && NotAttacking()){
                ChangePose(KirbyPose.Walking);
            }
        }

        public void MoveRight()
        {
            SetDirectionRight();
            movement.Walk(state.IsLeft());
            //walk connot override walking, jumping, floating, crouching, and attack
            if(!movement.jumping&& !movement.crouching && !movement.floating && NotAttacking()){
                ChangePose(KirbyPose.Walking);
            }
        }
        public void StopMoving() 
        {
            movement.StopMovement();
            if(!movement.jumping && !movement.floating)
            {
                ChangePose(KirbyPose.Standing);
            }
        }
        #region running
        public void RunLeft()
        {
            SetDirectionLeft();
            movement.Run(state.IsLeft());
           if(!movement.jumping&& !movement.crouching && !movement.floating && NotAttacking()){
                ChangePose(KirbyPose.Running);
            }
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run(state.IsLeft());
            if(!movement.jumping&& !movement.crouching && !movement.floating && NotAttacking()){
                ChangePose(KirbyPose.Running);
            }
        }
        #endregion

        #region jumping
        public void Jump()
        {
            if(!movement.crouching && !movement.floating && !movement.jumping){ //not floating, not jumping
                movement = new JumpMovement(movement.GetPosition());
                ChangePose(KirbyPose.JumpRising);
            }else if (movement.jumping && !movement.floating){ //if jumping and x is pressed again
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
            //crouching and sliding cannot be overwritten by float 
            if(!movement.crouching && !movement.floating){
                movement.StartFloating(this);
                movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.FloatingRising);
            } 
            if(!movement.crouching){ //if float is up arrow is pressed again it goes up
                ChangePose(KirbyPose.FloatingRising);
                movement.Jump(state.IsLeft()); //change this to flowting geenral movement
            }
        }

        #region crouch
        public void Crouch()
        {
            if(!movement.jumping && !movement.floating){ //crouch does not overwrite jump and floating
                ChangePose(KirbyPose.Crouching);
                movement = new CrouchingMovement(movement.GetPosition());
            }
        }
        public void EndCrouch()
        {
            if(!movement.jumping && !movement.floating){
                StopMoving();
                ChangeMovement();
            }
        }
        #endregion
        
        public async void Slide()
        {
            if(movement.crouching){
                ChangePose(KirbyPose.Sliding);
                movement.Slide(state.IsLeft());
                await Task.Delay(Constants.Physics.DELAY);
            }
        }
        #endregion //movement region


        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            movement.MovePlayer(this, gameTime);
            EndInvinciblility(gameTime);
            playerSprite.Update();
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
    }

}

