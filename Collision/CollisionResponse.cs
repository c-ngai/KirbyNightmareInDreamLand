using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Collision
{
    public enum CollisionSide { Top, Left, Right, Bottom };
    public sealed class CollisionResponse
    {
        public Dictionary<Tuple<CollisionType, CollisionType, CollisionSide>, Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>>> collisionMapping { get; private set; }
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
        public void RegisterCollision(CollisionType object1Type, CollisionType object2Type, CollisionSide side, Action<ICollidable, ICollidable, Rectangle> object1Command, Action<ICollidable, ICollidable, Rectangle> object2Command)
        {
            Tuple<CollisionType, CollisionType, CollisionSide> objects = new Tuple<CollisionType, CollisionType, CollisionSide>(object1Type, object2Type, side);
            Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>> commands = new Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>>(object1Command, object2Command);
            collisionMapping.Add(objects, commands);
        }
        // Overflow method, don't specify a side to register it for all sides
        public void RegisterCollision(CollisionType object1Type, CollisionType object2Type, Action<ICollidable, ICollidable, Rectangle> object1Command, Action<ICollidable, ICollidable, Rectangle> object2Command)
        {
            foreach (CollisionSide side in Enum.GetValues(typeof(CollisionSide)))
            {
                RegisterCollision(object1Type, object2Type, side, object1Command, object2Command);
            }
        }

        public void ExecuteCollision(ICollidable object1, ICollidable object2, CollisionSide side)
        {
            CollisionType key1 = object1.GetCollisionType();
            CollisionType key2 = object2.GetCollisionType();
            if(true) //ShouldCollide(key1, key2))
            {   
                //hand side that is being collided
                Tuple<CollisionType, CollisionType, CollisionSide> objects = new Tuple<CollisionType, CollisionType, CollisionSide>(key1, key2, side);

                if (collisionMapping.ContainsKey(objects))
                {
                    Rectangle intersection = Rectangle.Intersect(object1.GetHitBox(), object2.GetHitBox());
                    Tuple<Action<ICollidable, ICollidable, Rectangle>, Action<ICollidable, ICollidable, Rectangle>> commands = collisionMapping[objects];
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