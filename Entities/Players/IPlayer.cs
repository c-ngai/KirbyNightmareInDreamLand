using Microsoft.Xna.Framework;
namespace MasterGame
{
    public interface IPlayer:IEntity
    {
        Sprite PlayerSprite { set; }
        void Crouch();
        void EndCrouch();
        void Slide();
        void Float();
        void Inhale();
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