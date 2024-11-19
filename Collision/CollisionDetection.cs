using System;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace KirbyNightmareInDreamLand
{
    public enum CollisionType
    {
        Air,
        Block,
        Platform,
        Water,
        SlopeSteepLeft,
        SlopeGentle1Left,
        SlopeGentle2Left,
        SlopeGentle2Right,
        SlopeGentle1Right,
        SlopeSteepRight,
        BouncingStar, Enemy, EnemyAttack, Player, PlayerAttack, KirbyStar, PowerUp
    }
    public sealed class CollisionDetection
    {
        private ObjectManager manager { get; }
        private CollisionResponse response { get; }
        private Game1 game { get; }
        private Level level { get; }
        private static CollisionDetection instance = new CollisionDetection();
        private bool CollisionOn = true; // for debug purposes
        public static CollisionDetection Instance
        {
            get
            {
                return instance;
            }
        }
        public CollisionDetection()
        {
            manager = ObjectManager.Instance;
            response = CollisionResponse.Instance;
            game = Game1.Instance;
            level = game.Level;
        }
        
        private bool IsCloseEnough(ICollidable obj1, ICollidable obj2)
        {
            //check how many obejcts are taken out by these checks
            int distance =  obj1.GetHitBox().X - obj2.GetHitBox().X;
            int distance2 = obj1.GetHitBox().Y - obj2.GetHitBox().Y; //take off the Y 
            int close = 55;

            // If the distance between the objects is less than the sum of their radii, they're close enough to check further
            return distance<close && distance2<close;
        }

        // Method to handle collision detection
        public CollisionSide DetectCollisionSide(Rectangle object1, Rectangle intersection)
        {
            CollisionSide side = CollisionSide.Right;

            // More width determines collision must've occured either on the top or bottom
            if (intersection.Width >= intersection.Height)
            {
                // Left corner vertical alignment means it is a top collision
                if (object1.Y == intersection.Y)
                {
                    side = CollisionSide.Top;
                }
                else
                {
                    side = CollisionSide.Bottom;
                }
            }
            // More height determines collision must've occurred on the left or right
            else
            {
                // Left corner horizontal alignment means it must a a left collision
                if (object1.X == intersection.X)
                {
                    side = CollisionSide.Left;
                }
                else
                {
                    side = CollisionSide.Right;
                }
            }
            return side;
        }
        //in charge of static (tile) collisions
        public void StaticCollisionCheck()
        {
            foreach (var dynamicObj in manager.DynamicObjects)
            {
                // Registers all relevant tiles and returns a list of them
                IntersectingTiles(dynamicObj.GetHitBox(), dynamicObj.GetPosition());
                foreach (var staticObj in manager.StaticObjects)
                {
                    if (dynamicObj.GetHitBox().Intersects(staticObj.GetHitBox()))
                    {
                        Rectangle intersection = Rectangle.Intersect(dynamicObj.GetHitBox(), staticObj.GetHitBox());

                        CollisionSide side = DetectCollisionSide(dynamicObj.GetHitBox(), intersection);

                        CollisionType type1 = dynamicObj.GetCollisionType();

                        CollisionType type2 = staticObj.GetCollisionType();
                        Tuple<CollisionType, CollisionType, CollisionSide> key = new Tuple<CollisionType, CollisionType, CollisionSide>(type1, type2, side);
                        // Collision detection does not care about the response dictionary but debug does to accurately keep track of dynamic execution calls
                        if (response.collisionMapping.ContainsKey(key))
                        {
                            GameDebug.Instance.NumOfStaticExecuteCollisionCalls++;
                        }
                        response.ExecuteCollision(dynamicObj, staticObj, side);
                    }
                }
                // Clears relevant tiles after each dynamic object
                manager.ResetStaticObjects();
            }
        }
         //in charge of dynamic (tile) collisions
        public void DynamicCollisionCheck()
        {
            for (int i = 0; i < manager.DynamicObjects.Count; i++)
            {
                if (!manager.DynamicObjects[i].CollisionActive) continue;
                for (int j = i + 1; j < manager.DynamicObjects.Count; j++)
                {
                    //run and time and comment out close enough to check if it improved performance
                    if(!IsCloseEnough(manager.DynamicObjects[i],manager.DynamicObjects[j])) continue;
                    if(!manager.DynamicObjects[j].CollisionActive) continue;
                    if (manager.DynamicObjects[i].GetHitBox().Intersects(manager.DynamicObjects[j].GetHitBox()))
                    {
                        Rectangle intersection = Rectangle.Intersect(manager.DynamicObjects[i].GetHitBox(), manager.DynamicObjects[j].GetHitBox());

                        CollisionSide side = DetectCollisionSide(manager.DynamicObjects[i].GetHitBox(), intersection);

                        CollisionType type1 = manager.DynamicObjects[i].GetCollisionType();
                        CollisionType type2 = manager.DynamicObjects[j].GetCollisionType();
                        
                        Tuple<CollisionType, CollisionType, CollisionSide> key = new Tuple<CollisionType, CollisionType, CollisionSide>(type1, type2, side);

                        // Collision detection does not care about the response dictionary but debug does to accurately keep track of dynamic execution calls
                        if (response.collisionMapping.ContainsKey(key))
                        {
                            GameDebug.Instance.NumOfDynamicExecuteCollisionCalls++;
                        }
                        response.ExecuteCollision(manager.DynamicObjects[i], manager.DynamicObjects[j], side);
                    }
                }
                // Removes dynamic objects that are no longer active after checking a dynamic object with all other possibilies
                manager.UpdateDynamicObjects();
            }
        }

        // Method to handle collision detection
        public void CheckCollisions()
        {
            if (CollisionOn)
            {
                // Check dynamic objects against static objects
                StaticCollisionCheck();
                // Check dynamic objects against each other, avoiding duplicate tests
                DynamicCollisionCheck();
            }
        }

        // Given a rectangle in the world, returns a List of all Tiles in the level that intersect with that given rectangle.
        public List<Tile> IntersectingTiles(Rectangle collisionRectangle, Vector2 position)
        {
            List<Tile> tiles = new List<Tile>();

            // Set bounds on the TileMap to iterate from
            int TopY = Math.Max(collisionRectangle.Top / Constants.Level.TILE_SIZE, 0);
            int BottomY = Math.Min(collisionRectangle.Bottom / Constants.Level.TILE_SIZE + 1, level.CurrentRoom.TileHeight);
            int LeftX = Math.Max(collisionRectangle.Left / Constants.Level.TILE_SIZE, 0);
            int RightX = Math.Min(collisionRectangle.Right / Constants.Level.TILE_SIZE + 1, level.CurrentRoom.TileWidth);

            int centerTileX = Math.Min(Math.Max((int)position.X / Constants.Level.TILE_SIZE, 0), level.CurrentRoom.TileWidth - 1);
            int centerTileY = Math.Min(Math.Max((int)position.Y / Constants.Level.TILE_SIZE, 0), level.CurrentRoom.TileHeight - 1);
            TileCollisionType centerTileType = (TileCollisionType)level.CurrentRoom.TileMap[centerTileY][centerTileX];
            //Debug.WriteLine("centerTileX = " + centerTileX + ", centerTileY = " + centerTileY + ", centerTileType = " + centerTileType.ToString());

            // Iterate across all the rows of the TileMap visible within the frame of the camera
            for (int tileY = TopY; tileY < BottomY; tileY++)
            {
                // Iterate across all the columns of the TileMap visible within the frame of the camera
                for (int tileX = LeftX; tileX < RightX; tileX++)
                {
                    Tile tile = new Tile();
                    tile.type = (TileCollisionType)level.CurrentRoom.TileMap[tileY][tileX];

                    if (!(((centerTileType == TileCollisionType.SlopeSteepLeft
                        || centerTileType == TileCollisionType.SlopeGentle2Left)
                                && tileX == centerTileX + 1
                                && tileY == centerTileY
                        ) ||
                        ((centerTileType == TileCollisionType.SlopeSteepRight
                        || centerTileType == TileCollisionType.SlopeGentle2Right)
                                && tileX == centerTileX - 1
                                && tileY == centerTileY
                        )))
                    {
                        tile.rectangle = new Rectangle(tileX * Constants.Level.TILE_SIZE, tileY * Constants.Level.TILE_SIZE, Constants.Level.TILE_SIZE, Constants.Level.TILE_SIZE);
                        tiles.Add(tile);

                        // Registers tile into the static objects list
                        tile.RegisterTile();
                    }
                    
                }
            }
            return tiles;
        }

        public List<Tile> IntersectingTiles(Rectangle collisionRectangle)
        {
            return IntersectingTiles(collisionRectangle, Vector2.Zero);
        }

        private Color green = new Color(Constants.DebugValues.GREEN_R, Constants.DebugValues.GREEN_G, Constants.DebugValues.GREEN_B);
        public void DebugDraw(SpriteBatch spriteBatch)
        {
            foreach (var staticObj in manager.DebugStaticObjects)
            {
                GameDebug.Instance.DrawRectangle(spriteBatch, staticObj.GetHitBox(), green, Constants.DebugValues.GREEN_ALPHA);
            }

            foreach (var dynamicObj in manager.DynamicObjects)
            {
                if (dynamicObj.CollisionActive)
                {
                    GameDebug.Instance.DrawRectangle(spriteBatch, dynamicObj.GetHitBox(), Color.Red, Constants.DebugValues.RED_ALPHA);
                }
            }
        }

    }        
}