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
        private bool CollisionOn = true; // for debug purposes
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
            //check how many obejcts are taken out by these checks
            int distance =  obj1.GetHitBox().X - obj2.GetHitBox().X;
            int distance2 = obj1.GetHitBox().Y - obj2.GetHitBox().Y; //take off the Y 
            int close = 55;

            // If the distance between the objects is less than the sum of their radii, they're close enough to check further
            return distance<close && distance2<close;
        }

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
            foreach (var dynamicObj in DynamicObjects)
            {
                List<Tile> nearbyTiles = Game1.Instance.level.IntersectingTiles(dynamicObj.GetHitBox());
                foreach (Tile tile in nearbyTiles)
                {
                    if (dynamicObj.GetHitBox().Intersects(tile.rectangle))
                    {
                        Rectangle intersection = Rectangle.Intersect(dynamicObj.GetHitBox(), tile.rectangle);

                        CollisionSide side = DetectCollisionSide(dynamicObj.GetHitBox(), intersection);

                        //CollisionResponse.Instance.ExecuteCollision(dynamicObj, StaticObjects[tile], side);
                    }
                }
            }
        }
        public void DynamicCollisionCheck()
        {
            for (int i = 0; i < DynamicObjects.Count; i++)
            {
                if(!DynamicObjects[i].CollisionActive)continue;
                for (int j = i + 1; j < DynamicObjects.Count; j++)
                {
                    //run and time and comment out close enough to check if it improved performance
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
                //DynamicObjects.RemoveAll(obj => !obj.CollisionActive);
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
                if(dynamicObj.CollisionActive){
                    GameDebug.Instance.DrawRectangle(spriteBatch, dynamicObj.GetHitBox(), Color.Red);
                }
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