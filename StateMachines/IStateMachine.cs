using System;

namespace MasterGame
{
    public interface IStateMachine
    {
        public bool IsLeft();
        public string[] GetSpriteParameters();
    }
}
