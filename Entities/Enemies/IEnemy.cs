﻿using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public interface IEnemy : IEntity
    {
        void ChangeDirection();
        void Jump();
        void Fall();
        void Move();
        Sprite EnemySprite { set; }
        int Health { get; }
        bool IsDead { get; }
        void IncrementFrameCounter();
        void ResetFrameCounter();
        void UpdateTexture();
        string GetStateString();
        public void ChangePose(EnemyPose pose);
    }
}