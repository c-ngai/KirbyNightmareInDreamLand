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
        //add someobody in game to keep track of ALL the objects-- in game
        //when player is created level loader creates player and 
        //have object manager manage what gets added or deleted from collidable list

        //List and dictionary to be moved to object manager
        //dynamic objects: enemies, projectiles, player
        private List<ICollidable> DynamicObjects;
        //static: tiles
        private Dictionary<Tile, ICollidable> StaticObjects;
        private static CollisionDetection instance = new CollisionDetection();
        private bool CollisionOn = false; // for debug purposes
        //This will be changed once object manager is applied
        public static CollisionDetection Instance
        {
            get
            {
                return instance;
            }
        }
        public CollisionDetection()
        {
            DynamicObjects = new List<ICollidable>();
            StaticObjects = new Dictionary<Tile, ICollidable>();
        }
        //will be moved to object manager
        public void ResetDynamicCollisionBoxes()
        {
           DynamicObjects = new List<ICollidable>{};
        }

        // Register dynamic objects like Player, Enemy, etc.
         //will be moved to object manager
        public void RegisterDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Add(dynamicObj);
        }

        // Register static objects like Tiles.
         //will be moved to object manager
        public void RegisterStaticObject(Tile tile, ICollidable staticObj)
        {
            if(!StaticObjects.ContainsKey(tile)) StaticObjects.Add(tile, staticObj);
        }
         //will be moved to object manager
        public void RemoveDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Remove(dynamicObj);
        }
        
        private bool IsCloseEnough(ICollidable obj1, ICollidable obj2)
        {
            int distance =  obj1.GetHitBox().X - obj2.GetHitBox().X;
            int distance2 = obj1.GetHitBox().Y - obj2.GetHitBox().Y;
            int close = 55;

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
            Rectangle objectRectangle = object1.GetHitBox();
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
                //if(!DynamicObjects[i].CollisionActive)continue;
                for (int j = i + 1; j < DynamicObjects.Count; j++)
                {
                    //if(!DynamicObjects[j].CollisionActive) continue;
                    if(!IsCloseEnough(DynamicObjects[i],DynamicObjects[j])) continue;
                    if (DynamicObjects[i].GetHitBox().Intersects(DynamicObjects[j].GetHitBox()))
                    {
                        //go to collision response
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
                //if(dynamicObj.CollisionActive){
                    GameDebug.Instance.DrawRectangle(spriteBatch, dynamicObj.GetHitBox(), Color.Red);
                //}
            }

            IDictionaryEnumerator enumerator = StaticObjects.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ICollidable staticObject = (ICollidable)enumerator.Value;
                GameDebug.Instance.DrawRectangle(spriteBatch, staticObject.GetHitBox(), Color.Red);
            }
        }

    }
    //im thinking of adding a remove box method to take it off the list, instead of working thpugh
    //is active should that be in detection2          

}