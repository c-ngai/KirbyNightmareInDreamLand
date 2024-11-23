using KirbyNightmareInDreamLand.Levels;
using Microsoft.Xna.Framework;


namespace KirbyNightmareInDreamLand.GameState
{
    public class GamePowerChangeState : BaseGameState
    {

        public Vector2 DestinationPoint;

        private bool CurrentlyFadingOut;
        private bool CurrentlyFadingIn;
        private float FadeSpeed = Constants.Transition.FADE_SPEED;
        private float opaqueAlpha = Constants.Transition.FADE_VALUE_OPAQUE;
        private float transparentAlpha = Constants.Transition.FADE_VALUE_TRANSPARENT;
        private float startFade = Constants.Transition.FADE_OUT_START;
        private string gameOverString = Constants.RoomStrings.GAME_OVER_ROOM;
        public float FadeAlpha { get; private set; }

        private double timer = 0;

        public GamePowerChangeState(Level _level) : base( _level)
        {
            level.FadeAlpha = Constants.Transition.FADE_OUT_START;
            CurrentlyFadingIn = false;
            CurrentlyFadingOut = true;
        }

        public override void Update()
        {
            //if(timer == 0)Game1.Instance.huds[0].ActivatePowerup("ui_power_beam");
            base.Update();

            //System.Diagnostics.Debug.WriteLine($"FadeAlpha: {FadeAlpha}, CurrentlyFadingOut: {CurrentlyFadingOut}, CurrentlyFadingIn: {CurrentlyFadingIn}");

            timer += Game1.Instance.time.ElapsedGameTime.TotalSeconds; 

            if(timer > Constants.Transition.ATTACK_STATE_TIMER)
            {
                level.ChangeState(Game1.Instance.Level._playingState); 
            }
        }
    }
}