using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Entities
{
    public interface IEntity{
        void TakeDamage();
        void Attack();
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);

    }
}