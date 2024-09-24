
using System.Numerics;

namespace MasterGame
{
    public static class Constants
    {
        public static class Graphics
        {
            public const int GAME_HEIGHT = 160;
            public const int GAME_WIDTH = 240;
            public const int WINDOW_HEIGHT = 480;
            public const int WINDOW_WIDTH = 720;

            public const bool IS_FULL_SCREEN = false;
            public const int FLOOR = 128;
        } 

        public static class Physics
        {
            public const float GRAVITY = -10;
        }

        public class Kirby
        {
            public const int MAX_HEALTH = 6;
            public const int MAX_LIVES = 3;
        }

    }
}