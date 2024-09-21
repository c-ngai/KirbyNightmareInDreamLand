using Microsoft.Xna.Framework;
namespace MasterGame
{
    public interface IPlayer:IEntity
    {
        Vector2 Position { get; set; }
        Sprite PlayerSprite { set; }
        void MoveRight();
        void UpdateTexture();
    }
}