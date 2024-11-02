﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KirbyNightmareInDreamLand
{

    public class GameDebug
    {

        private readonly Game1 _game;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Texture2D texture;

        public int NumOfSpriteDrawCalls;
        public int NumOfSpriteBatchDrawCalls;
        public int NumOfStaticExecuteCollisionCalls;
        public int NumOfDynamicExecuteCollisionCalls;

        private static readonly GameDebug _instance = new();

        public static GameDebug Instance
        {
            get
            {
                return _instance;
            }
        }

        public GameDebug()
        {
            _game = Game1.Instance;
            _graphicsDevice = _game.GraphicsDevice;
            // TEMPORARY, FOR DEBUG SPRITE VISUALS
            texture = new Texture2D(_graphicsDevice, 1, 1);
            texture.SetData(new Color[] { new(255, 255, 255, 255) });

            NumOfSpriteDrawCalls = 0;
            NumOfSpriteBatchDrawCalls = 0;
            NumOfStaticExecuteCollisionCalls = 0;
            NumOfDynamicExecuteCollisionCalls = 0;
        }



        public void ResetCounters()
        {
            NumOfSpriteDrawCalls = 0;
            NumOfSpriteBatchDrawCalls = 0;
            NumOfStaticExecuteCollisionCalls = 0;
            NumOfDynamicExecuteCollisionCalls = 0;
        }



        // Draws an unfilled rectangle
        public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, float alpha)
        {
            color *= alpha;
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



        // Draws a solid-filled rectangle
        public void DrawSolidRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, float alpha)
        {
            color *= alpha;
            spriteBatch.Draw(texture, rectangle, color);
        }



        // Draws a 2x2 box centered at a point. (points refer to the top-left corner of their pixel, which is why this is 2x2 and not 1x1)
        public void DrawPoint(SpriteBatch spriteBatch, Vector2 position, Color color, float alpha)
        {
            color *= alpha;
            spriteBatch.Draw(texture, new Rectangle((int)(position.X) - 1, (int)(position.Y) - 1, 2, 2), color);
        }


        private List<Vector2> positionLog = new List<Vector2>();
        public void LogPosition(Vector2 point)
        {
            positionLog.Add(point);
        }
        public void ClearPositionLog()
        {
            positionLog.Clear();
        }
        public void DrawPositionLog(SpriteBatch spriteBatch, Color color, float alpha)
        {
            foreach (Vector2 position in positionLog)
            {
                DrawPoint(spriteBatch, position, color, alpha);
            }
        }


        // Draws the debug text from the top-left of the screen
        List<double> fpsLog = new List<double>();
        List<double> maxfpsLog = new List<double>();
        public void DrawDebugText(SpriteBatch spriteBatch)
        {
            // Log actual framerate (from time between last frame and this one)
            double frameRate = 1 / _game.time.ElapsedGameTime.TotalSeconds;
            fpsLog.Add(frameRate);
            if (fpsLog.Count > 60)
            {
                fpsLog.RemoveAt(0);
            }

            // Log "max" framerate (from time it took this frame to update and draw, not factoring in waiting time)
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
            texts.Add("Target framerate: " + _game.TARGET_FRAMERATE);
            texts.Add("Current FPS: " + Math.Round(frameRate));
            texts.Add("Average FPS: " + Math.Round(fpsLog.Average()));
            texts.Add("Current Max FPS: " + Math.Round(maxFrameRate));
            texts.Add("Average Max FPS: " + Math.Round(maxfpsLog.Average()));
            texts.Add("Current room: " + _game.Level.CurrentRoom.Name);
            texts.Add("");
            texts.Add("Sprite.Draw calls: " + NumOfSpriteDrawCalls);
            texts.Add("SpriteBatch.Draw calls: " + NumOfSpriteBatchDrawCalls);
            texts.Add("Static ExecuteCollision calls: " + NumOfStaticExecuteCollisionCalls);
            texts.Add("Dynamic ExecuteCollision calls: " + NumOfDynamicExecuteCollisionCalls);
            texts.Add("");
            texts.Add("+/- : Resize window");
            texts.Add("F : Toggle fullscreen");
            texts.Add("");
            texts.Add("F1 : Toggle debug text");
            texts.Add("F2 : Toggle sprite debug mode");
            texts.Add("F3 : Toggle level debug mode");
            texts.Add("F4 : Toggle sprite culling");
            texts.Add("F5 : Toggle collision debug mode");
            texts.Add("M : Toggle mute");
            //texts.Add("Alt (hold) : Record Kirby position");
            //texts.Add("Ctrl : Clear Kirby position log");

            // Draw lines to screen
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            for (int i = 0; i < texts.Count; i++)
            {
                Vector2 position = new Vector2(_game.WINDOW_XOFFSET + 10, _game.WINDOW_YOFFSET + 10 + 16 * i);
                spriteBatch.DrawString(LevelLoader.Instance.Font, texts[i], position, Color.Black);
            }
            spriteBatch.End();
        }


        private Color translucent = new Color( 255, 255, 255, 127 );
        // Draws black letterbox borders on the edge of the screen. Should be only visible in fullscreen. Done to maintain aspect ratio and integer scaling regardless of display resolution.
        public void DrawBorders(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            Color color = _game.DEBUG_TEXT_ENABLED ? translucent : Color.White;

            // Left side
            spriteBatch.Draw(LevelLoader.Instance.Borders, new Rectangle(0, _game.WINDOW_YOFFSET, _game.WINDOW_XOFFSET, _game.WINDOW_HEIGHT), color);
            // Top side
            spriteBatch.Draw(LevelLoader.Instance.Borders, new Rectangle(0, 0, _game.WINDOW_WIDTH + 2 * _game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET), color);
            // Right side
            spriteBatch.Draw(LevelLoader.Instance.Borders, new Rectangle(_game.WINDOW_XOFFSET + _game.WINDOW_WIDTH, _game.WINDOW_YOFFSET, _game.WINDOW_XOFFSET, _game.WINDOW_HEIGHT), color);
            // Bottom side
            spriteBatch.Draw(LevelLoader.Instance.Borders, new Rectangle(0, _game.WINDOW_YOFFSET + _game.WINDOW_HEIGHT, _game.WINDOW_WIDTH + 2 * _game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET), color);

            // weird color blendstate test: mouse X on screen determines opacity of a red rectangle covering the whole screen
            //byte alpha = (byte)((Mouse.GetState().X * 255 / Game1.Instance.GraphicsDevice.Viewport.Width - Game1.Instance.GraphicsDevice.Viewport.X));
            //DrawSolidRectangle(spriteBatch, new Rectangle(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, _game.WINDOW_WIDTH, _game.WINDOW_HEIGHT), Color.Red, (float)alpha / 255);

            spriteBatch.End();
        }

    }
}
