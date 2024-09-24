using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace MasterGame
{
    public class Player : IPlayer
    {
        //
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
        //take out state machine instance
        //projectiles should be its own objs (obv)
        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new NormalPlayerMovement(ref state);
            factory = SpriteFactory.Instance;
            oldState = state.GetStateString();
            position = pos;
        }

        #region Position
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
            System.Console.WriteLine("texture" + state.GetStateString());
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = factory.createSprite(state.GetSpriteParameters());
                oldState = state.GetStateString();
            } 
        }
         public void ChangePose(KirbyPose pose)
        {
            state.ChangePose(pose);
            UpdateTexture();
        }

        #region direction
        public void SetDirectionLeft()
        {
            state.SetDirectionLeft();
            UpdateTexture();
        }
        public void SetDirectionRight()
        {
            state.SetDirectionRight();
            UpdateTexture();
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
            movement.ReceiveDamage();
            DecreaseHealth();
        }
        //calls state machine to attack
        public void Attack()
        {
            movement.Attack();
            UpdateTexture();
        }
        #endregion

        #region Movement
        public void MoveLeft()
        {   
            ChangePose(KirbyPose.Walking);
            SetDirectionLeft();
            UpdateTexture();
        }

        public void MoveRight()
        {
            SetDirectionRight();
            if(!state.GetPose().ToString().Equals("JumpRising")){
                movement.Walk();
                ChangePose(KirbyPose.Walking);
            }
        }

        public void StopMoving()
        {
            ChangePose(KirbyPose.Standing);
            movement.StopMovement();
        }

        #region running
        public void RunLeft()
        {
            ChangePose(KirbyPose.Running);
            SetDirectionLeft();
            movement.Run();
        }
        public void RunRight()
        {
            SetDirectionRight();
            movement.Run();
            state.ChangePose(KirbyPose.Running);
        }
        #endregion
        public void JumpFall()
        {
            state.ChangePose(KirbyPose.JumpFalling);
            UpdateTexture();
        }
        public void JumpY()
        {
            state.ChangePose(KirbyPose.JumpRising);  
            UpdateTexture();
            movement.JumpY();

        }

        public void JumpXY()
        {
            
        }
        public void Float()
        {
            //crouching and sliding cannot be overwritten by float 
            if(state.GetPose()!= KirbyPose.Crouching || state.GetPose()!= KirbyPose.Sliding){
                movement = new FloatingMovement(ref state);
                state.ChangePose(KirbyPose.Floating);
            } 
            if(state.GetPose()== KirbyPose.Floating)
            {
                movement.Walk(); //change this to flowting geenral movement
            }

        }

        public void Crouch()
        {
            state.ChangePose(KirbyPose.Crouching);
        }
        
        #endregion

        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update(GameTime gameTime)
        {
            movement.MovePlayer(this, gameTime);
            playerSprite.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(position, spriteBatch);
        }   
    }
}

