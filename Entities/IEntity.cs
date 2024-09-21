using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public interface IEntity{
        void TakeDamage();
        void Attack();
        void Update();
        void Draw();

    }
}