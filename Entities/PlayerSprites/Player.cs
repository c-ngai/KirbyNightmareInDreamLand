using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace MasterGame
{
    public class Player : IPlayer
    {
        private PlayerStateMachine state;
        private PlayerMovement movement;
        private SpriteFactory factory;
        private Sprite playerSprite ;
        private int health = Constants.Kirby.MAX_HEALTH;
        private int lives = 5;
        private Vector2 position;
        private string oldState;


        //todo:
        // make kirby and game instance fields private, and take out state machine instance
        //projectiles should be its own objs (obv)
        //constructor
        public Player(Vector2 pos)
        {
            state = PlayerStateMachine.Instance;
            movement = new NormalPlayerMovement();
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
            if(!state.GetStateString().Equals(oldState)){
                playerSprite = factory.createSprite(state.GetSpriteParameters());
                oldState = state.GetStateString();
            } 
        }

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
        //calls state machine to drecease health
        public void TakeDamage()
        {
            state.ChangePose(KirbyPose.Hurt);
            UpdateTexture();
        }
        //calls state machine to attack
        public void Attack()
        {
            movement.Attack();
            UpdateTexture();
        }

        #region Movement
        public void MoveLeft()
        {
            state.ChangePose(KirbyPose.Walking);
            state.SetDirectionLeft();
            movement.Walk();
            UpdateTexture();
        }

        public void MoveRight()
        {
            state.SetDirectionRight();
            if(movement.floating == true)
            {
                movement.Walk();
            } else {
                movement.Walk();
                state.ChangePose(KirbyPose.Walking);
            }
            UpdateTexture();
        }

        public void StopMoving()
        {
            state.ChangePose(KirbyPose.Standing);
            movement.StopMovement();
            UpdateTexture();
        }

        #region running
        public void RunLeft()
        {
            state.SetDirectionLeft();
            movement.Run();
            state.ChangePose(KirbyPose.Running);
        }
        public void RunRight()
        {
            state.SetDirectionRight();
            movement.Run();
            state.ChangePose(KirbyPose.Running);
        }
        #endregion
        public void Float()
        {
            //crouching and sliding cannot be overwritten by float 
            if(state.GetPose()!= KirbyPose.Crouching || state.GetPose()!= KirbyPose.Sliding){
                movement = new FloatingMovement();
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
        public void Update()
        {
            movement.MovePlayer(this);
            playerSprite.Update();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(position, spriteBatch);
        }   
    }
}

