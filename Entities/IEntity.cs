using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame.Entities
{
    public interface IEntity{
        void TakeDamage();
        void Attack();
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);

    }
}