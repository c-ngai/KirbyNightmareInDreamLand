using Microsoft.Xna.Framework;
namespace MasterGame
{
    public interface IPlayer:IEntity
    {
        Sprite PlayerSprite { set; }
        void MoveRight();
        void MoveLeft();
        void StopMoving();
        void SetDirectionLeft();
        void SetDirectionRight();
        void UpdateTexture();
    }
}