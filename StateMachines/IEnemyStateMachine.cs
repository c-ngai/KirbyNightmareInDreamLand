namespace KirbyNightmareInDreamLand.StateMachines
{
    public enum EnemyType { WaddleDee, WaddleDoo, BrontoBurt, PoppyBrosJr, Sparky, Hothead, ProfessorKirby }
    public enum EnemyPose { Walking, Attacking, Jumping, Charging, FlyingSlow, FlyingFast, Standing, Hop, Hurt, Shooting }
    public interface IEnemyStateMachine : IStateMachine
    {
        public void ChangeDirection();
        public void SetDirection(bool facingLeft);
        public void ChangePose(EnemyPose pose);
        public EnemyPose GetPose();
    }
}
