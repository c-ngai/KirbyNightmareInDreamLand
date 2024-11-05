using Microsoft.Xna.Framework;
using System.Collections.Generic;
//using System.Numerics;
namespace KirbyNightmareInDreamLand
{
    public static class Constants
    {
        public static readonly List<string> ValidEnemyNames = new List<string> { "WaddleDee", "WaddleDoo", "BrontoBurt", "PoppyBrosJr", "Sparky", "Hothead" };

        public static class Game
        {
            public const int MAXIMUM_PLAYER_COUNT = 4;
        }

        public static class Filepaths
        {
            public const string TextureList = "Content/Images/Textures.txt";
            public const string SpriteRegistry = "Content/Images/SpriteAnimations.json";
            public const string TilemapList = "Content/Tilemaps.txt";
            public const string RoomRegistry = "Content/Rooms.json";
            public const string KeymapRegistry = "Content/Keymaps.json";
            public const string ButtonmapRegistry = "Content/Buttonmaps.json";
            public const string TileSpriteList = "Content/Images/Level/TileSprites.txt";
            public const string HitboxRegistry = "Content/Hitboxes.json";
            // Directories
            public const string AudioDirectory = "Content/Audio";
        }

        public static class Namespaces
        {
            public const string ENEMY_NAMESPACE = "KirbyNightmareInDreamLand.Entities.Enemies.";
            public const string POWERUP_NAMESPACE = "KirbyNightmareInDreamLand.Entities.PowerUps.PowerUp";

        }

        public static class Graphics
        {
            public const int GAME_WIDTH = 240;
            public const int GAME_HEIGHT = 160;
            public const int FLOOR = 128;
            public const float PARALLAX_FACTOR = 0.85f;
        }

        public static class Physics
        {
            public const float FRAME_RATE = 0.03125f;
            public const float GRAVITY = 10f;
            public const float DT = 0.0166666f;
            public const float FLOAT_GRAVITY = 5f; //its 2f as the gravity times 0.01 as a "time elapsed" so float kirby falls at a constan rate
            public const float FLOAT_GRAVITY2 = 0.16f; //the other gravity rate but the float where this is used does not have game time acess
            public const float JUMP_VEL = -2f;
            public const float JUMP_MAX_HEIGHT = 45; // Slight misnomer, not the max height of the jump but the height that you can no longer accellerate upwards at by holding jump
            public const float FLOAT_VEL = 0.9F;
            public const float WALKING_VELOCITY = 1.3f;
            public const float RUNNING_VELOCITY = 1.75f;
            public const float DAMAGE_VELOCITY =2f;
            public const float JUMP_CEILING = 38.4f;

            public const int DELAY = 400;
            public const int DELAY2 = 10;
        }

        public static class Level
        {
            public const int TILE_SIZE = 16;
            public const int NUMBER_OF_TILE_TYPES = 10;
            public static Vector2 BOTTOM_MIDDLE_OF_TILE = new Vector2(TILE_SIZE / 2, TILE_SIZE);
        }

        public static class Collision
        {
            public const float GENTLE1_SLOPE_LEFT_M = 0.5F;
            public const int GENTLE1_SLOPE_LEFT_YINTERCEPT = 0;

            public const float GENTLE2_SLOPE_LEFT_M = 0.5F;
            public const int GENTLE2_SLOPE_LEFT_YINTERCEPT = 8;

            public const float STEEP_SLOPE_LEFT_M = 1;
            public const int STEEP_SLOPE_LEFT_YINTERCEPT = 0;

            public const float GENTLE1_SLOPE_RIGHT_M = -0.5F;
            public const int GENTLE1_SLOPE_RIGHT_YINTERCEPT = 8;

            public const float GENTLE2_SLOPE_RIGHT_M = -0.5F;
            public const int GENTLE2_SLOPE_RIGHT_YINTERCEPT = 16;

            public const float STEEP_SLOPE_RIGHT_M = -1;
            public const int STEEP_SLOPE_RIGHT_YINTERCEPT = 16;
        }
        public static class HitBoxes
        {
            public const int ENTITY_WIDTH = 13;
            public const int ENTITY_HEIGHT = 15;
            public const int ENEMY_WIDTH = 16;
            public const int ENEMY_HEIGHT = 16;
            public const int SPARKY_ATTACK_WIDTH = 40;
            public const int SPARKY_ATTACK_HEIGHT = 40;
            public const int SPARKY_ATTACK_OFFSET =11;
            public const int BEAM_HEIGHT = 10;
            public const int BEAM_WIDTH = 10;
            public const int BEAM_HEIGHT_OFFSET = 7;
            public const int FIREBALL_HEIGHT = 15;
            public const int FIREBALL_WIDTH = 15;
            public const int FIREBALL_OFFSET = 7;
            public const int FLAME_HEIGHT = 10;
            public const int FLAME_WIDTH = 10;

