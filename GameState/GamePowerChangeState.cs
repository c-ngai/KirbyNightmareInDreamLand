using KirbyNightmareInDreamLand.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.GameState
{
    public class GamePowerChangeState : BaseGameState
    {

        public Vector2 DestinationPoint;

        private bool CurrentlyFadingOut;
        private bool CurrentlyFadingIn;
        private float FadeSpeed = 0.05f;
        private float opaqueAlpha = 0.25f;
        private float transparentAlpha = 0.05f;
        private float startFade = 0.0f;
        private float FadeAlpha;

        private int timer = 0;

        public GamePowerChangeState(Level _level) : base( _level)
        {
            FadeAlpha = Constants.Transition.FADE_OUT_START;
            CurrentlyFadingIn = false;
            CurrentlyFadingOut = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Camera camera = _game.cameras[_game.CurrentCamera];
            GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, Color.Black, FadeAlpha);
            //GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, new Color(), 1f);
            //GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, Color.Black, 1f);
            Game1.Instance.manager.DrawPlayers(spriteBatch);
            Game1.Instance.manager.DrawProjectiles(spriteBatch);
        }

        public override void Update()
        {
            Game1.Instance.manager.UpdatePlayers();

            Game1.Instance.manager.UpdateProjectiles();

            timer ++;

            // if we are currently fading out we want to keep fading out
            if (CurrentlyFadingOut)
            {
                FadeAlpha += FadeSpeed; // increment opacity
                if (FadeAlpha >= opaqueAlpha ) // if we are opaque  
                {
                    FadeAlpha = opaqueAlpha; // reset fadeAlpha so fade-in is ready
                    CurrentlyFadingOut = false; // Fade-out complete
                }
            }

            // if we are transitioning and not fading out we want wait until the attack state timer ends to fade in
            if (!CurrentlyFadingOut && !CurrentlyFadingIn  && timer > Constants.Transition.ATTACK_STATE_FRAMES)
            {
                CurrentlyFadingIn = true; //  Que the fade in
            }

            // if we are currently fading in we want to keep fading in
            if (CurrentlyFadingIn)
            {
                FadeAlpha -= FadeSpeed; // decrement opacity
                if (FadeAlpha <= transparentAlpha) // if we are transparent 
                {
                    FadeAlpha = startFade; // reset fadeAlpha so fade-out is ready to go
                    CurrentlyFadingIn = false; // Fade-in complete
                    level.ChangeState(Game1.Instance.Level._playingState);
                }
            }
        }
    }
}