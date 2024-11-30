using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Levels;
using Microsoft.VisualBasic;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public interface IPlayer : IEntity
    {
        public PlayerStateMachine state { get; }
        public PlayerMovement movement { get; }
        public int health { get; }
        public int lives { get; }
        public bool IsActive { get; }
        public bool DEAD { get; }
        public bool powerChangeAnimation { get; set; }
        void Crouch();
        void EndCrouch();
        Vector2 GetKirbyPosition();
        Vector2 GetKirbyVelocity();
        string GetKirbyType();
        string GetKirbyTypePause();
        void FillLives();
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
        void EnterDoor();
        void ResetAfterDoor();
        void StopAttacking();
        void SetDirectionLeft();
        void SetDirectionRight();
        void DropAbility();
        void ManualDropAbility();
        void ChangeToNormal();
        void ChangeToBeam();
        void ChangeToFire();
        void ChangeToSpark();
        void ChangeToProfessor();
        void BottomCollisionWithBlock(Rectangle intersection);
        void RightCollisionWithBlock(Rectangle intersection);
        void LeftCollisionWithBlock(Rectangle intersection);
        void BottomCollisionWithPlatform(Rectangle intersection);
        void TopCollisionWithBlock(Rectangle intersection);
        void EatEnemy(KirbyType kirbyType);
        KirbyType GetPowerUp();
        void ChangePose(KirbyPose pose);
    
    }
}