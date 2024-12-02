using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.GameState
{
    public class GamePowerChangeState : BaseGameState
    {

        public Vector2 DestinationPoint;

        private bool CurrentlyFadingOut;
        private bool CurrentlyFadingIn;
        private float FadeSpeed = Constants.Transition.FADE_VALUE_TRANSPARENT;
        private float opaqueAlpha = Constants.Transition.HALF_OPAQUE;
        private float transparentAlpha = Constants.Transition.FADE_VALUE_TRANSPARENT;
        private float startFade = Constants.Transition.FADE_OUT_START;
        private float FadeAlpha;

        private int timer = 0;

        private BlendState subtractive = new BlendState
        {
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,

            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha,

            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaBlendFunction = BlendFunction.Add
        };

        public GamePowerChangeState(Level _level) : base( _level)
        {
            SoundManager.PauseAllSounds();
            FadeAlpha = Constants.Transition.FADE_OUT_START;
            CurrentlyFadingIn = false;
            CurrentlyFadingOut = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Camera camera = _game.cameras[_game.CurrentCamera];

            // end the old spritebatch and start a fade spritebatch
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, subtractive, SamplerState.PointClamp, null, _game.rasterizerState, null, _game.cameras[_game.CurrentCamera].LevelMatrix * _game.viewMatrix);

            GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, new Color(FadeAlpha, FadeAlpha, FadeAlpha, 0), 1f);
            //GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, Color.Black, FadeAlpha);

            // end the fade spritebatch and start a new copy of the old spritebatch
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, _game.rasterizerState, null, _game.cameras[_game.CurrentCamera].LevelMatrix * _game.viewMatrix);

            // Draw ONLY the players currently changing power and their projectiles
            Game1.Instance.manager.DrawPowerChangeObjects(spriteBatch);
        }

        public override void Update()
        {
            // Update only players who are changing power right now
            foreach (IPlayer player in _manager.Players)
            {
                if (player.powerChangeAnimation)
                {
                    player.Update(Game1.Instance.time);
                }
            }
            // Update only projectiles originating from players who are changing power right now and if that player is not null
            for (int i = 0; i < _manager.Projectiles.Count; i++)
            {
                if (_manager.Projectiles[i].player != null && _manager.Projectiles[i].player.powerChangeAnimation)
                {
                    _manager.Projectiles[i].Update();
                }
            }

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

                    // set the powerChangeAnimation states of all the players to false
                    foreach (IPlayer player in _manager.Players)
                    {
                        player.powerChangeAnimation = false;
                    }
                    SoundManager.ResumeAllSounds();
                    level.ChangeState(Game1.Instance.Level._playingState);
                }
            }
        }
    }
}