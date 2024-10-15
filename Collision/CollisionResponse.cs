using System;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Commands;

namespace KirbyNightmareInDreamLand.Collision
{
    public enum CollisionSide {Top, Left, Right, Bottom};
    public class CollisionResponse
    {
        private Dictionary<Tuple<Object, Object, CollisionSide>, Tuple<ICommand, ICommand>> collisionMapping;
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
            collisionMapping = new Dictionary<Tuple<Object, Object, CollisionSide>, Tuple<ICommand, ICommand>>();
        }

        // Do we want to consider making projectile entities: it does not meet our IEntity method needs
        // Is it ok to keep parameters as objects and implement type checking before calling these?
        public void RegisterCollision(Object object1, Object object2, CollisionSide side, ICommand object1Command, ICommand object2Command)
        {
            Tuple<Object, Object, CollisionSide> objects = new Tuple<Object, Object, CollisionSide>(object1, object2, side);
            Tuple<ICommand, ICommand> commands = new Tuple<ICommand, ICommand>(object1Command, object2Command);
            collisionMapping.Add(objects, commands);
        }

        public void ExecuteCollision(Object object1, Object object2, CollisionSide side)
        {
            Tuple<Object, Object, CollisionSide> objects = new Tuple<Object, Object, CollisionSide>(object1, object2, side);
            collisionMapping[objects].Item1.Execute();
            collisionMapping[objects].Item2.Execute();
        }
    }
}
