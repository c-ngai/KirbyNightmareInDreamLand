using Microsoft.Xna.Framework;

namespace MasterGame
{
    public static class Constants
    {
        public static class Graphics
        {
            public const int GAME_WIDTH = 240;
            public const int GAME_HEIGHT = 160;

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

        public class Controller
        {
            // determines max time that can elapse for double button presses to register as a command
            public const double RESPONSE_TIME = 250;
            public const double SLIDE_TIME = 250;
        }

        public class KirbyBeam
        { 
            public const int TOTAL_SEGMENTS = 18;
            public const float ROTATION_STEP = MathHelper.Pi / 32; // approx 4 degrees in radians
            public const float INIT_POSITION_RIGHT = -MathHelper.PiOver4;
            public const float INIT_POSITION_LEFT = MathHelper.PiOver4 * 5; // 5pi/4
            public const int UNITS_PER_FRAME = 10;
            public const int FRAME_FOUR = 3;
            public const int FRAME_FIVE = 4;
        }

    }
}