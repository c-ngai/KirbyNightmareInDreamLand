namespace MasterGame
{
    public class EnemyStateMachine : IEnemyStateMachine
    {
        private bool facingLeft;
        private EnemyType type;
        private EnemyPose pose;
        public EnemyStateMachine(EnemyType newType)
        {
            facingLeft = false;
            type = newType;
            pose = EnemyPose.Walking;
        }
        #region Direction
        public void ChangeDirection()
        {
            facingLeft = !facingLeft;
        }

        public bool IsLeft()
        {
            return facingLeft;
        }
        #endregion Direction
        #region Pose
        public void ChangePose(EnemyPose newPose)
        {
            pose = newPose;
        }

        public EnemyPose GetPose()
        {
            return pose;
        }
        #endregion Pose
        #region Type
        public void ChangeType(EnemyType newType)
        {
            type = newType;
        }
        #endregion Type

        // public int[] GetSpriteParameters()
        // {

        //     int[] spriteParameters = new int[3];
        //     if (facingLeft)
        //     {
        //         spriteParameters[0] = 1;
        //     }
        //     else
        //     {
        //         spriteParameters[0] = 0;
        //     }
        //     spriteParameters[1] = (int)pose;
        //     spriteParameters[2] = (int)type;
        //     return spriteParameters;
        // }

        public string[] GetSpriteParameters()
        {
            string[] spriteParameters = new string[3];
            spriteParameters[0] = type.ToString().ToLower();
            spriteParameters[1] = pose.ToString().ToLower();
            spriteParameters[2] = facingLeft ? "left" : "right";
            return spriteParameters;
        }
    }
}
