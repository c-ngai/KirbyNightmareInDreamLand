using System;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Commands;

namespace KirbyNightmareInDreamLand.Collision
{
    public enum CollisionSide {Top, Left, Right, Bottom};
    public class CollisionResponse
    {
        private Dictionary<Tuple<String, String, CollisionSide>, Tuple<Action, Action>> collisionMapping;
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
            collisionMapping = new Dictionary<Tuple<String, String, CollisionSide>, Tuple<Action, Action>>();
        }

        // Creates string mappings of object types and collision side to determine object reactions
        public void RegisterCollision(String object1, String object2, CollisionSide side, Action object1Command, Action object2Command)
        {
            Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(object1, object2, side);
            Tuple<Action, Action> commands = new Tuple<Action, Action>(object1Command, object2Command);
            collisionMapping.Add(objects, commands);
        }

        public void ExecuteCollision(ICollidable object1, ICollidable object2, CollisionSide side)
        {
            // Determine object types to use as part of the key
            String key1 = object1.GetType().ToString();
            String key2 = object2.GetType().ToString();
            Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(key1, key2, side);

            // IS THERE A WAY I CAN CALL INSTANCE ACTIONS?
            if (collisionMapping[objects] != null) collisionMapping[objects].Item1();
            if (collisionMapping[objects] != null) collisionMapping[objects].Item2();
        }
    }
}
