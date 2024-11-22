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
        void AccellerateTowards(Vector2 position);
        Sprite EnemySprite { set; }
        int Health { get; }
        bool Active { get; }
        bool IsBeingInhaled { get; }
        void IncrementFrameCounter();
        void ResetFrameCounter();
        void UpdateTexture();
        string GetStateString();
        void ChangePose(EnemyPose pose);
        void Dispose();
        KirbyType PowerType();

        void BottomCollisionWithBlock(Rectangle intersection);
        void TopCollisionWithBlock(Rectangle intersection);
        void BottomCollisionWithPlatform(Rectangle intersection);
    }
}