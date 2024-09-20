namespace MasterGame
{
    public enum EnemyType { WaddleDee, WaddleDoo, BrontoBurt, PoppyBrosJr, Sparky, Hothead }
    public enum EnemyPose { Walking, Attacking, Jumping, LoadingAttack, Flying, Hopping }
    public interface IEnemyStateMachine : IStateMachine
    {
        public void ChangeDirection();
        public void ChangePose(EnemyPose pose);
        public EnemyPose GetPose();
        public void ChangeType(EnemyType enemy);
        public EnemyType GetEnemyType();
    }
}
