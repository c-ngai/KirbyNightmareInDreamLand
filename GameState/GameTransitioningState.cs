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
using KirbyNightmareInDreamLand.Sprites;
using System.Xml.Linq;

namespace KirbyNightmareInDreamLand.GameState
{
    public class GameTransitioningState : BaseGameState
    {

        private readonly SpriteBatch spriteBatch;
        private readonly Camera _camera;
        private readonly ObjectManager _manager;

        public Vector2 DestinationPoint;
        private bool FadeInComplete;
        private bool FadeOutComplete;
        private bool CurrentlyFadingOut;
        private bool CurrentlyFadingIn;
        private bool CurrentlyTransitioning;
        private float FadeSpeed = 0.01f;
        public float FadeAlpha { get; private set; }

        public GameTransitioningState(Level _level) : base( _level)
        {
            spriteBatch = Game1.Instance._spriteBatch;
            _camera = Game1.Instance.Camera;
            _manager = Game1.Instance.manager;
            level.FadeAlpha = 0f;

            CurrentlyFadingIn = false;
            CurrentlyFadingOut = true;
        }

        public override void Update()
        {

            base.Update();

            System.Diagnostics.Debug.WriteLine($"FadeAlpha: {FadeAlpha}, CurrentlyFadingOut: {CurrentlyFadingOut}, CurrentlyFadingIn: {CurrentlyFadingIn}");

            // if we are currently fading out we want to keep fading out
            if (CurrentlyFadingOut)
            {
                level.FadeAlpha += FadeSpeed; // increment opacity 
                if (level.FadeAlpha >= 1.0f) // if we are opaque  
                {
                    level.FadeAlpha = 1f; // reset fadeAlpha so fade-in is ready 
                    CurrentlyFadingOut = false; // Fade-out complete
                }
            }

            // if we are transitioning and not fading out we want to use the opaque screen to load the new room 
            if (!CurrentlyFadingOut && !CurrentlyFadingIn)
            {
                level.LoadRoom(level.NextRoom, level.NextSpawn); // load new room
                System.Diagnostics.Debug.WriteLine("Next room is ..." + level.NextRoom);
                CurrentlyFadingIn = true; //  Que the fade in
            }

            // if we are currently fading in we want to keep fading in
            if ( CurrentlyFadingIn)
            {
                level.FadeAlpha -= FadeSpeed; // decrement opacity 
                if (level.FadeAlpha <= 0.05f) // if we are transparent 
                {
                    level.FadeAlpha = 0f; // reset fadeAlpha so fade-out is ready to go
                    CurrentlyFadingIn = false; // Fade-in complete
                    if (level.NextRoom == "game_over")
                    {
                        level.ChangeState(Game1.Instance.Level._gameOverState);
                    }
                    else
                    {
                        level.ChangeState(Game1.Instance.Level._playingState); // We are done transitioning so set game state to playing 
                    }
                }
            }
        }
    }
}

