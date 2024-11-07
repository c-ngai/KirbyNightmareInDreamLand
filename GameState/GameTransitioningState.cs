using System;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.Levels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static KirbyNightmareInDreamLand.Levels.Level;

namespace KirbyNightmareInDreamLand.GameState
{
    public class GameTransitioningState : BaseGameState
    {

        private readonly SpriteBatch spriteBatch;
        private readonly Camera _camera;
        private readonly ObjectManager _manager;
        private readonly Level _level;

        public Vector2 DestinationPoint;
        private bool FadeInComplete;
        private bool FadeOutComplete;
        private bool CurrentlyFadingOut;
        private bool CurrentlyFadingIn;
        private bool CurrentlyTransitioning;
        private float FadeSpeed = 0.01f;
        public float FadeAlpha { get; private set; }

        public GameTransitioningState()
        {
            spriteBatch = Game1.Instance._spriteBatch;
            _camera = Game1.Instance.Camera;
            _manager = Game1.Instance.manager;
            _level = Game1.Instance.Level;

            FadeAlpha = 0f;

            CurrentlyFadingIn = false;
            CurrentlyFadingOut = true;
        }

        public override void Draw()
        {
            DrawBackground(spriteBatch);
            DrawForeground(spriteBatch);
            DrawDoorStars(spriteBatch);
            DrawLevelObjects(spriteBatch);
        }

        public override void Update()
        {
            System.Diagnostics.Debug.WriteLine($"FadeAlpha: {FadeAlpha}, CurrentlyFadingOut: {CurrentlyFadingOut}, CurrentlyFadingIn: {CurrentlyFadingIn}");

            // if we are currently fading out we want to keep fading out
            if (CurrentlyFadingOut)
            {
                FadeAlpha += FadeSpeed; // increment opacity 
                if (FadeAlpha >= 1.0f) // if we are opaque  
                {
                    FadeAlpha = 1f; // reset fadeAlpha so fade-in is ready 
                    CurrentlyFadingOut = false; // Fade-out complete
                }
            }

            // if we are transitioning and not fading out we want to use the opaque screen to load the new room 
            if (!CurrentlyFadingOut && !CurrentlyFadingIn)
            {
                _level.LoadRoom(_level.NextRoom, _level.NextSpawn); // load new room
                System.Diagnostics.Debug.WriteLine("Loading new room ...");
                CurrentlyFadingIn = true; //  Que the fade in
            }

            // if we are currently fading in we want to keep fading in
            if ( CurrentlyFadingIn)
            {
                FadeAlpha -= FadeSpeed; // decrement opacity 
                if (FadeAlpha <= 0.05f) // if we are transparent 
                {
                    FadeAlpha = 0f; // reset fadeAlpha so fade-out is ready to go
                    CurrentlyFadingIn = false; // Fade-in complete
                    _level.ChangeState(Game1.Instance.Level._playingState); // We are done transitioning so set game state to playing 
                }
            }
        }
    }
}

