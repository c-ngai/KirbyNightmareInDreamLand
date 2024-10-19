using System;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand
{
    public class CollisionDetection
    {
        private ObjectManager manager { get; }
        private CollisionResponse response { get; }
        private Game1 game { get; }
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

        public void StaticCollisionCheck()
        {
            foreach (var dynamicObj in manager.DynamicObjects)
            {
                // Registers all relevant tiles
                Game1.Instance.Level.IntersectingTiles(dynamicObj.GetHitBox());
                foreach (var staticObj in manager.StaticObjects)
                {
                    if (dynamicObj.GetHitBox().Intersects(staticObj.GetHitBox()))
                    {
                        Rectangle intersection = Rectangle.Intersect(dynamicObj.GetHitBox(), staticObj.GetHitBox());

                        CollisionSide side = DetectCollisionSide(dynamicObj.GetHitBox(), intersection);

                        string type1 = dynamicObj.GetObjectType();

                        string type2 = staticObj.GetObjectType();
                        Tuple<string, string, CollisionSide> key = new Tuple<string, string, CollisionSide>(type1, type2, side);
                        if (response.collisionMapping.ContainsKey(key))
                        {
                            response.ExecuteCollision(dynamicObj, staticObj, side);
                        }
                    }
                }
            }
        }
        public void DynamicCollisionCheck()
        {
            for (int i = 0; i < manager.DynamicObjects.Count; i++)
            {
                if (!manager.DynamicObjects[i].CollisionActive) continue;
                for (int j = i + 1; j < manager.DynamicObjects.Count; j++)
                {
                    //run and time and comment out close enough to check if it improved performance
                    if (!IsCloseEnough(manager.DynamicObjects[i], manager.DynamicObjects[j])) continue;
                    if (manager.DynamicObjects[i].GetHitBox().Intersects(manager.DynamicObjects[j].GetHitBox()))
                    {
                        Rectangle intersection = Rectangle.Intersect(manager.DynamicObjects[i].GetHitBox(), manager.DynamicObjects[j].GetHitBox());

                        CollisionSide side = DetectCollisionSide(manager.DynamicObjects[i].GetHitBox(), intersection);

                        string type1 = manager.DynamicObjects[i].GetObjectType();
                        string type2 = manager.DynamicObjects[j].GetObjectType();
                        Tuple<string, string, CollisionSide> key = new Tuple<string, string, CollisionSide>(type1, type2, side);
                        if (response.collisionMapping.ContainsKey(key)) response.ExecuteCollision(manager.DynamicObjects[i], manager.DynamicObjects[j], side);
                    }
                }
            }
        }

            // Method to handle collision detection
            public void CheckCollisions()
        {
            if (CollisionOn)
            {
                //DynamicObjects.RemoveAll(obj => !obj.CollisionActive);
                // Check dynamic objects against static objects

                StaticCollisionCheck();
                // Check dynamic objects against each other, avoiding duplicate tests
                //add check for enemies not colliding with each other?? probably within their ouwn class
                DynamicCollisionCheck();
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            foreach (var dynamicObj in manager.DynamicObjects)
            {
                if (dynamicObj.CollisionActive)
                {
                    GameDebug.Instance.DrawRectangle(spriteBatch, dynamicObj.GetHitBox(), Color.Red);
                }
            }

            foreach (var staticObj in manager.StaticObjects)
            {
                GameDebug.Instance.DrawRectangle(spriteBatch, staticObj.GetHitBox(), Color.Red);
            }
        }

    }        
}