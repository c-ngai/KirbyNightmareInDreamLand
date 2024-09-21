using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.InteropServices;

namespace MasterGame
{
    public class Player : IPlayer
    {
        public PlayerStateMachine state;
        public PlayerMovement movement;
        public SpriteFactory factory;
        public Sprite playerSprite ;
        public static int maxHealth = 6;
        private int health = maxHealth;
        private int lives = 5;
        public Vector2 position;
        public string oldState;

        //constructor
        public Player(Vector2 pos)
        {
            state = PlayerStateMachine.Instance;
            movement = new NormalPlayerMovement();
            factory = SpriteFactory.Instance;
            oldState = state.GetStateString();
            position = pos;
        }

        public Vector2 Position
        {
            get { return position; }    // Getter returns the current position
            set { position = value; }   // Setter updates the position
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
            movement.MoveLeft();
            movement.MovePlayer(this);
            state.ChangePose(KirbyPose.Walking);
            state.SetDirectionRight();
            UpdateTexture();
        }

        public void MoveRight()
        {
            movement.Walk();
            movement.MovePlayer(this);
            state.ChangePose(KirbyPose.Walking);
            state.SetDirectionRight();
            UpdateTexture();
        }

        public void StopMoving()
        {
            movement.StopMoving();
            state.ChangePose(KirbyPose.Standing);
            UpdateTexture();
        }

        public void RunLeft()
        {

        }
        public void RunRight()
        {

        }
        
        #endregion
        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update()
        {
            playerSprite.Update();
            movement.MovePlayer(this);
        }
        public void Draw()
        {
            playerSprite.Draw(position);
        }   
    }
}

