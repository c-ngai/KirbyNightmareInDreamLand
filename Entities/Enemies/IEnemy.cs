using KirbyNightmareInDreamLand.Sprites;
namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();

        Sprite EnemySprite { set; }
    }
}