using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Commands;

namespace KirbyNightmareInDreamLand.Controllers
{
    public class MouseController : IController
    {

        private Game1 game;
        private List<string> roomList;
        private int currentRoomIndex;

        private MouseState previousMouseState;

        public MouseController()
        {
            this.game = Game1.Instance;
            roomList = new List<string> { "room1", "room2", /*"room3", */ "testroom1"}; // Make sure names align with what's in Rooms.json
            currentRoomIndex = 0;
        }

        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();

            // Check for left click (previous room) or right click (next room)
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                GoToPreviousRoom();
            }
            else if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
            {
                GoToNextRoom();
            }

            previousMouseState = currentMouseState;
        }

        private void GoToPreviousRoom()
        {
            if (currentRoomIndex > 0)
            {
                currentRoomIndex--;
                LoadRoom(roomList[currentRoomIndex]);
            }
        }

        private void GoToNextRoom()
        {
            if (currentRoomIndex < roomList.Count - 1)
            {
                currentRoomIndex++;
                LoadRoom(roomList[currentRoomIndex]);
            }
        }

        private void LoadRoom(string roomName)
        {
            game.level.LoadRoom(roomName);
        }

    }
}
