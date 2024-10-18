using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Commands
{
    public static class CommandFactory
    {
        public static ICommand GetCommand(string commandName)
        {
            switch (commandName)
            {
                case "GraphicsToggleDebugTextCommand":
                    return new GraphicsToggleDebugTextCommand();
                case "GraphicsToggleDebugSpriteCommand":
                    return new GraphicsToggleDebugSpriteCommand();
                case "GraphicsToggleDebugLevelCommand":
                    return new GraphicsToggleDebugLevelCommand();
                case "GraphicsToggleCullingCommand":
                    return new GraphicsToggleCullingCommand();
                case "GraphicsToggleDebugCollisionCommand":
                    return new GraphicsToggleDebugCollisionCommand();
                case "GraphicsToggleFullscreenCommand":
                    return new GraphicsToggleFullscreenCommand();
                case "GraphicsIncreaseWindowSizeCommand":
                    return new GraphicsIncreaseWindowSizeCommand();
                case "GraphicsDecreaseWindowSizeCommand":
                    return new GraphicsDecreaseWindowSizeCommand();
                case "GraphicsIncreaseTargetFramerateCommand":
                    return new GraphicsIncreaseTargetFramerateCommand();
                case "GraphicsDecreaseTargetFramerateCommand":
                    return new GraphicsDecreaseTargetFramerateCommand();
                default:
                    throw new Exception($"Command '{commandName}' not found.");
            }
        }
    }
}
