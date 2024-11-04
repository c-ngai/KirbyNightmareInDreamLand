namespace KirbyNightmareInDreamLand.StateMachines
{
    public enum LevelState { Playing, Paused, GameOver, WiningScreen, Transitioning};

    public interface ILevelStateMachine : IStateMachine
    {
        public void ChangeState(LevelState newState);
        public LevelState GetLevelState();

    }
}

