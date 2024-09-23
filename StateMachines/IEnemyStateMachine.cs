namespace MasterGame
{
    public enum EnemyType { WaddleDee, WaddleDoo, BrontoBurt, PoppyBrosJr, Sparky, Hothead }
    public enum EnemyPose { Walking, Attacking, Jumping, LoadingAttack, Flying, Hopping, Hurt }
    public interface IEnemyStateMachine : IStateMachine
    {
        public void ChangeDirection();
        public void ChangePose(EnemyPose pose);
        public EnemyPose GetPose();
    }
}
