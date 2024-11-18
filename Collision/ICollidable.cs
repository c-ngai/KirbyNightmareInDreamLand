using System;
using KirbyNightmareInDreamLand.Actions;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public interface ICollidable
    {
        bool CollisionActive {get;}
        Rectangle GetHitBox();
        Vector2 GetPosition();
        CollisionType GetCollisionType();

    }
}
