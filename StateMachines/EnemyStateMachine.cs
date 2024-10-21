namespace KirbyNightmareInDreamLand.StateMachines
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

        public string[] GetSpriteParameters()
        {
            string[] spriteParameters = new string[3];
            spriteParameters[0] = type.ToString().ToLower();
            spriteParameters[1] = pose.ToString().ToLower();
            spriteParameters[2] = facingLeft ? "left" : "right";
            return spriteParameters;
        }

        public string GetStateString()
        {
            string facing = facingLeft ? "left" : "right";
            string posing = pose.ToString().ToLower();
            string enemy = type.ToString().ToLower();
            return enemy + "_" + posing + "_" + facing;
        }
    }
}
