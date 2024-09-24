namespace MasterGame
{
    public class FloatingMovement : PlayerMovement
    {
        public FloatingMovement(ref PlayerStateMachine pState) : base(ref pState)
        {
            floating = true;
        }
    }
}
