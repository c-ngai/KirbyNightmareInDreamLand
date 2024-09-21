namespace MasterGame
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();

        Sprite EnemySprite { set; }
    }
}