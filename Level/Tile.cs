using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand
{

    public enum TileCollisionType
    {
        Air = 0,
        Block = 1,
        Platform = 2,
        Water = 3,
        SlopeSteepLeft = 4,
        SlopeGentle1Left = 5,
        SlopeGentle2Left = 6,
        SlopeGentle2Right = 7,
        SlopeGentle1Right = 8,
        SlopeSteepRight = 9
    }

    public struct Tile :ICollidable
    {
        public TileCollisionType type;
        public Rectangle rectangle;
        public void RegisterTile()
        {
            CollisionDetection.Instance.RegisterStaticObject(this, this);
        }
        public Rectangle GetHitBox()
        {
            return rectangle;
        }
    }

}
