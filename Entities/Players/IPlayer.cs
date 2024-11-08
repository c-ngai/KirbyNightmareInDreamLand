using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.StateMachines;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public interface IPlayer : IEntity
    {
        public PlayerStateMachine state { get; }
        public PlayerMovement movement { get; }
        void Crouch();
        void EndCrouch();
        void AttackPressed();
        Vector2 GetKirbyPosition();
        string GetKirbyType();
        void GoToRoomSpawn();
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
        void StopAttacking();
        void SetDirectionLeft();
        void SetDirectionRight();
        void ChangeToNormal();
        void ChangeToBeam();
        void ChangeToFire();
        void ChangeToSpark();
        void BottomCollisionWithBlock(Rectangle intersection);
        void RightCollisionWithBlock(Rectangle intersection);
        void LeftCollisionWithBlock(Rectangle intersection);
        void BottomCollisionWithPlatform(Rectangle intersection);
        void SwallowEnemy();
    
    }
}