using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public interface IEntity{
        void TakeDamage();
        void Attack();
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);

    }
}