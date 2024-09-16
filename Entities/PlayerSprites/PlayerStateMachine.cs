using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class PlayerStateMachine{
        // default is facing right
        private bool facingLeft = false;
        // double check these, I Googled
        public static int maxHealth = 6;
        private int health = maxHealth;
        private int lives = 5;
        // determine default starting position to initialize this to
        private Vector2 position;
        // might want to change the access level of this to limit it to just get and not set
        public enum KirbyPose { Standing, Crouching, Swimming, Attacking, Jumping, Walking, Floating, Absorbing, FloatingAbsorbing, Running, Hurt};
        private KirbyPose pose = KirbyPose.Standing;
        public enum KirbyType { Normal, Dead, Beam, Spark, Fire };
        private KirbyType power = KirbyType.Normal;

        public void SetDirectionRight()
        {
            facingLeft = false;
        }
        public void SetDirectionLeft()
        {
            facingLeft = true;
        }

        public void ChangePose(KirbyPose newPose)
        {
            pose = newPose;
        }

        public void ChangeType(KirbyType newPower)
        {
            power = newPower;
        }

        public void TakeDamage()
        {
            ChangePose(KirbyPose.Hurt);
            if (health == 1)
            {
                health = maxHealth;
                // note: if this puts kirby in a dead state: kirby will have -1 lives and 6 health
                lives--;
            } 
            else
            {
                health--;
            }
        }

        public void Attack()
        {
            ChangePose(KirbyPose.Attacking);
        }


        // Uses current state to determine movement: I assume this gives back the correct location for Player to call Draw?
        public void Update()
        {
        }
    }
}
