using System;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public interface ICollidable
    {
        //bool CollisionActive {get;}
        Rectangle GetHitBox();

    }
}
