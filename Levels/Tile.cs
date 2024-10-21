using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand.Levels
{
    public enum TileCollisionType
    {
        Air,
        Block,
        Platform,
        Water,
        SlopeSteepLeft,
        SlopeGentle1Left,
        SlopeGentle2Left,
        SlopeGentle2Right,
        SlopeGentle1Right,
        SlopeSteepRight
    }

    public struct Tile :ICollidable
    {
        public TileCollisionType type;
        public Rectangle rectangle;
        public bool CollisionActive { get; private set;}
        public void RegisterTile()
        {
            CollisionActive = true;
            ObjectManager.Instance.RegisterStaticObject(this);
        }

        public void RemoveTile()
        {
            CollisionActive = false;
            ObjectManager.Instance.RemoveStaticObject(this);
        }
        public Rectangle GetHitBox()
        {
            return rectangle;
        }

        public string GetObjectType()
        {
            return ObjectManager.Instance.tileTypes[(int)type];
        }
    }

}
