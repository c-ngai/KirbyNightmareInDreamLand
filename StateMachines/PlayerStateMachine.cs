namespace MasterGame
{
    public class PlayerStateMachine : IPlayerStateMachine
    {
        // default is facing right
        private bool facingLeft = false;
        private KirbyPose pose = KirbyPose.Standing;
        private KirbyType type = KirbyType.Normal;

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
            spriteParameters[1] = (int) pose;
            spriteParameters[2] = (int) type;
            return spriteParameters;
        }
    }
}
