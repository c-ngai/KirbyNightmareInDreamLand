namespace KirbyNightmareInDreamLand.StateMachines
{
    public class PlayerStateMachine : IPlayerStateMachine
    {
        // default is facing right
        private bool facingLeft;
        private KirbyPose pose;
        private KirbyType type;

        public PlayerStateMachine()
        {
            facingLeft = false;
            pose = KirbyPose.Standing;
            type = KirbyType.Normal;

        }
        #region Direction
        public void SetDirectionRight()
        {
            facingLeft = false;
        }
        public void SetDirectionLeft()
        {
            facingLeft = true;
        }

        public bool IsLeft()
        {
            return facingLeft;
        }
        #endregion

        #region Pose
        public void ChangePose(KirbyPose newPose)
        {
            pose = newPose;
        }

        public KirbyPose GetPose()
        {
            return pose;
        }
        #endregion Pose

        #region Type
        public void ChangeType(KirbyType newPower)
        {
            type = newPower;
        }

        public KirbyType GetKirbyType()
        {
            return type;
        }
        #endregion Type

        public bool HasPowerUp()
        {
            return GetKirbyType() == KirbyType.Beam || GetKirbyType() == KirbyType.Fire || GetKirbyType() == KirbyType.Spark;
        }
        public bool IsJumping()
        {
            return GetPose() == KirbyPose.JumpRising || GetPose() == KirbyPose.JumpFalling;
        }
        public bool IsFloating()
        {
            //checks if any of the float poses are active
            bool checkOne = GetPose() == KirbyPose.FloatingStart || GetPose() == KirbyPose.FloatingRising;
            bool checkTwo = GetPose() == KirbyPose.FloatingFalling || GetPose() ==KirbyPose.FloatingGrounded;
            bool checkThree = GetPose() == KirbyPose.FloatingEnd || GetPose() == KirbyPose.FreeFall;
            return checkOne || checkTwo || checkThree;

        }
        public bool IsCrouching()
        {
            return GetPose() == KirbyPose.Crouching || GetPose() == KirbyPose.Sliding;
        }
        public bool IsAttacking() //checks if kirby is not attacking
        {
            return (GetPose() == KirbyPose.Attacking) || (GetPose() == KirbyPose.Inhaling)|| (GetPose() == KirbyPose.Sliding);
        }
        public bool IsWithEnemy()
        {
            return GetKirbyType() == (KirbyType.Mouthful);
        }
        public bool IsSliding()
        {
            return GetPose() == KirbyPose.Sliding;
        }
        public bool IsFalling()
        {
            return GetPose() == KirbyPose.FreeFall;
        }

        public bool CanMove()
        {
            //walk and running cannot override jumping, floating, crouching, and attack
            bool checkOne = !IsJumping() && !IsFloating();
            bool checkTwo = !IsCrouching() && !IsAttacking();
            return checkOne && checkTwo;
        }

        public bool CanJump(){
            //not floating, not jumping, not crouching
            return !IsJumping() && !IsCrouching() && !IsFloating()  && !IsAttacking();
        }

        public bool CanFloat() //crouching and sliding cannot be overwritten by float 
        {
            return !IsCrouching() && !IsAttacking() && !IsWithEnemy(); //&& !IsFloating()
        }
        public bool CanCrouch() //crouch does not overwrite jump and floating
        {
            return !IsJumping() && !IsFloating() && !IsAttacking() && !IsWithEnemy();
        }
        public bool CanStand()
        {
            return !IsJumping() && !IsFloating();
        }

        public bool LongAttack()
        {
            return (GetKirbyType() == KirbyType.Normal && !IsCrouching()) || (GetKirbyType() == KirbyType.Fire) || (GetKirbyType() == KirbyType.Spark);
        }
        public bool ShortAttack()
        {
            return (GetKirbyType() == KirbyType.Beam) || (GetKirbyType() == KirbyType.Mouthful) || IsFloating() || IsCrouching(); //&& !IsFalling());
        }
        public string[] GetSpriteParameters()
        {
            string[] spriteParameters = new string[4];
            spriteParameters[0] = "kirby";
            spriteParameters[1] = type.ToString().ToLower();
            spriteParameters[2] = pose.ToString().ToLower();
            spriteParameters[3] = facingLeft ? "left" : "right";
            return spriteParameters;
        }

        public string GetStateString()
        {
            string power = type.ToString().ToLower();
            string posing = pose.ToString().ToLower();
            string facing = facingLeft ? "left" : "right";
            return "kirby_" + power + "_" + posing + "_" + facing;
        }

        public bool IsInAir()
        {
            bool checkOne = GetPose() == KirbyPose.FloatingStart || GetPose() == KirbyPose.FloatingRising;
            bool checkTwo = GetPose() == KirbyPose.FloatingFalling;
            bool checkThree = GetPose() == KirbyPose.FloatingEnd || GetPose() == KirbyPose.FreeFall;
            return checkOne || checkTwo || checkThree;
        }

        public bool ShouldFallThroughAirTile()
        {
            return !IsJumping() && !IsFloating();
        }
    }
}
