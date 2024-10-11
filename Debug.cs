using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;

namespace KirbyNightmareInDreamLand
{

    public class Debug
    {

        private Game1 _game;
        private GraphicsDevice _graphicsDevice;
        private Texture2D texture;



        private static Debug instance = new();

        public static Debug Instance
        {
            get
            {
                return instance;
            }
        }

        public Debug()
        {
        }


        // Create textures for drawing boxes.
        public void Load(Game1 game, GraphicsDevice graphicsDevice)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            // TEMPORARY, FOR DEBUG SPRITE VISUALS
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { new(255, 255, 255, 63) });
        }



        // Draws a rectangle around the sprite bounds and a point at its center. Pretty messy, should probably tidy up, but also it's debug, so not a priority
        public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {

            // Draw box around inside of rectangle
            // top side
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), color);
            // bottom side
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - 1, rectangle.Width, 1), color);
            // left side
            spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + 1, 1, rectangle.Height - 2), color);
            // right side
            spriteBatch.Draw(texture, new Rectangle(rectangle.X + rectangle.Width - 1, rectangle.Y + 1, 1, rectangle.Height - 2), color);
            
        }

        // Draws a rectangle around the sprite bounds and a point at its center. Pretty messy, should probably tidy up, but also it's debug, so not a priority
        public void DrawPoint(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle((int)(position.X) - 1, (int)(position.Y) - 1, 2, 2), color);
        }



        // TODO: Tidy up, this is really messy (although it is only for debug)
        List<double> fpsLog = new List<double>();
        List<double> maxfpsLog = new List<double>();
        public void DrawDebugText(SpriteBatch spriteBatch)
        {
            double frameRate = 1 / _game.time.ElapsedGameTime.TotalSeconds;
            fpsLog.Add(frameRate);
            if (fpsLog.Count > 60)
            {
                fpsLog.RemoveAt(0);
            }

            double maxFrameRate = 1 / _game.TickStopwatch.Elapsed.TotalSeconds;
            maxfpsLog.Add(maxFrameRate);
            if (maxfpsLog.Count > 60)
            {
                maxfpsLog.RemoveAt(0);
            }

            // Add debug text to list of lines
            List<string> texts = new List<string>();
            texts.Add("GraphicsAdapter.DefaultAdapter.CurrentDisplayMode: (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + ", " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + ")");
            texts.Add("GraphicsDevice.Viewport: (" + _graphicsDevice.Viewport.Width + ", " + _graphicsDevice.Viewport.Height + ")");
            texts.Add("Current FPS: " + Math.Round(frameRate));
            texts.Add("Average FPS: " + Math.Round(fpsLog.Average()));
            texts.Add("Current Max FPS: " + Math.Round(maxFrameRate));
            texts.Add("Average Max FPS: " + Math.Round(maxfpsLog.Average()));
            texts.Add("");
            texts.Add("+/- : Resize window");
            texts.Add("F : Toggle fullscreen");
            texts.Add("");
            texts.Add("F1 : Toggle debug text");
            texts.Add("F2 : Toggle sprite debug mode");
            texts.Add("F3 : Toggle level debug mode");
            texts.Add("F4 : Toggle sprite culling");
            texts.Add("F5 : Toggle collision debug mode");

            // Draw lines to screen
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            for (int i = 0; i < texts.Count; i++)
            {
                Vector2 position = new Vector2(_game.WINDOW_XOFFSET + 10, _game.WINDOW_YOFFSET + 10 + 16 * i);
                spriteBatch.DrawString(LevelLoader.Instance.font, texts[i], position, Color.Black);
            }
            spriteBatch.End();
        }

        //take off draw text magic numbers
        //eventually take these off and make game only deal with high level objects 
        //game object management takes care of the lists and iterates them
        //game then grabs it from them and does its job.

        // Draws black letterbox borders on the edge of the screen. Should be only visible in fullscreen. Done to maintain aspect ratio and integer scaling regardless of display resolution.
        public void DrawBorders(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            // Left side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(0, _game.WINDOW_YOFFSET, _game.WINDOW_XOFFSET, _game.WINDOW_HEIGHT), Color.White);
            // Top side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(0, 0, _game.WINDOW_WIDTH + 2 * _game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET), Color.White);
            // Right side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(_game.WINDOW_XOFFSET + _game.WINDOW_WIDTH, _game.WINDOW_YOFFSET, _game.WINDOW_XOFFSET, _game.WINDOW_HEIGHT), Color.White);
            // Bottom side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(0, _game.WINDOW_YOFFSET + _game.WINDOW_HEIGHT, _game.WINDOW_WIDTH + 2 * _game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET), Color.White);

            spriteBatch.End();
        }

    }
}
