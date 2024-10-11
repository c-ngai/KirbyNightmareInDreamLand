using System;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public interface ICollidable
    {
        Rectangle BoundingBox { get; }
        bool IsDynamic { get; } // Whether the object is dynamic (moving) or static.
        bool IsActive { get; }
        void DestroyHitBox();
        void EnableHitBox();
        void UpdateBoundingBox(Vector2 pos);
        void OnCollision(ICollidable other);

    }
}
