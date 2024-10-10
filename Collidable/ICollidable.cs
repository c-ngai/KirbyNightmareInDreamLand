using System;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public interface ICollidable
    {
        Rectangle BoundingBox { get; }
        bool IsDynamic { get; } // Whether the object is dynamic (moving) or static.
        void UpdateBoundingBox(Vector2 pos);
        void OnCollision(ICollidable other);

    }
}
