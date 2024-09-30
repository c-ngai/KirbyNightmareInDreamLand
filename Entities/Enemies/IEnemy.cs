using MasterGame.Sprites;
namespace MasterGame.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();

        Sprite EnemySprite { set; }
    }
}