using System;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Commands;

namespace KirbyNightmareInDreamLand.Collision
{
    public enum CollisionSide {Top, Left, Right, Bottom};
    public class CollisionResponse
    {
        //level loader times register collision?
        //not  abad idea to and them the collision rectangle, most ocllision handlers will want to know overlap
        //or have it in constructors 
        public Dictionary<Tuple<string, string, CollisionSide>, Tuple<Action<ICollidable>, Action<ICollidable>>> collisionMapping { get; private set; }
        private static CollisionResponse instance = new CollisionResponse();
        public static CollisionResponse Instance
        {
            get
            {
                return instance;
            }
        }

        public CollisionResponse()
        {
            collisionMapping = new Dictionary<Tuple<string, string, CollisionSide>, Tuple<Action<ICollidable>, Action<ICollidable>>>();
        }

        // Creates string mappings of object types and collision side to determine object reactions 
        // gets called by level loader
        public void RegisterCollision(string object1, string object2, CollisionSide side, Action<ICollidable> object1Command, Action<ICollidable> object2Command)
        {
            Tuple<string, string, CollisionSide> objects = new Tuple<string, string, CollisionSide>(object1, object2, side);
            Tuple<Action<ICollidable>, Action<ICollidable>>commands = new Tuple<Action<ICollidable>, Action<ICollidable>>(object1Command, object2Command);
            collisionMapping.Add(objects, commands);
        }

        public void ExecuteCollision(ICollidable object1, ICollidable object2, CollisionSide side)
        {
            // Determine object types to use as part of the key
            //effective but not optimal, object.GetCollisionType()
            //per concrete class
            //feel free to let different objects have different collision types
            //i gues for tile we could have it? i feel like its easier to just make the tile hit box smaller
            //ans stop the intersection instead of allowing there to be one
            string key1 = object1.GetObjectType();
            string key2 = object2.GetObjectType();
            //hand side that is being collided
            Tuple<string, string, CollisionSide> objects = new Tuple<string, string, CollisionSide>(key1, key2, side);


            collisionMapping[objects]?.Item1(object2);
            collisionMapping[objects]?.Item2(object1);
        }
    }
}