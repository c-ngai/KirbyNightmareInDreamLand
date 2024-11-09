using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Collision
{
    public enum CollisionSide { Top, Left, Right, Bottom };
    sealed class CollisionResponse
    {
        public Dictionary<Tuple<string, string, CollisionSide>, Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>>> collisionMapping { get; private set; }
        private static CollisionResponse instance = new CollisionResponse();
        public static CollisionResponse Instance
        {
            get
            {
                return instance;
            }
        }
        public bool DictEmpty()
        {
            return collisionMapping.Count == 0;
        }
        public CollisionResponse()
        {
            collisionMapping = new();
        }

        // Creates string mappings of object types and collision side to determine object reactions 
        // gets called by level loader
        public void RegisterCollision(string object1, string object2, CollisionSide side, Action<ICollidable, ICollidable, Rectangle> object1Command, Action<ICollidable, ICollidable, Rectangle> object2Command)
        {
            Tuple<string, string, CollisionSide> objects = new Tuple<string, string, CollisionSide>(object1, object2, side);
            Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>> commands = new Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>>(object1Command, object2Command);
            collisionMapping.Add(objects, commands);
        }
        private bool ShouldCollide(String obj1, String obj2)
        {
            if(obj2.Contains(obj1) || obj1.Contains(obj2) || obj1.Equals(obj2))
            {
                return false;
            }
            return true;
        }

        public void ExecuteCollision(ICollidable object1, ICollidable object2, CollisionSide side)
        {
            String key1 = object1.GetObjectType();
            String key2 = object2.GetObjectType();
            if(ShouldCollide(key1, key2))
            {   
                //hand side that is being collided
                Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(key1, key2, side);

                Rectangle intersection = Rectangle.Intersect(object1.GetHitBox(), object2.GetHitBox());
                Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>> commands = collisionMapping[objects];
                if (collisionMapping.ContainsKey(objects))
                {
                    if (commands.Item1 != null)
                    {
                        commands.Item1(object1, object2, intersection);
                    }
                    if (commands.Item2 != null)
                    {
                        commands.Item2(object1, object2, intersection);
                    }
                }
            } 
        }
    }
}