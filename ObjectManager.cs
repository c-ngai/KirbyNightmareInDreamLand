using System.Collections.Generic;

namespace KirbyNightmareInDreamLand
{
    public class ObjectManager
    {
        private List<ICollidable> DynamicObjects;
        private static ObjectManager instance = new ObjectManager();
        public static ObjectManager Instance
        {
            get
            {
                return instance;
            }
        }
        public ObjectManager()
        {
            DynamicObjects = new List<ICollidable>();
        }

        #region Collision
        public void RemoveDymanicObject(ICollidable obj)
        {
            DynamicObjects.Remove(obj);
        }
        #endregion
    }
}