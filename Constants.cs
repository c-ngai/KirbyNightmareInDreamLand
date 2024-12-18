using Microsoft.Xna.Framework;
using System.Collections.Generic;
//using System.Numerics;
namespace KirbyNightmareInDreamLand
{
    public static class Constants
    {
        public static readonly List<string> ValidEnemyNames = new List<string> { "WaddleDee", "WaddleDoo", "BrontoBurt", "PoppyBrosJr", "Sparky", "Hothead", "ProfessorKirby" };

        public static class Game
        {
            public const int MAXIMUM_PLAYER_COUNT = 4;
        }
        
        #region FilePaths/NamesSpace/Graphics
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
            public const int MAX_FRAME_RATE = 60;
            public const int MIN_FRAME_RATE = 2;
            public const float TIME_CONVERSION = 1000f;
            public static Color INVINCIBLE_COLOR = new Color(255, 255, 0, 127);

            public const int PLAYER_ARROW_VISIBILITY_BOUNDS_OUTSET = 10;
            public const int PLAYER_ARROW_BOUNDS_INSET = 12;
            public const int PLAYER_ARROW_BOUNDS_BOTTOM_INSET = 14;
            public const float BLUE_ALPHA = 0.5f;
            public const float RED_ALPHA = 0.75f;
            public const float FADE_COLOR_ADDITION = 255;
        }
        #endregion
        #region  Physics
        public static class Physics
        {
            public const float GRAVITY = 1/8f; // pixel units per frame per frame (pixel/frame^2)
            public const float FLOAT_GRAVITY = 1/16f;
            public const float TERMINAL_VELOCITY = 2.5f;
            public const float FLOATING_TERMINAL_VELOCITY = 1f;
            public const float X_DECELERATION = 0.05f;

            public const float JUMP_VEL = -3f;
            public const float JUMP_RELEASE_VEL = -1f;
            public const int JUMP_MAX_FRAMES = 7;
            public const float FLOAT_YACCELLERATION = -0.15f;
            public const float FLOAT_MIN_YVEL = -1.3f;

            public const float BOUNCE_VEL = -2.25f;
            public const float BURN_BOUNCE_VEL = -3f;

            public const float WALKING_VELOCITY = 1.3f;
            public const float WALKING_ACCELLERATION = 0.1f;

            public const float RUNNING_VELOCITY = 1.75f;
            public const float RUNNING_ACCELLERATION = 0.15f;

            public const float JUMPING_XVELOCITY = 1.85f;
            public const float JUMPING_XACCELLERATION = 0.15f;

            public const float FLOATING_XVELOCITY = 1f;
            public const float FLOATING_XACCELLERATION = 0.1f;

            public const float DAMAGE_VELOCITY =2f;
            public const float JUMP_CEILING = 38.4f;

            public const float DEATH_VELOCITY = -5;
        }
        #endregion
        public static class Level
        {
            public const int TILE_SIZE = 16;
            public const int NUMBER_OF_TILE_TYPES = 10;
            public static Vector2 BOTTOM_MIDDLE_OF_TILE = new Vector2(TILE_SIZE / 2, TILE_SIZE);
            public static Vector2 ROOM1_SPAWN_POINT = new Vector2(2, 4);
            public static Vector2 HUB_SPAWN_POINT = new Vector2(2, 7);
            public static Vector2 GAME_OVER_SPAWN_POINT = new Vector2(2, 7);
            public static Vector2 HUB_DOOR_1_SPAWN_POINT = new Vector2(112, 272);
            public static Vector2 HUB_DOOR_2_SPAWN_POINT = new Vector2(240, 304);


        }
        #region  Collision
        public static class Collision
        {
            public const float GROUND_COLLISION_OFFSET = 1 - Constants.Physics.FLOAT_GRAVITY; // Use the lowest gravity can ever be. In this case, Kirby floating experiences the lowest gravity anything can experience
            
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

