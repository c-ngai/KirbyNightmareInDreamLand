namespace KirbyNightmareInDreamLand.StateMachines
{
    public enum KirbyType { Normal, Beam, Spark, Fire, Professor, Mouthful };
    public enum KirbyPose
    {
        Standing, Crouching, Swimming, Attacking, ThrowEnemy, AttackingEnd,
        JumpRising, JumpFalling, Walking, FloatingStart, FloatingGrounded, FloatingRising,
        FloatingFalling, FloatingEnd, Inhaling, Running, Hurt, Sliding, FreeFall, Swallow,
        DeathSpin, DeathStun
    };
    public interface IPlayerStateMachine : IStateMachine
    {
        public void SetDirectionLeft();
        public void SetDirectionRight();
        public void ChangePose(KirbyPose newPose);
        public KirbyPose GetPose();
        public void ChangeType(KirbyType newType);
        public KirbyType GetKirbyType();
        public bool CanMove();

        public bool IsFalling();
        public bool IsFloating();

    }
}
