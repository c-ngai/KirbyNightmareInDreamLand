public enum KirbyPose { Standing, Crouching, Swimming, Attacking, Jumping, Walking, Floating, Absorbing, Running, Hurt };
public enum KirbyType { Normal, Dead, Beam, Spark, Fire };
namespace MasterGame
{
    public interface IPlayerStateMachine : IStateMachine
    {
        public void SetDirectionLeft();
        public void SetDirectionRight();
        public void ChangePose(KirbyPose newPose);
        public KirbyPose GetPose();
        public void ChangeType(KirbyType newType);
        public KirbyType GetKirbyType();

    }
}
