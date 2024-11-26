using KirbyNightmareInDreamLand.Levels;
using Microsoft.Xna.Framework;


namespace KirbyNightmareInDreamLand.GameState
{
    public class GameTransitioningState : BaseGameState
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

        public GameTransitioningState(Level _level) : base( _level)
        {
            level.FadeAlpha = Constants.Transition.FADE_OUT_START;
            CurrentlyFadingIn = false;
            CurrentlyFadingOut = true;
        }

        public override void Update()
        {

            

            //System.Diagnostics.Debug.WriteLine($"FadeAlpha: {FadeAlpha}, CurrentlyFadingOut: {CurrentlyFadingOut}, CurrentlyFadingIn: {CurrentlyFadingIn}");

            // if we are currently fading out we want to keep fading out
            if (CurrentlyFadingOut)
            {
                level.FadeAlpha += FadeSpeed; // increment opacity 
                if (level.FadeAlpha >= opaqueAlpha) // if we are opaque  
                {
                    level.FadeAlpha = opaqueAlpha; // reset fadeAlpha so fade-in is ready 
                    CurrentlyFadingOut = false; // Fade-out complete
                }
            }

            Vector2? doorPositionForNextRoom = level.NextSpawn;

            // if we are transitioning and not fading out we want to use the opaque screen to load the new room 
            if (!CurrentlyFadingOut && !CurrentlyFadingIn)
            {
                level.LoadRoom(level.NextRoom, doorPositionForNextRoom); // load new room
                //System.Diagnostics.Debug.WriteLine("Next room is ..." + level.NextRoom);
                CurrentlyFadingIn = true; //  Que the fade in
            }

            // if we are currently fading in we want to keep fading in
            if ( CurrentlyFadingIn)
            {
                level.FadeAlpha -= FadeSpeed; // decrement opacity 
                if (level.FadeAlpha <= transparentAlpha) // if we are transparent 
                {
                    level.FadeAlpha = startFade; // reset fadeAlpha so fade-out is ready to go
                    CurrentlyFadingIn = false; // Fade-in complete
                    if (level.NextRoom == gameOverString)
                    {
                        level.ChangeState(Game1.Instance.Level._gameOverState);
                    }
                    else if (level.NextRoom == "winner_room")
                    {
                        level.ChangeState(Game1.Instance.Level._winningState);
                    }
                    else
                    {
                        level.ChangeState(Game1.Instance.Level._playingState); // We are done transitioning so set game state to playing
                        level.IsDoorBeingOpened = false;
                        level.IsDoorBeingExited = true;
                        level.ExitDoorAt(doorPositionForNextRoom);
                    }
                }
            }

            base.Update();
        }


    }
}

