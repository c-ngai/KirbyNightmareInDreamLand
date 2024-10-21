using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public interface IProjectile
    {
        void Update(); // Update the position of the projectile
        void Draw(SpriteBatch spriteBatch);   // Render the projectile on the screen
        bool IsDone();
        void EndAttack();
        Vector2 Position { get; } // Position of the projectile
        Vector2 Velocity { get;} // Velocity or direction of the projectile
    }
}