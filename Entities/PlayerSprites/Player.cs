using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class Player{

        public PlayerStateMachine state;
        public static int maxHealth = 6;
        private int health = maxHealth;
        private int lives = 5;
        private Vector2 position;

        public Player()
        {
            state = new PlayerStateMachine();
        }

        public void SetDirection()
        {

        }

        public void TakeDamage()
        {

        }
        public void Attack()
        {

        }

        public void Draw()
        {

        }

        // makes state changes by calling other player methods, calls state.Update(), and finally calls Draw last?
        public void Update()
        {

        }
    }
}

