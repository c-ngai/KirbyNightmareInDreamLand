using Microsoft.Xna.Framework;
//using System.Numerics;
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
            public const bool DEBUG_SPRITE_MODE = false;
        } 

        public static class Physics
        {
            public const float FRAME_RATE = 0.03125f;
            public const float GRAVITY = 10f;
            public const float FLOAT_GRAVITY = 0.02f; //its 2f as the gravity times 0.01 as a "time elapsed" so float kirby falls at a constan rate
            public const float FLOAT_GRAVITY2 = 0.16f; //the other gravity rate but the float where this is used does not have game time acess
            public const float JUMP_VEL = -2f;
            public const float FLOAT_VEL = 0.5F;
            public const float WALKING_VELOCITY = 0.4f;
            public const float RUNNING_VELOCITY = 0.9f;
            public const float DAMAGE_VELOCITY =2f;
            public const float JUMP_CEILING = 38.4f;

            public const int DELAY = 400;
            public const int DELAY2 = 10;
        }

        public class Kirby
        {
            public const int MAX_HEALTH = 6;
            public const int MAX_LIVES = 3;
            public const int FLAME_ATTACK_FRAMES = 200;
            public const int BEAM_ATTACK_FRAMES = 50;
            public const int PUFF_ATTACK_FRAMES = 30;
            public static Vector2 BEAM_ATTACK_OFFSET_RIGHT = new Vector2(13, -7);
            public static Vector2 BEAM_ATTACK_OFFSET_LEFT = new Vector2(-13, -7);
            public static Vector2 PUFF_ATTACK_OFFSET = new Vector2(10, 0);
            public static Vector2 FLAME_ATTACK_OFFSET_RIGHT = new Vector2(25, -10);
            public static Vector2 FLAME_ATTACK_OFFSET_LEFT = new Vector2(-25, -10);

        }

        public class Controller
        {
            // determines max time that can elapse for double button presses to register as a command
            public const double RESPONSE_TIME = 250;
            public const double SLIDE_TIME = 250;
        }

      public class EnemyFire
        {
            // For flame segmentts
            public const int MAX_FRAMES = 13; // Number of frames before the flame segment disappears
            public const float SECONDS_PER_FRAME = 0.016f; // 60fps. 1/60 = ~0.016 seconds per frame

            // For flamethrower
            public const float FIRE_RATE = 0.35f; // Time between each segment spawn
            public const int NUMBER_OF_SEGMENTS = 10; // Number of flame segments to spawn at once
            public const float MIN_SPEED = 1f;
            public const float MAX_SPEED = 4f;
            public const float MIN_DELAY = 0f;
            public const float MAX_DELAY = 0.3f;
            public const float MIN_ANGLE = -0.3f;
            public const float MAX_ANGLE = 0.3f;

            // For fireball
            public const float SPEED = 1f; // Constant speed of the fireball
        }

        public class WaddleDooBeam
        {
            // For enemy beam
            public const int TOTAL_SEGMENTS = 16;
            public const float INITIAL_ROTATION = -MathHelper.PiOver2; // -90 degrees to fire straight up
            public const float ROTATION_STEP = MathHelper.PiOver4 / 2; // 22.5 degrees in radians
            public const int UNITS_PER_FRAME = 8;

            // For enemy beam segments
            public const int MAX_FRAMES = 6; // Segment disappears after 6 frames
        }

        public class KirbyFire
        {
            // For flamethrower
            public const float MIN_SPEED = 1f;
            public const float MAX_SPEED = 4f;
            public const float MIN_DELAY = 0f;
            public const float MAX_DELAY = 0.3f;
            public const float MIN_ANGLE = -0.3f;
            public const float MAX_ANGLE = 0.3f;
            public const int NUMBER_OF_SEGMENTS = 10;


            // For flame segments
            public const int MAX_FRAMES = 14; // Number of frames before the flame segment disappears   
            public const float SECONDS_PER_FRAME = 0.016f; // 1/60fps = 0.016 seconds per frame
        }

        public class Puff
        {
            public const float INITIAL_SPEED = 3.5f;
            public const float DECELERATION_RATE = 0.05f;
            public const int MAX_FRAMES = 20; // Puff disappears after 20 frames
            public const float SMALL_VELOCITY = 0.01f; // Cutoff for Velocity getting close to zero
        }

        public class Star
        {
            public const float SPEED = 1f; // Constant speed of the star

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

        public class BrontoBurt
        {
            public const float MOVE_SPEED = 0.5f;
            public const float WAVE_AMPLITUDE = 10f; // height of wave
            public const float WAVE_FREQUENCY = 0.05f; // wave speed
            public const int HURT_FRAMES = 50;
            public const int SLOW_FLY_FRAMES = 100;
            public const int FAST_FLY_FRAMES = 100;
            public const int STANDING_FRAMES = 100;
        }

        public class Hothead
        {
            public const float MOVE_SPEED = 0.5f; // Move speed of Hothead
            public const int WALK_FRAMES = 180;
            public const int STOP_FRAMES = 60;
            public const int ATTACK_FRAMES = 100;
            public const int SHOOT_FRAMES = 100;
            public const int HURT_FRAMES = 50;
        }

        public class PoppyBrosJr
        {
            public const float MOVE_SPEED = 0.5f;
            public const int HOP_FREQUENCY = 60; // frames between hops
            public const float HOP_HEIGHT = 1f; //height of hops
            public const int HOP_FRAMES = 180;
            public const int HURT_FRAMES = 50;

        }

        public class Sparky
        {
            public const int HOP_FREQUENCY = 60; // frames between hops
            public const float SHORT_HOP_HEIGHT = 1f;
            public const float TALL_HOP_HEIGHT = 2f;
            public const float HOP_SPEED = 0.4f; // speed

            public const int PAUSE_TIME = 30;
            public const int ATTACK_TIME = 120;
            public const int HURT_FRAMES = 50;
        }

        public class WaddleDee
        {
            public const float MOVE_SPEED = 0.5f;
            public const int WALK_FRAMES = 180;
            public const int HURT_FRAMES = 50;
        }

        public class WaddleDoo
        {
            public const float MOVE_SPEED = 0.5f;
            public const int WALK_FRAMES = 180;
            public const int STOP_FRAMES = 60;
            public const int ATTACK_FRAMES = 33;
            public const int HURT_FRAMES = 50;
            public const int JUMPING_FRAMES = 50;
            public const float JUMP_HEIGHT = 2f;
            public const float GRAVITY = 0.1f;
            public const float FORWARD_MOVEMENT = 0.5f;
        }
    }
}