namespace KirbyNightmareInDreamLand.StateMachines
{
    public enum KirbyType { Normal, Dead, Beam, Spark, Fire, Mouthful };
    public enum KirbyPose
    {
        Standing, Crouching, Swimming, Attacking, ThrowEnemy,
        JumpRising, JumpFalling, Walking, FloatingStart, FloatingGrounded, FloatingRising,
        FloatingFalling, FloatingEnd, Inhaling, Running, Hurt, Sliding, FreeFall, Swallow
    };
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
