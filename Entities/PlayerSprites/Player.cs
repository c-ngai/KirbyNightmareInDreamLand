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

        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new PlayerMovement();
            factory = SpriteFactory.Instance;
            position = pos;
        }

         public Vector2 Position
        {
            get { return position; }    // Getter returns the current position
            set { position = value; }   // Setter updates the position
        }
        public void UpdateTexture()
        {
            playerSprite = factory.createSprite(state.GetSpriteParameters());
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
            state.ChangePose(KirbyPose.Attacking);
            UpdateTexture();
        }

        #region Movement
        public void MoveLeft()
        {
            state.SetDirectionLeft();
            state.ChangePose(KirbyPose.Walking);
            movement.MovePlayer(this);
            UpdateTexture();
        }

        public void MoveRight()
        {
            movement.MoveRight();
            movement.MovePlayer(this);
            state.ChangePose(KirbyPose.Walking);
            state.SetDirectionRight();
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

