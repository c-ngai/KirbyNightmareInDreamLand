namespace MasterGame
{
    public class EnemyStateMachine : IEnemyStateMachine
    {
        private bool facingLeft = false;
        // Needs to be initialized by each enemy class
        private EnemyType type;
        private EnemyPose pose;
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

        public EnemyType GetEnemyType()
        {
            return type;
        }
        #endregion Type

        public int[] GetSpriteParameters()
        {

            int[] spriteParameters = new int[3];
            if (facingLeft)
            {
                spriteParameters[0] = 1;
            }
            else
            {
                spriteParameters[0] = 0;
            }
            spriteParameters[1] = (int)pose;
            spriteParameters[2] = (int)type;
            return spriteParameters;
        }
    }
}
