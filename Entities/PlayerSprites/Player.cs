using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class Player{
        public PlayerStateMachine state;
        public static int maxHealth = 6;
        private int health = maxHealth;
        private int lives = 5;
        private Vector2 position;

        //constructor
        public Player(Vector2 pos)
        {
            state = new PlayerStateMachine();
            position = pos;
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
            state.TakeDamage();
        }
        //calls state machine to attack
        public void Attack()
        {
            state.Attack();
        }

        #region Movement
        public void MoveLeft()
        {
            state.SetDirectionLeft();
            PlayerMovement.MovePlayer();
        }

        public void MoveRight()
        {
            state.SetDirectionRight();
            PlayerMovement.MovePlayer();
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
            state.Update();
            PlayerMovement.Update();
        }
        public void Draw()
        {
            
        }
    }
}

