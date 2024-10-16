using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();

        Sprite EnemySprite { set; }
    }
}