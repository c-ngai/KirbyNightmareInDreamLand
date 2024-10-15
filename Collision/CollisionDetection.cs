using System;
using System.Collections;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand
{
    public class CollisionDetection
    {
        //dynamic objects: enemies, projectiles, player
        private List<ICollidable> DynamicObjects;
        //static: tiles
        private Dictionary<Tile, ICollidable> StaticObjects;
        private static CollisionDetection instance = new CollisionDetection();
        private bool CollisionOn = false; // for debug purposes
        public static CollisionDetection Instance
        {
            get
            {
                return instance;
            }
        }

        //what im thinking is:
        //the repsonder calls each objects handler which has the method with parameters takend care of
        //based on the dictionary
        //yeah
        //just make it work with the collision classes

        public CollisionDetection()
        {
            DynamicObjects = new List<ICollidable>();
            StaticObjects = new Dictionary<Tile, ICollidable>();
        }
        public void ResetDynamicCollisionBoxes()
        {
            foreach (var dynamObject in DynamicObjects)
            {
                dynamObject.DestroyHitBox();
            }
            DynamicObjects.RemoveAll(obj => !obj.IsActive);
        }

        // Register dynamic objects like Player, Enemy, etc.
        //
        public void RegisterDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Add(dynamicObj);
        }

        // Register static objects like Tiles.
        public void RegisterStaticObject(Tile tile, ICollidable staticObj)
        {
            if(!StaticObjects.ContainsKey(tile)) StaticObjects.Add(tile, staticObj);
        }
        private bool IsCloseEnough(ICollidable obj1, ICollidable obj2)
        {
            int distance =  obj1.BoundingBox.X - obj2.BoundingBox.X;
            int distance2 = obj1.BoundingBox.Y - obj2.BoundingBox.Y;
            int close = 40;

            // If the distance between the objects is less than the sum of their radii, they're close enough to check further
            return distance<close && distance2<close;
        }

        // Method to handle collision detection
        public CollisionSide CheckSide(Rectangle intersection, ICollidable object1)
        {
            // Determine positions for all corners of the intersection
            int intersectionTopAndBottomRightCornerX = intersection.X + intersection.Width;
            int intersectionTopRightCornerY = intersection.Y;
            int intersectionBottomLeftCornerX = intersection.X;
            int intersectionBottomLeftAndRightCornerY = intersection.Y - intersection.Height;

            // Determine x-y positions for all corners of the object's hit box
            Rectangle objectRectangle = object1.BoundingBox;
            int objectTopAndBottomRightCornerX = objectRectangle.X + objectRectangle.Width;
            int objectTopRightCornerY = objectRectangle.Y;
            int objectBottomLeftCornerX = objectRectangle.X;
            int objectBottomLeftandRightCornerY = objectRectangle.Y - objectRectangle.Height;

            // Calculates the length of overlap on an edge when the intersection touches the edge
            double topOverlap = 0, bottomOverlap = 0, rightOverlap = 0, leftOverlap = 0;
            if (intersection.Y == objectRectangle.Y)
            {
                topOverlap = intersection.Width;
            }
            if (intersectionBottomLeftAndRightCornerY == objectBottomLeftandRightCornerY)
            {
                bottomOverlap = intersection.Width;
            }
            if (intersection.X == objectRectangle.X)
            {
                leftOverlap = intersection.Height;
            }
            if (intersectionTopAndBottomRightCornerX == objectTopAndBottomRightCornerX)
            {
                rightOverlap = intersection.Height;
            }

            // TODO: create a constant for 4: the number of sides; Note this cannot be changed for this to work correctly
            double[] percentageOfIntersection = new double[Constants.HitBoxes.SIDES];
            percentageOfIntersection[(int)CollisionSide.Top] = topOverlap / objectRectangle.Width;
            percentageOfIntersection[(int)CollisionSide.Bottom] = bottomOverlap / objectRectangle.Width;
            percentageOfIntersection[(int)CollisionSide.Right] = rightOverlap / objectRectangle.Height;
            percentageOfIntersection[(int)CollisionSide.Left] = leftOverlap / objectRectangle.Height;

            // Finds the side that has the largest intersection percentage and returns it
            double largestIntersection = 0;
            int index = 0;
            for (int i = 0; i < percentageOfIntersection.Length; i++)
            {
                if (largestIntersection < percentageOfIntersection[i])
                {
                    largestIntersection = percentageOfIntersection[i];
                    index = i;
                }
            }
            return (CollisionSide)index;
        }
        public void StaticCollisionCheck()
        {
            //foreach (var dynamicObj in DynamicObjects)
            //{
            //    List<Tile> nearbyTiles = Game1.Instance.level.IntersectingTiles(dynamicObj.BoundingBox);
            //    foreach (Tile tile in nearbyTiles)
            //    {
            //        if (dynamicObj.BoundingBox.Intersects(tile.rectangle))
            //        {
            //            Rectangle intersection = Rectangle.Intersect(dynamicObj.BoundingBox, tile.rectangle);

            //            CollisionSide side = CheckSide(intersection, dynamicObj);

            //            CollisionResponse.Instance.ExecuteCollision(dynamicObj, StaticObjects[tile], side);
            //        }
            //    }
            //}
        }
        public void DynamicCollisionCheck()
        {
            for (int i = 0; i < DynamicObjects.Count; i++)
            {
                for (int j = i + 1; j < DynamicObjects.Count; j++)
                {
                    if (DynamicObjects[i].IsActive && DynamicObjects[j].IsActive)
                    {
                        if (!DynamicObjects[i].IsActive || !DynamicObjects[j].IsActive) continue;
                        //make function to get type to check if its enemies
                        if(!IsCloseEnough(DynamicObjects[i],DynamicObjects[j])) continue;
                        if (DynamicObjects[i].BoundingBox.Intersects(DynamicObjects[j].BoundingBox))
                        {
                            DynamicObjects[i].OnCollision(DynamicObjects[j]);
                            DynamicObjects[j].OnCollision(DynamicObjects[i]);
                        }
                    }

                }
            }
        }

        // Method to handle collision detection
        public void CheckCollisions()
        {
            if (CollisionOn)
            {
                //DynamicObjects.RemoveAll(obj => !obj.IsActive);
                // Check dynamic objects against static objects

                StaticCollisionCheck();
                // Check dynamic objects against each other, avoiding duplicate tests
                //add check for enemies not colliding with each other?? probably within their ouwn class
                DynamicCollisionCheck();
            }
        }
        // i dont think he wants that many command classes
        //otherwise he would have mentioned it
        public void DebugDraw(SpriteBatch spriteBatch)
        {
            foreach (var dynamicObj in DynamicObjects)
            {
                if (dynamicObj.IsActive)
                {
                    GameDebug.Instance.DrawRectangle(spriteBatch, dynamicObj.BoundingBox, Color.Red);
                }
            }

            IDictionaryEnumerator enumerator = StaticObjects.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ICollidable staticObject = (ICollidable)enumerator.Value;
                if (staticObject.IsActive)
                {
                    GameDebug.Instance.DrawRectangle(spriteBatch, staticObject.BoundingBox, Color.Red);
                }
            }
        }

    }
    //im thinking of adding a remove box method to take it off the list, instead of working thpugh
    //is active should that be in detection2          

}