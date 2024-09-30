using Microsoft.Xna.Framework;
using MasterGame.Sprites;
namespace MasterGame.Entities.Players
{
    public interface IPlayer : IEntity
    {
        Sprite PlayerSprite { set; }
        void Crouch();
        void EndCrouch();
        Vector2 GetKirbyPosition();
        string GetKirbyType();
        bool IsLeft();
        bool IsFloating();
        void Slide();
        void Float();
        void Jump();
        void MoveRight();
        void MoveLeft();
        void RunRight();
        void RunLeft();
        void StopMoving();
        void SetDirectionLeft();
        void SetDirectionRight();
        void ChangeToNormal();
        void ChangeToBeam();
        void ChangeToFire();
        void ChangeToSpark();
        void UpdateTexture();
    }
}