using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand
{
    public class CollisionManager
    {
        //dynamic objects: enemies, projectiles, player
        private List<ICollidable> DynamicObjects;
        //static: tiles
        private List<ICollidable> StaticObjects;
        private static CollisionManager instance = new CollisionManager();
        public static CollisionManager Instance
        {
            get
            {
                return instance;
            }
        }
        public CollisionManager()
        {
            DynamicObjects = new List<ICollidable>();
            StaticObjects  = new List<ICollidable>();
        }

        // Register dynamic objects like Player, Enemy, etc.
        public void RegisterDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Add(dynamicObj);
        }

        // Register static objects like Tiles.
        public void RegisterStaticObject(ICollidable staticObj)
        {
            StaticObjects.Add(staticObj);
        }

        // Method to handle collision detection
        public void CheckCollisions()
        {
            // Check dynamic objects against static objects
            foreach (var dynamicObj in DynamicObjects)
            {
                foreach (var staticObj in StaticObjects)
                {
                    if (dynamicObj.BoundingBox.Intersects(staticObj.BoundingBox))
                    {
                        dynamicObj.OnCollision(staticObj);
                        staticObj.OnCollision(dynamicObj);
                    }
                }
            }

            // Check dynamic objects against each other, avoiding duplicate tests
            for (int i = 0; i < DynamicObjects.Count; i++)
            {
                for (int j = i + 1; j < DynamicObjects.Count; j++)
                {
                    if (DynamicObjects[i].BoundingBox.Intersects(DynamicObjects[j].BoundingBox))
                    {
                        System.Console.WriteLine(DynamicObjects[i]);
                        DynamicObjects[i].OnCollision(DynamicObjects[j]);
                        DynamicObjects[j].OnCollision(DynamicObjects[i]);
                    }
                }
            }
        }

    }               

}