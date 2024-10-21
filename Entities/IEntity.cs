using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Entities
{
    public interface IEntity{
        void TakeDamage(Rectangle intersection);
        void Attack();
        void Update(GameTime gametime);
        String GetCollisionType();
        void Draw(SpriteBatch spriteBatch);

    }
}