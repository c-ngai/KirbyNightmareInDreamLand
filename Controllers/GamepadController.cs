using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Commands;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Reflection;

namespace KirbyNightmareInDreamLand.Controllers
{
    public class GamepadController : IController
    {
        private List<Buttons> controllerButtons;

        private Dictionary<Buttons, ICommand>[] pressedButtons;
        private Dictionary<Buttons, ICommand>[] startButtons;
        private Dictionary<Buttons, ICommand>[] stopButtons;

        private GamePadState[] oldStates;

        private int MaximumGamePadCount;

        public GamepadController()
        {
            MaximumGamePadCount = 4;
            //MaximumGamePadCount = GamePad.MaximumGamePadCount;
            Debug.WriteLine("Maximum supported Gamepads on this system: " + MaximumGamePadCount);

            controllerButtons = new List<Buttons>();

            // Initialize arrays
            pressedButtons = new Dictionary<Buttons, ICommand>[MaximumGamePadCount];
            startButtons = new Dictionary<Buttons, ICommand>[MaximumGamePadCount];
            stopButtons = new Dictionary<Buttons, ICommand>[MaximumGamePadCount];
            oldStates = new GamePadState[MaximumGamePadCount];
            for (int gamepadIndex = 0; gamepadIndex < MaximumGamePadCount; gamepadIndex++)
            {
                pressedButtons[gamepadIndex] = new Dictionary<Buttons, ICommand>();
                startButtons[gamepadIndex] = new Dictionary<Buttons, ICommand>();
                stopButtons[gamepadIndex] = new Dictionary<Buttons, ICommand>();
                oldStates[gamepadIndex] = GamePadState.Default;
            }
        }

        public void ClearMappings()
        {
            for (int gamepadIndex = 0; gamepadIndex < MaximumGamePadCount; gamepadIndex++)
            {
                pressedButtons[gamepadIndex].Clear();
                startButtons[gamepadIndex].Clear();
                stopButtons[gamepadIndex].Clear();
            }
            controllerButtons.Clear();
        }

        public void RegisterCommand(Buttons button, ExecutionType executionType, ConstructorInfo commandConstructorInfo)
        {
            
            // For every valid gamepad index
            for (int gamepadIndex = 0; gamepadIndex < MaximumGamePadCount; gamepadIndex++)
            {
                // Get the ParameterInfos for the current command constructor
                ParameterInfo[] parameterInfos = commandConstructorInfo.GetParameters();
                ICommand command;
                // If there are no parameters, invoke the ConstructerInfo with null
                if (parameterInfos.Length == 0)
                {
                    command = (ICommand)commandConstructorInfo.Invoke(null);
                }
                // If there are parameters, pass in the gamepadIndex as the only parameter (ASSUMPTION THAT THIS IS THE ONLY PARAMETER A CONSTRUCTERINFO WILL HAVE)
                else
                {
                    command = (ICommand)commandConstructorInfo.Invoke(new object[] { gamepadIndex });
                }
                
                // Add the command to its respective dictionary based on the ExecutionType
                switch (executionType)
                {
                    case ExecutionType.Pressed:
                        pressedButtons[gamepadIndex].Add(button, command);
                        break;

                    case ExecutionType.StartingPress:
                        startButtons[gamepadIndex].Add(button, command);
                        break;

                    case ExecutionType.StoppingPress:
                        stopButtons[gamepadIndex].Add(button, command);
                        break;
                }

                if (!controllerButtons.Contains(button))
                {
                    controllerButtons.Add(button);
                }
            }
            //Debug.WriteLine("controllerButtons.Count = " + controllerButtons.Count);
        }

        private bool IsButtonDown(GamePadState state, Buttons button)
        {
            float a = Constants.GamePad.ANALOG_TO_DIGITAL_QUANTIZATION_SLOPE;

            float x = state.ThumbSticks.Left.X;
            float y = state.ThumbSticks.Left.Y;
            bool inDeadZone = x * x + y * y < Constants.GamePad.THUMBSTICK_DEADZONE * Constants.GamePad.THUMBSTICK_DEADZONE;

            switch (button)
            {
                case Buttons.DPadUp:
                    bool dpadUp = state.IsButtonDown(Buttons.DPadUp);
                    bool thumbstickUp = !inDeadZone && - a * y < x && x < a * y;
                    return dpadUp || thumbstickUp;

                case Buttons.DPadDown:
                    bool dpadDown = state.IsButtonDown(Buttons.DPadDown);
                    bool thumbstickDown = !inDeadZone && a * y < x && x < -a * y;
                    return dpadDown || thumbstickDown;

                case Buttons.DPadLeft:
                    bool dpadLeft = state.IsButtonDown(Buttons.DPadLeft);
                    bool thumbstickLeft = !inDeadZone && a * x < y && y < -a * x;
                    return dpadLeft || thumbstickLeft;

                case Buttons.DPadRight:
                    bool dpadRight = state.IsButtonDown(Buttons.DPadRight);
                    bool thumbstickRight = !inDeadZone && -a * x < y && y < a * x;
                    return dpadRight || thumbstickRight;

                default:
                    return state.IsButtonDown(button);
            }
        }

        public void Update()
        {
            // For every valid gamepad index
            for (int gamepadIndex = 0; gamepadIndex < MaximumGamePadCount; gamepadIndex++)
            {
                // Check if the gamepad is connected
                GamePadCapabilities capabilities = GamePad.GetCapabilities(gamepadIndex);
                if (capabilities.IsConnected)
                {
                    // Get the current state of the controller
                    GamePadState state = GamePad.GetState(gamepadIndex);

                    // Iterates through all used buttons
                    foreach (Buttons button in controllerButtons)
                    {
                        // Execute pressed buttons if it is currently being pressed
                        if (pressedButtons[gamepadIndex].ContainsKey(button) && IsButtonDown(state, button))
                        {
                            pressedButtons[gamepadIndex][button].Execute();
                        }
                        // Execute start buttons if is is now pressed and was not pressed last update
                        if (startButtons[gamepadIndex].ContainsKey(button) && IsButtonDown(state, button) && !IsButtonDown(oldStates[gamepadIndex], button))
                        {
                            startButtons[gamepadIndex][button].Execute();
                        }
                        // Execute stop buttons if it is no longer pressed and was pressed last update
                        if (stopButtons[gamepadIndex].ContainsKey(button) && !IsButtonDown(state, button) && IsButtonDown(oldStates[gamepadIndex], button))
                        {
                            stopButtons[gamepadIndex][button].Execute();
                        }
                    }

                    // Stores current update's state for next update's use 
                    oldStates[gamepadIndex] = state;
                }
            }
        }

    }
}
