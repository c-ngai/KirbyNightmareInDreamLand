using Microsoft.Xna.Framework;
namespace MasterGame
{
    public interface IPlayer:IEntity
    {
        Sprite PlayerSprite { set; }
        void Crouch();
        void EndCrouch();
        void Float();
        void Inhale();
        void JumpY();
        void MoveRight();
        void MoveLeft();
        void RunRight();
        void RunLeft();
        void StopMoving();
        void SetDirectionLeft();
        void SetDirectionRight();
        void UpdateTexture();
    }
}