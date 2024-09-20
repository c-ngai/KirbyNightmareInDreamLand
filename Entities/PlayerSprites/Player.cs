using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace MasterGame
{
    public class Player : IPlayer
    {
        public PlayerStateMachine state;
        public PlayerMovement movement;
        public static int maxHealth = 6;
        private int health = maxHealth;
        private int lives = 5;
        public Vector2 position;

        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            movement = new PlayerMovement();
            position = pos;
        }

        public void setDirection()
        {

        }
        public float GetXPos()
        {
            return position.X;
        }
        public float GetYPos()
        {
            return position.Y;
        }

        public void SetXPos(float newX)
        {
            position.X = newX;
        }
        public void SetYPos(float newY)
        {
            position.Y = newY;
        }

        public void SetDirectionLeft()
        {
            state.SetDirectionLeft();
        }
        public void SetDirectionRight()
        {
            state.SetDirectionRight();
        }
        //calls state machine to drecease health
        public void TakeDamage()
        {
            state.ChangePose(KirbyPose.Hurt);
        }
        //calls state machine to attack
        public void Attack()
        {
            state.ChangePose(KirbyPose.Attacking);
        }

        #region Movement
        public void MoveLeft()
        {
            state.SetDirectionLeft();
            movement.MovePlayer(this);
        }

        public void MoveRight()
        {
            state.SetDirectionRight();
            movement.MovePlayer(this);
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
            //state.Update();
            movement.MovePlayer(this);
        }
        public void Draw()
        {
            
        }
    }
}

