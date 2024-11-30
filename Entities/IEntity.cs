using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Entities
{
    public interface IEntity{
        void TakeDamage(Rectangle intersection, Vector2 positionOfDamageSource);
        void Attack();
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);

    }
}