            public const int TILE_WIDTH = 16;
            public const int TILE_HEIGHT = 16;
            public const int NULL_WIDTH = 0;
            public const int NULL_HEIGHT = 0;
            //specifically for inhale and electric which are not projectiles
            public const int ATTACK_SIZE = 36;
            public const int SIDES = 4;
            
            public static int BEAM_SIZE = 18;
            public static Vector2 BEAM_OFFSET = new Vector2(-9, -9);
            public static Vector2 BEAM_OFFSET_LEFT = new Vector2(-13, -7);

            public static int FIRE_SIZE = 20;
            public static Vector2 FIRE_OFFSET_RIGHT = new Vector2(0, -5);
            public static Vector2 FIRE_OFFSET_LEFT = new Vector2(-5, -5);

            public static int PUFF_SIZE = 15;
            public static Vector2 PUFF_OFFSET = new Vector2(-8, -9);
            public static Vector2 PUFF_OFFSET_LEFT = new Vector2(-4, -5);

            //Inhale
            public static int NORMAL_SIZE = 36;
            public static Vector2 NORMAL_OFFSET_RIGHT = new Vector2(7, -24);
            public static Vector2 NORMAL_OFFSET_LEFT = new Vector2(-41, -24);

            public static int SPARK_SIZE = 55;
            public static Vector2 SPARK_OFFSET = new Vector2(-27, -33);

            public static int SLIDE_WIDTH = 12;
            public static int SLIDE_HEIGHT = 30;
            public static Vector2 SLIDE_OFFSET_RIGHT = new Vector2(7, -12);
            public static Vector2 SLIDE_OFFSET_LEFT = new Vector2(-19, -12);

            public static int STAR1_SIZE = 12;
            public static Vector2 STAR_OFFSET_RIGHT = new Vector2(7, -12);
            public static Vector2 STAR_OFFSET_LEFT = new Vector2(-19, -12);
        }

        public class Kirby
        {
            public const int MAX_HEALTH = 6;
            public const int MAX_LIVES = 3;
            public static Vector2 BEAM_ATTACK_OFFSET_RIGHT = new Vector2(13, -7);
            public static Vector2 BEAM_ATTACK_OFFSET_LEFT = new Vector2(-13, -7);
            public static Vector2 PUFF_ATTACK_OFFSET = new Vector2(15, 0);
            public static Vector2 FLAME_ATTACK_OFFSET_RIGHT = new Vector2(30, -10);
            public static Vector2 FLAME_ATTACK_OFFSET_LEFT = new Vector2(-25, -10);
            public static Vector2 STAR_ATTACK_OFFSET_RIGHT= new Vector2(8, -10);
            public static Vector2 STAR_ATTACK_OFFSET_LEFT= new Vector2(-8, -10);

        }

        public class Controller
        {
            // determines max time that can elapse for double button presses to register as a command
            public const double RESPONSE_TIME = 250;
            public const double SLIDE_TIME = 250;
        }

        public class GamePad
        {
            public const float ANALOG_TO_DIGITAL_QUANTIZATION_SLOPE = 1.5f; // For a visual explanation of what this actually represents, see https://www.desmos.com/calculator/vaylnekztn and adjust the slider at the top
            public const float THUMBSTICK_DEADZONE = 0.25f;
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
            public const float FIRE_RATE = 0.35f; // Time between each segment spawn
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
            public const float SPEED = 4f; // Constant speed of the star

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
            public const float TALL_JUMP_VELOCITY = 2.1f;
            public const float SHORT_JUMP_VELOCITY = 1.5f;

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
            public const int WALK_FRAMES = 250;
            public const int STOP_FRAMES = 120;
            public const int ATTACK_FRAMES = 50;
            public const int HURT_FRAMES = 50;
            public const int JUMPING_FRAMES = 50;
            public const float JUMP_VELOCITY = 2.5f;
            public const float FORWARD_MOVEMENT = 0.5f;
            public const float DEAD_FRAMES = 20;
        }

        public class Particle
        {
            public static Vector2 STAR_OFFSET_TOPLEFT = new Vector2(-4, -10);
            public static Vector2 STAR_OFFSET_TOP = new Vector2(0, -10);
            public static Vector2 STAR_OFFSET_TOPRIGHT = new Vector2(4, -10);
            public static Vector2 STAR_OFFSET_LEFT = new Vector2(-4, -5);
            public static Vector2 STAR_OFFSET_RIGHT = new Vector2(4, -5);
            public static Vector2 STAR_OFFSET_BOTTOMLEFT = new Vector2(-4, 0);
            public static Vector2 STAR_OFFSET_BOTTOM = new Vector2(0, 0);
            public static Vector2 STAR_OFFSET_BOTTOMRIGHT = new Vector2(4, 0);

        }
    }
}