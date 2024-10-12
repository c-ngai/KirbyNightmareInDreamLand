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
        // Broad-phase distance check using bounding circles
        private float GetBoundingRadius(Rectangle boundingBox)
        {
            // Use Pythagoras theorem to get half the diagonal as the radius
            float width = boundingBox.Width;
            float height = boundingBox.Height;
            return (float)Math.Sqrt(width * width + height * height) / 2;
        }
        // Calculate the center of a bounding box
        private Vector2 GetCenter(Rectangle boundingBox)
        {
            return new Vector2(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
        }
        private bool IsCloseEnough(ICollidable obj1, ICollidable obj2)
        {
            // Get the center points of the objects
            Vector2 center1 = GetCenter(obj1.BoundingBox);
            Vector2 center2 = GetCenter(obj2.BoundingBox);

            // Calculate the distance between the two objects' centers
            float distance = Vector2.Distance(center1, center2);

            // Define a rough collision radius (half the diagonal length of the bounding box)
            float radius1 = GetBoundingRadius(obj1.BoundingBox);
            float radius2 = GetBoundingRadius(obj2.BoundingBox);

            // If the distance between the objects is less than the sum of their radii, they're close enough to check further
            return distance < (radius1 + radius2);
        }

        // Method to handle collision detection
        public void CheckCollisions()
        {
            DynamicObjects.RemoveAll(obj => !obj.IsActive);
            // Check dynamic objects against static objects
            foreach (var dynamicObj in DynamicObjects)
            {
                foreach (var staticObj in StaticObjects)
                {
                    if (!dynamicObj.IsActive) continue;
                    if (dynamicObj.BoundingBox.Intersects(staticObj.BoundingBox))
                    {
                        dynamicObj.OnCollision(staticObj);
                        staticObj.OnCollision(dynamicObj);
                    }
                }
            }

            // Check dynamic objects against each other, avoiding duplicate tests
            //add check for enemies not colliding with each other?? probably within their ouwn class
            for (int i = 0; i < DynamicObjects.Count; i++)
            {
                for (int j = i + 1; j < DynamicObjects.Count; j++)
                {
                    if (DynamicObjects[i].IsActive && DynamicObjects[j].IsActive) 
                    {
                        if (DynamicObjects[i].BoundingBox.Intersects(DynamicObjects[j].BoundingBox))
                        {
                            DynamicObjects[i].OnCollision(DynamicObjects[j]);
                            DynamicObjects[j].OnCollision(DynamicObjects[i]);
                        }
                    }
                    
                }
            }
        }

    }               

}