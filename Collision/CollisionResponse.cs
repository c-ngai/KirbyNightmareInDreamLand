using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Collision
{
    public enum CollisionSide {Top, Left, Right, Bottom};
    public class CollisionResponse
    {
        private Dictionary<Tuple<String, String, CollisionSide>, Tuple<Action<ICollidable>, Action<ICollidable>>> collisionMapping;
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
            collisionMapping = new Dictionary<Tuple<String, String, CollisionSide>, Tuple<Action<ICollidable>, Action<ICollidable>>> ();
        }

        // Creates string mappings of object types and collision side to determine object reactions
        public void RegisterCollision(String object1, String object2, CollisionSide side, Action<ICollidable> object1Command, Action<ICollidable> object2Command)
        {
            Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(object1, object2, side);
            Tuple<Action<ICollidable>, Action<ICollidable>> commands = new Tuple<Action<ICollidable>, Action<ICollidable>>(object1Command, object2Command);
            collisionMapping.Add(objects, commands);
        }

        public void ExecuteCollision(ICollidable object1, ICollidable object2, CollisionSide side)
        {
            // Determine object types to use as part of the key
            String key1 = object1.GetType().ToString();
            String key2 = object2.GetType().ToString();
            Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(key1, key2, side);

            if (collisionMapping.ContainsKey(objects) && collisionMapping[objects] != null) collisionMapping[objects].Item1(object2);
            if (collisionMapping.ContainsKey(objects) && collisionMapping[objects] != null) collisionMapping[objects].Item2(object1);
        }
    }
}
