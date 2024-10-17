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
        // gets called by level loader
        public void RegisterCollision(String object1, String object2, CollisionSide side, Action object1Command, Action object2Command)
        {
            Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(object1, object2, side);
            Tuple<Action, Action> commands = new Tuple<Action, Action>(object1Command, object2Command);
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
            String key1 = object1.GetType().ToString();
            String key2 = object2.GetType().ToString();
            //hand side that is being collided
            Tuple<String, String, CollisionSide> objects = new Tuple<String, String, CollisionSide>(key1, key2, side);

            // IS THERE A WAY I CAN CALL INSTANCE ACTIONS? 
            //pass in parameters! 
            if (collisionMapping[objects] != null) collisionMapping[objects].Item1();
            if (collisionMapping[objects] != null) collisionMapping[objects].Item2();
        }
    }
}