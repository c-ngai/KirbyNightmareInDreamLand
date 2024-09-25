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


        private Vector2 position;
        private string oldState;


        //todo:
        //projectiles should be its own objs (obv)
        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new NormalPlayerMovement();
            //grab the factory whenever you need it -- take it out of here
            //if factory gets rebuilt youre fucked

            factory = SpriteFactory.Instance;
            oldState = state.GetStateString();
            position = pos;
        }

        #region Position
        //mposition should be an ask not 
        public float PositionX
        {
            get { return position.X; }    // Getter returns the current position
            set { position.X = value; }   // Setter updates the position
        }
        public float PositionY
        {
            get { return position.Y; }    // Getter returns the current position
            set { position.Y = value; }   // Setter updates the position
        }
    #endregion
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
        public string GetType()
        {
            return state.GetType().ToString();
        }
        public void ChangeMovement()
        {
            if(this.GetType().Equals("Normal"))
            {
                movement = new NormalPlayerMovement();
            } else {
                movement = new PowerupMovement();
            }
        }

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
            ChangePose(KirbyPose.Attacking);
            movement.Attack(this);
        }
        #endregion

        #region Movement
        public void MoveLeft()
        {   
            SetDirectionLeft();
            movement.Walk(state.IsLeft());
            if(!movement.jumping && !movement.crouching){
                ChangePose(KirbyPose.Walking);
            }
        }

        public void MoveRight()
        {
            SetDirectionRight();
            movement.Walk(state.IsLeft());
            if(!movement.jumping&& !movement.crouching){
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
            ChangePose(KirbyPose.Running);
            SetDirectionLeft();
            movement.Run(state.IsLeft());
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run(state.IsLeft());
            state.ChangePose(KirbyPose.Running);
        }
        #endregion

        #region jumping
        public void JumpFloat()
        {
            if(movement.jumping)
            {
                movement = new FloatingMovement();
                ChangePose(KirbyPose.Floating);
            }
        }
        public void JumpFall()
        {
            ChangePose(KirbyPose.JumpFalling);
        }
        public void JumpY()
        {
            //JumpFloat();
            movement = new JumpMovement();
            movement.Jump();
            ChangePose(KirbyPose.JumpRising);
            //movement.JumpY();

        }
        public void JumpXY()
        {
            movement = new JumpMovement();
            //JumpFloat();
            ChangePose(KirbyPose.JumpRising);  
            //movement.JumpXY(state.IsLeft());
        }
        #endregion
        public void Float()
        {
            //crouching and sliding cannot be overwritten by float 
            if(!movement.crouching || state.GetPose()!= KirbyPose.Sliding){
                movement = new FloatingMovement();
                ChangePose(KirbyPose.Floating);
            } 
            if(state.GetPose()== KirbyPose.Floating)
            {
                //movement.Jump(state.IsLeft()); //change this to flowting geenral movement
            }
        }

        #region crouch
        public void Crouch()
        {
            if(!movement.jumping && !movement.floating){
                ChangePose(KirbyPose.Crouching);
                movement = new CrouchingMovement();
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
            playerSprite.Draw(position, spriteBatch);
        }   
    }
}