            public const int COLLISION_RADIUS = 55;
        }
        #endregion
        #region  HitBoxes
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
        #endregion
        #region  Kirby
        public class Kirby
        {
            public const float INVINCIBLE_TIME = 1.5f;
            public const int MAX_HEALTH = 6; //6
            public const int MAX_LIVES = 2; //2
            public const int STARTING_XPOSITION = 30;
            public const int KIRBY_VERTICAL_MIDDLE = 8;
            public const float FLOAT_UP = -0.3f;
            public static Vector2 BEAM_ATTACK_OFFSET_RIGHT = new Vector2(13, -11);
            public static Vector2 BEAM_ATTACK_OFFSET_LEFT = new Vector2(-13, -11);
            public static Vector2 PUFF_ATTACK_OFFSET = new Vector2(15, 0);
            public static Vector2 FLAME_ATTACK_OFFSET_RIGHT = new Vector2(25, -12);
            public static Vector2 FLAME_ATTACK_OFFSET_LEFT = new Vector2(-25, -12);
            public static Vector2 STAR_ATTACK_OFFSET_RIGHT= new Vector2(8, -10);
            public static Vector2 STAR_ATTACK_OFFSET_LEFT= new Vector2(-8, -10);
            public static Vector2 BOUNCING_STAR_OFFSET_RIGHT= new Vector2(8, -10);
            public static Vector2 BOUNCING_STAR_OFFSET_LEFT= new Vector2(-8, -10);
            public static Vector2 BRIEFCASE_OFFSET_RIGHT = new Vector2(6, -25);
            public static Vector2 BRIEFCASE_OFFSET_LEFT= new Vector2(-6, -25);
            public static int SLIDE_FRAMES = 40;
            public static float SLIDE_VEL = 2.5f;
            public static float SLIDE_YVEL = 1f; // band aid solution to prevent him from bouncing off of slopes too much when starting a slide
            public static float CEILING = 15;
            public static int BOUNDS = 10;
            public static int MIN_FREEFALL_FAR_FRAMES = 16;
            public static int BOUNCE_JUMP_FRAME = 2;
            public static int STOP_BOUNCE_FRAME = 22;
            public static int STOP_FLOATING_TRANSITION_FRAME = 9;
            public static int STOP_SWALLOWING = 25;
            public static int HURT_STUN_FRAMES = 24;
            public static int STOP_HURT_FRAME = 20;
            public static int STOP_HURT_FIRE_FRAME = 16;
            public static int STOP_HURT_SPARK_FRAME = 12;
            public static int STOP_BURN_BOUNCE_FRAME = 24;
            public static int FLOATING_LOOP = 16;
            public static int START_DEATH_SPIN = 90;
            public static int SET_DEATH_INACTIVE = 240;
            public static int DEATH_STAR_ANIMATION_LOOP = 8;
            public static int INVINCIBLE_ANIMATION_LOOP = 8;
            public static int INVINCIBLE_COLOR_CHANGE = 4;
            public static int JUMP_FREEFALL_START = 12;
            public static int BOUNCE_FREEFALL_START = 16;
            public static int WALL_SQUISH_END = 5;
        }
        #endregion
        public class Arrows
        {
            public const int MAX_ARROWS = 3;
        }
        public class SoundValues
        {
            public const int PITCH_ADJUSTMENT = 60;
        }

        public class Controller
        {
            // determines max time that can elapse for double button presses to register as a command
            public const double RESPONSE_FRAMES = 15;
            public const int MOUSE_IDLE_HIDE_FRAMES = 30;
        }

        public class GamePad
        {
            public const float ANALOG_TO_DIGITAL_QUANTIZATION_SLOPE = 1.5f; // For a visual explanation of what this actually represents, see https://www.desmos.com/calculator/vaylnekztn and adjust the slider at the top
            public const float THUMBSTICK_DEADZONE = 0.25f;
        }

        public class Attack
        {
            public const int END_INHALE = 30;
            public const int END_BEAM = 42;
            public const int END_SPARK = 20;
            public const int END_FIRE = 30;
            public const int END_PROFESSOR = 26;
            public const int END_STAR = 15;
            public const int END_PUFF = 9;
            public const int END_SLIDE = 40;
            public const int END_ATTACK_INHALE_ANIMATION = 3;
            public const int END_ATTACK_SPARK_ANIMATION = 1;
            public const int END_ATTACK_FIRE_ANIMATION = 7;
        }


