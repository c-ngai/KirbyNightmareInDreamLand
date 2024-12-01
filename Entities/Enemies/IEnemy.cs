using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();
        void Jump();
        void Move();
        void StopMoving();
        void DecelerateX(float deceleration);
        void AccellerateTowards(Vector2 position);
        Sprite EnemySprite { set; }
        int Health { get; }
        bool Active { get; }
        bool IsBeingInhaled { get; }
        void UpdateTexture();
        string GetStateString();
        Vector2 GetPosition();
        void ChangePose(EnemyPose pose);
        void Dispose();
        KirbyType PowerType();
        void SetDirection(bool facingLeft);
        void BottomCollisionWithBlock(Rectangle intersection);
        void TopCollisionWithBlock(Rectangle intersection);
        void LeftCollisionWithBlock(Rectangle intersection);
        void RightCollisionWithBlock(Rectangle intersection);
        void BottomCollisionWithPlatform(Rectangle intersection);
    }
}