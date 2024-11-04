using System;
namespace KirbyNightmareInDreamLand.StateMachines
{
	public class LevelStateMachine : ILevelStateMachine
	{
        private LevelState state;

        public LevelStateMachine()
        {
            state = LevelState.Playing;
        }

        public void ChangeState(LevelState newState)
        {
            state = newState;
        }

        public bool IsPaused()
        {
            return state == LevelState.Paused;
        }

        public bool IsPlaying()
        {
            return state == LevelState.Playing;
        }

        public bool IsGameOver()
        {
            return state == LevelState.GameOver;
        }

        public bool IsWinning()
        {
            return state == LevelState.WiningScreen;
        }

        public bool IsTransitioning()
        {
            return state == LevelState.Transitioning;
        }


        public string[] GetSpriteParameters()
        {
            throw new NotImplementedException();
        }

        public bool IsLeft()
        {
            throw new NotImplementedException();
        }

        public LevelState GetLevelState()
        {
            throw new NotImplementedException();
        }
    }
}