        #region Projectiles
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
            public const int FIRE_RATE = 21; // FRAMES between each segment spawn (used to be seconds)
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
            public static Vector2  BOUNCING_STAR_VEL_LEFT = new Vector2 (-1f, -5f);
            public static Vector2  BOUNCING_STAR_VEL_RIGHT = new Vector2 (1f, -5f);
            public static double BOUNCING_TIMER = 4;

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
            public const int MAX_BEAM_FRAMES = 6;
        }

        public class Briefcase
        {
            public static Vector2 SUITCASE_VEL_LEFT = new Vector2(-2, -2);
            public static Vector2 SUITCASE_VEL_RIGHT = new Vector2(2, -2);

            public static int SUITCASE_WIDTH = 12;
            public static int SUITCASE_HEIGHT = 12;
            public static int SUITCASE_EXPLODE_WIDTH = 48;
            public static int SUITCASE_EXPLODE_HEIGHT = 48;
            public static Vector2 SUITCASE_OFFSET = new Vector2(-SUITCASE_WIDTH / 2, -SUITCASE_HEIGHT / 2);
            public static Vector2 SUITCASE_EXPLODE_OFFSET = new Vector2(-SUITCASE_EXPLODE_WIDTH / 2, -SUITCASE_EXPLODE_HEIGHT / 2);

            public static int BRIEFCASE_WINDUP_FRAMES = 8;
            public static int ENEMY_BRIEFCASE_WINDUP_FRAMES = 10;
            public static int BRIEFCASE_EXPLODE_COLLISION_FRAMES = 5;
        }
        #endregion
        #region enemies

        public class Enemies
        {
            public const int SPAWN_BOUNDS_OFFSET = 32; // 2 tiles wide/tall
            public const int DELAY = 400;
            public const int HEALTH = 1;
            public const int STRONG_ENEMY_POINTS = 600;
            public const int WEAK_ENEMY_POINTS = 400;
            public const int DAMAGE_TAKEN = 1;
            public const int HURT_VIBRATE_MAX_MAGNITUDE = 4;
            public const int HURT_FRAMES = 24;
            public const float DAMAGE_OFFSET_TO_KNOCKBACK_VELOCITY_RATIO = 1 / 8f;
        }

        public class WaddleDee
        {
            public const float MOVE_SPEED = 0.5f;
            public const int WALK_FRAMES = 180;
        }

        public class WaddleDoo
        {
            public const float MOVE_SPEED = 0.5f;
            public const int WALK_MIN_FRAMES = 60;
            public const int WALK_MAX_FRAMES = 180;
            public const int STOP_FRAMES = 60;
            public const int ATTACK_FRAMES = 25;
            public const float JUMP_VELOCITY = 3.25f;
            public const float FORWARD_MOVEMENT = 0.5f;
            public const float DEAD_FRAMES = 20;
        }

        public class BrontoBurt
        {
            public const float MOVE_SPEED = 0.75f;
            public const float WAVE_AMPLITUDE = 0.75f; // height of wave
            public const float WAVE_FREQUENCY = 0.075f; // wave speed
            public const int STANDING_FRAMES = 100;
        }

        public class PoppyBrosJr
        {
            public const float MOVE_SPEED = 0.5f;
            public const float JUMP_VELOCITY = -1f;
            public const int PAUSE_TIME = 4;
            public const float GRAVITY = 1 / 16f; // poppy bros jr has half gravity
        }

        public class Sparky
        {
            public const int HOP_FREQUENCY = 60; // frames between hops
            public const float HOP_SPEED = 1f; // speed
            public const float TALL_JUMP_VELOCITY = 3.5f;
            public const float SHORT_JUMP_VELOCITY = 1.5f;
            public const int HITBOX_RECTANGLE_OFFSET = 2;

            public const int PAUSE_TIME = 30;
            public const int ATTACK_TIME = 120;

            public const float GRAVITY = 1 / 4f; // sparky has double gravity

        }

        public class Hothead
        {
            public const float MOVE_SPEED = 0.5f; // Move speed of Hothead
            public const int WALK_MIN_FRAMES = 60;
            public const int WALK_MAX_FRAMES = 180;
            public const int FLAMETHROWER_ATTACK_FRAMES = 100;
            public const int FIREBALL_ATTACK_FRAMES = 24;
            public const int CHARGE_FRAMES = 24;
            public const float FLAMETHROWER_RANGE = 80;
            public static Vector2 FLAMETHROWER_LEFT = new Vector2(-1, 0);
            public static Vector2 FLAMETHROWER_RIGHT = new Vector2(1, 0);
            public const int FLAMETHROWER_X_OFFSET = 18;
            public const int FLAMETHROWER_Y_OFFSET = 7;
            public const int FLAMETHROWER_Y_HITBOX_OFFSET = 6;
            public static Vector2 FLAMETHROWER_OFFSET = new Vector2(18,7);
            public static Vector2 FIREBALL_LEFT = new Vector2(-1, -0.5f);
            public static Vector2 FIREBALL_RIGHT = new Vector2(1, -0.5f);
            public const int FRAME_1 = 1;
        }
        
        public class ProfessorKirby
        {
            public const int HEALTH = 10;
            public const float MOVE_SPEED = 0.5f;
            public const int WALK_FRAMES = 60;
            public const int STOP_FRAMES = 120;
            public const int ATTACK_FRAMES = 30;
            public const int JUMPING_FRAMES = 50;
            public const float JUMP_VELOCITY = 2.5f;
            public const float FORWARD_MOVEMENT = 0.5f;
            public const float DEAD_FRAMES = 20;
            public const int WALK_MIN_FRAMES = 60;
            public const int WALK_MAX_FRAMES = 180;
            public static Vector2 BRIEFCASE_OFFSET_RIGHT = new Vector2(0, -20);
            public static Vector2 BRIEFCASE_OFFSET_LEFT = new Vector2(-0, -20);
        }

        #endregion
        #region  Particle+HUD

        public class Particle
        {
            public static Vector2 STAR_STARTXY_TOPLEFT = new Vector2(-8, -16);
            public static Vector2 STAR_STARTXY_TOP = new Vector2(0, -16);
            public static Vector2 STAR_STARTXY_TOPRIGHT = new Vector2(8, -16);
            public static Vector2 STAR_STARTXY_LEFT = new Vector2(-10, -10);
            public static Vector2 STAR_STARTXY_RIGHT = new Vector2(10, -10);
            public static Vector2 STAR_STARTXY_BOTTOMLEFT = new Vector2(-8, 0);
            public static Vector2 STAR_STARTXY_BOTTOM = new Vector2(0, 0);
            public static Vector2 STAR_STARTXY_BOTTOMRIGHT = new Vector2(8, 0);

            public static Vector2 STAR_OFFSET_TOPLEFT = new Vector2(-1, -1);
            public static Vector2 STAR_OFFSET_TOP = new Vector2(0, -1);
            public static Vector2 STAR_OFFSET_TOPRIGHT = new Vector2(1, -1);
            public static Vector2 STAR_OFFSET_LEFT = new Vector2(-1, 0);
            public static Vector2 STAR_OFFSET_RIGHT = new Vector2(1, 0);
            public static Vector2 STAR_OFFSET_BOTTOMLEFT = new Vector2(-1, 1);
            public static Vector2 STAR_OFFSET_BOTTOM = new Vector2(0, 1);
            public static Vector2 STAR_OFFSET_BOTTOMRIGHT = new Vector2(1, 1);

            public static Vector2[] startingLocations = new[] { Constants.Particle.STAR_STARTXY_TOPLEFT, Constants.Particle.STAR_STARTXY_TOP, Constants.Particle.STAR_STARTXY_TOPRIGHT,
            Constants.Particle.STAR_STARTXY_LEFT, Constants.Particle.STAR_STARTXY_RIGHT, Constants.Particle.STAR_STARTXY_BOTTOMLEFT, Constants.Particle.STAR_STARTXY_BOTTOM,
            Constants.Particle.STAR_STARTXY_BOTTOMRIGHT};
            public static Vector2[] offsets = new[] { Constants.Particle.STAR_OFFSET_TOPLEFT, Constants.Particle.STAR_OFFSET_TOP, Constants.Particle.STAR_OFFSET_TOPRIGHT,
            Constants.Particle.STAR_OFFSET_LEFT, Constants.Particle.STAR_OFFSET_RIGHT, Constants.Particle.STAR_OFFSET_BOTTOMLEFT, Constants.Particle.STAR_OFFSET_BOTTOM,
            Constants.Particle.STAR_OFFSET_BOTTOMRIGHT};

            public static int OFFSET1 = 0;
            public static int OFFSET8 = 8;
            public static int STAR_MAX_FRAMES = 14;
            public static int STAREXPLODE_MAX_FRAMES = 14;
            public static int STARDUST_MAX_FRAMES = 7;
            public static int ENEMYEXPLODE_MAX_FRAMES = 23;
            public static int ENEMYEXPLODE_START_FRAME = 6; // frames before enemy disappears
            public static Vector2 ENEMYEXPLODE_OFFSET = new Vector2(0, -8);
            public static int DROPABILITY_MAX_FRAMES = 8;

            public static int DASH_CLOUD_FRAMES = 10;
            public static int DASH_CLOUD_LOOPS = 3;

            public static int PAPER_COUNT = 16;
            public static int PAPER_START_FRAME = 3;
            public static Vector2 PAPER_SPAWN_OFFSET = new Vector2(0, -5);
            public static float PAPER_INITIAL_XVEL_MIN = -2f;
            public static float PAPER_INITIAL_XVEL_MAX = 2f;
            public static float PAPER_INITIAL_YVEL_MIN = -2f;
            public static float PAPER_INITIAL_YVEL_MAX = -0.5f;
            public static float PAPER_TERMINALVEL_MIN = 1.25f;
            public static float PAPER_TERMINALVEL_MAX = 1.75f;
            public static float PAPER_GRAVITY = 0.05f;
            public static float PAPER_X_DECELERATION = 0.05f;
            public static int PAPER_FRAMES = 36;
        }

        public class HUD
        {
            public const float SLIDE_SPEED = 8f; // Speed at which sprites slide up/down
            public const float STAY_TIME = 2f; // Time in seconds to stay at position (0, 115)
            public static Vector2 POWERUP_INIT_POS = new Vector2(0, 147);
            public const float POWERUP_INIT_TIMER = 0f; 
            public const int POWERUP_MAX_Y = 115;
            public const int SPRITES_Y = 147;
            public const int SCORE_PAD = 8;
            public const int SPRITE_GAP = 8;
            public const int SCORE1 = 176;
            public static Vector2 LIVES_ICON_POS = new Vector2(57, 147);
            public const int LIVES_PAD = 2;
            public static Vector2 LIVES_TENS_POS = new Vector2(80, 147);
            public static Vector2 LIVES_ONES_POS = new Vector2(88, 147);
            public const int HEALTH_INIT_X = 104;
            public const int HEALTH_Y = 146;
            public const int HEALTH_NEXT_X = 8;
        }

        public class Hub
        {
            public static Vector2 DRAW_HUB_DOOR_OFFSET = new Vector2(0, -8);
            public static Vector2 DRAW_HUB_DOOR_SIGN_OFFSET = new Vector2(2, -24);
        }

        public class RoomStrings
        {
            public const string ROOM_1 = "room1";
            public const string GAME_OVER_ROOM = "game_over";
            public const string LEVEL_COMPLETE_ROOM = "winner_room";
        }

        public class ButtonLocations
        {
            public static Vector2 GAMEOVER_BUTTONS = new Vector2(136, 71);
            public static Vector2 LEVEL_COMPLETE_BUTTONS = new Vector2(125, 80);
        }

        public class Transition
        {
            public const float FADE_SPEED = 0.03f;
            public const float FADE_SPEED_DARK = 0.125f;
            public const float FADE_OUT_START = 0.0f;
            public const float FADE_VALUE_OPAQUE = 1.0f;
            public const float FADE_VALUE_HALF_OPAQUE = 0.5f;
            public const float FADE_VALUE_TRANSPARENT = 0.05f;
            public const int FADE_HOLD_FRAMES = 75;
            public const float HALF_OPAQUE = 0.5f;
            public const int ATTACK_FRAMES = 45;

        }
        #endregion

        #region Debug
        public class DebugValues
        {
            public const int GREEN_R = 0;
            public const int GREEN_G = 255;
            public static Color TRANSLUCENT = new Color(127, 127, 127, 127);
            public const int GREEN_B = 0;
            public const float GREEN_ALPHA = 0.5f;
            public const float HALF_OPAQUE = 0.5f;
            public const float RED_ALPHA = 1.0f;
        }
        #endregion
    }
}