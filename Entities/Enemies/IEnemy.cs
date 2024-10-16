using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();
        void Jump();
        void Fall();
        Sprite EnemySprite { set; }
    }
}