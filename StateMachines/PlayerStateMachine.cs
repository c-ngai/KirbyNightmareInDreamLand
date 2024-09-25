using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class PlayerStateMachine : IPlayerStateMachine
    {
        // default is facing right
        private bool facingLeft;
        private KirbyPose pose;
        private KirbyType type;

        private static PlayerStateMachine instance = new PlayerStateMachine();
        // public static PlayerStateMachine Instance
        // {
        //     get
        //     {
        //         return instance;
        //     }
        // }
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
            return "kirby_" + power + "_"+ posing + "_" + facing;
        }
    }
}
