using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public interface IProjectile
    {
        void Update(); // Update the position of the projectile
        void Draw(SpriteBatch spriteBatch);   // Render the projectile on the screen
        Vector2 Position { get; set; } // Position of the projectile
        Vector2 Velocity { get; set; } // Velocity or direction of the projectile
    }
}