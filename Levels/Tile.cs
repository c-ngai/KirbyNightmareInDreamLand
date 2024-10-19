using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

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
