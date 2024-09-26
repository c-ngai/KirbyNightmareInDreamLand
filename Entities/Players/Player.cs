using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MasterGame
{
    public class Player : IPlayer
    {
        // TODO: Is it possible to make this a public property so commands can access it?
        private PlayerStateMachine state;
        private PlayerMovement movement;
        private SpriteFactory factory;
        private Sprite playerSprite ;

        //health stuffs
        private int health = Constants.Kirby.MAX_HEALTH;
        private int lives = Constants.Kirby.MAX_LIVES;


        //private Vector2 position;
        private string oldState;


        //todo:
        //projectiles should be its own objs (obv)
        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new NormalPlayerMovement(pos);
            //grab the factory whenever you need it -- take it out of here
            //if factory gets rebuilt youre fucked
            //ask in office hours
            factory = SpriteFactory.Instance;
            oldState = state.GetStateString();
        }
        public Sprite PlayerSprite
        {
            set{playerSprite = value;}
        }

        public void UpdateTexture()
        {
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = factory.createSprite(state.GetSpriteParameters());
                oldState = state.GetStateString();
            } 
        }
        //have draw ask if it has the right sprite and call update texture from there
        //draw if sprite is not good -- fix it and everyone else is freed from that job
        //have parent class check if the sprite is right
        public void ChangePose(KirbyPose pose)
        {
            state.ChangePose(pose);
            UpdateTexture();
        }
        public string GetPose()
        {
            return state.GetPose().ToString();
        }
        public string GetKirbyType()
        {
            return state.GetType().ToString();
        }
        public void ChangeMovement()
        {
            if(this.GetKirbyType().Equals("Normal"))
            {
                movement = new NormalPlayerMovement(movement.GetPosition());
            } else {
                movement = new PowerupMovement(movement.GetPosition());
            }
        }

        #region Power-Up
        public void ChangeToNormal()
        {
            state.ChangeType(KirbyType.Normal);
        }
        public void ChangeToBeam()
        {
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
            state.SetDirectionLeft();
        }
        public void SetDirectionRight()
        {
            state.SetDirectionRight();
        }
        #endregion

        #region health
        public void Death()
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
        public void TakeDamage()
        {
            ChangePose(KirbyPose.Hurt);
            movement.ReceiveDamage(state.IsLeft());
            //DecreaseHealth();
        }
        //calls state machine to attack
        public void Attack()
        {
            if(movement.floating){
                movement = new NormalPlayerMovement(movement.GetPosition());
                ChangePose(KirbyPose.JumpFalling);
            } else {
                ChangePose(KirbyPose.Attacking);
                movement.Attack(this);
            }
            
        }
        #endregion

        #region Movement
        public void MoveLeft()
        {   
            SetDirectionLeft();
            movement.Walk(state.IsLeft());
            if(!movement.jumping && !movement.crouching&& !movement.floating){
                ChangePose(KirbyPose.Walking);
            }
        }

        public void MoveRight()
        {
            SetDirectionRight();
            movement.Walk(state.IsLeft());
            if(!movement.jumping&& !movement.crouching && !movement.floating){
                ChangePose(KirbyPose.Walking);
            }
        }

        public void StopMoving()
        {
            if(!movement.jumping){
            ChangePose(KirbyPose.Standing);
            movement.StopMovement();}
        }
        public void DontMove()
        {
            movement.StopMovement();
        }

        #region running
        public void RunLeft()
        {
            SetDirectionLeft();
            movement.Run(state.IsLeft());
           if(!movement.jumping&& !movement.crouching && !movement.floating){
                ChangePose(KirbyPose.Running);
            }
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run(state.IsLeft());
            if(!movement.jumping&& !movement.crouching && !movement.floating){
                ChangePose(KirbyPose.Running);
            }
        }
        #endregion

        #region jumping
        public void JumpFloat()
        {
            if(movement.jumping)
            {
                movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.Floating);
            }
        }
        public void JumpFall()
        {
            ChangePose(KirbyPose.JumpFalling);
        }
        public void JumpY()
        {
            if(!movement.floating ){//&& !movement.jumping){ //not floating, not jumping
                movement = new JumpMovement(movement.GetPosition());
                //movement.Jump(state.IsLeft());
                ChangePose(KirbyPose.JumpRising);
            }  else if(movement.jumping && !movement.floating){ //if jumping and x is pressed again
                movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.Floating);
            } else { // floating and jump is pressed
                //movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.Floating);
            }
        }
        public void JumpXY()
        {
            if(!movement.floating){
                movement = new JumpMovement(movement.GetPosition());
                //JumpFloat();
                ChangePose(KirbyPose.JumpRising);  
                //movement.JumpXY(state.IsLeft());
            }
            
        }
        #endregion
        public void Float()
        {
            //crouching and sliding cannot be overwritten by float 
            if(!movement.crouching || state.GetPose()!= KirbyPose.Sliding){
                movement = new FloatingMovement(movement.GetPosition());
                ChangePose(KirbyPose.Floating);
            } 
            if(state.GetPose()== KirbyPose.Floating)
            {
                movement.Jump(state.IsLeft()); //change this to flowting geenral movement
            }
        }

        #region crouch
        public void Crouch()
        {
            if(!movement.jumping && !movement.floating){
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
        
        public void Slide()
        {
            ChangePose(KirbyPose.Sliding);
            movement.Slide(this, state.IsLeft());
        }
        public void Inhale()
        {
            ChangePose(KirbyPose.AbsorbingAir);
            movement.StopMovement();
        }
        #endregion //movement region

        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            movement.MovePlayer(this, gameTime);
            playerSprite.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateTexture();
            playerSprite.Draw(movement.GetPosition(), spriteBatch);
        }   
    }
}

