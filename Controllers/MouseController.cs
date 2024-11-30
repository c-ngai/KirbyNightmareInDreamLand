using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Commands;
using KirbyNightmareInDreamLand.GameState;

namespace KirbyNightmareInDreamLand.Controllers
{
    public class MouseController : IController
    {

        private readonly Game1 game;
        private readonly List<string> roomList;

        private int updatesSinceLastMovement;

        private MouseState previousMouseState;

        public MouseController()
        {
            this.game = Game1.Instance;
            roomList = new List<string> { "hub", "room1", "room2", "room3", "treasureroom", "game_over", "winner_room"}; // Make sure names align with what's in Rooms.json
            previousMouseState = Mouse.GetState();
            updatesSinceLastMovement = 0;
        }

        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            // If mouse is inside game window
            if (game.GraphicsDevice.Viewport.Bounds.Contains(currentMouseState.Position) && game.IsActive)
            {
                // Check for left click (previous room) or right click (next room)
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    GoToPreviousRoom();
                }
                else if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                {
                    GoToNextRoom();
                }
            }

            int deltaScrollWheel = (currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) / 120;
            if (deltaScrollWheel != 0)
            {
                game.TARGET_FRAMERATE += deltaScrollWheel;
                if (game.TARGET_FRAMERATE > Constants.Graphics.MAX_FRAME_RATE)
                {
                    game.TARGET_FRAMERATE = Constants.Graphics.MAX_FRAME_RATE;
                }
                else if (game.TARGET_FRAMERATE < Constants.Graphics.MIN_FRAME_RATE)
                {
                    game.TARGET_FRAMERATE = Constants.Graphics.MIN_FRAME_RATE;
                }
                game.TargetElapsedTime = TimeSpan.FromMilliseconds(Constants.Graphics.TIME_CONVERSION / game.TARGET_FRAMERATE);
            }
            

            // Track time since last movement (in updates)
            if (currentMouseState.Position == previousMouseState.Position)
            {
                updatesSinceLastMovement++;
            }
            else
            {
                updatesSinceLastMovement = 0;
            }

            // Makes it so that the mouse disppears if it hasn't moved in more than half a second
            if (updatesSinceLastMovement > 30)
            {
                game.IsMouseVisible = false;
            }
            else
            {
                game.IsMouseVisible = true;
            }

            previousMouseState = currentMouseState;
        }

        private void GoToPreviousRoom()
        {
            // Find the index of the current room in the roomList (-1 if not present)
            int currentRoomIndex = roomList.IndexOf(game.Level.CurrentRoom.Name);
            // If the current room exists in roomList and is not the first room, load the previous room
            if (currentRoomIndex > 0 && currentRoomIndex != -1)
            {
                currentRoomIndex--;
                LoadRoom(roomList[currentRoomIndex]);
            }
        }

        private void GoToNextRoom()
        {
            // Find the index of the current room in the roomList (-1 if not present)
            int currentRoomIndex = roomList.IndexOf(game.Level.CurrentRoom.Name);
            // If the current room exists in roomList and is not the last room, load the next room
            if (currentRoomIndex < roomList.Count - 1 && currentRoomIndex != -1)
            {
                currentRoomIndex++;
                LoadRoom(roomList[currentRoomIndex]);
            }
        }

        private void LoadRoom(string roomName)
        {
            Game1.Instance.Level.LoadRoom(roomName);
        }

    }
}
