﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using System;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public abstract class Enemy : IEnemy
    {
        protected Vector2 position; //Where enemy is drawn on screen
        protected int health; //Enemy health
        protected bool isDead;  //If enemy is dead
        protected bool isActive;
        protected Sprite enemySprite;   
        protected EnemyStateMachine stateMachine;
        protected IEnemyState currentState; // Current state of the enemy
        protected Vector2 leftBoundary; //Boundaries for where enemy will turn around on screen
        protected Vector2 rightBoundary;
        protected string oldState; //Previous state
        protected int frameCounter; // Frame counter for tracking state duration


        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            //Initialize all variables
            position = startPosition;
            health = 1;
            isDead = false;
            stateMachine = new EnemyStateMachine(type);
            leftBoundary = new Vector2(100, 100);
            rightBoundary = new Vector2(230, 100);
            oldState = string.Empty;
            currentState = new WaddleDooWalkingState(this); // Initialize with the walking state
            currentState.Enter();
            frameCounter = 0; 
        }

        public Vector2 Position
        {
            //Returns position on screen
            get { return position; }
            set { position = value; }
        }
 
        public Sprite EnemySprite
        {
            //Returns Sprite
            set { enemySprite = value; }
        }

        public int Health
        {
            get => health;
            set => health = value;
        }

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }

        public int FrameCounter
        {
            get { return frameCounter; }
        }

        public void IncrementFrameCounter()
        {
            frameCounter++;
        }

        // Method to reset the frame counter
        public void ResetFrameCounter()
        {
            frameCounter = 0;
        }

        public void UpdateTexture()
        {
            if (!stateMachine.GetStateString().Equals(oldState))
            {
                enemySprite = SpriteFactory.Instance.CreateSprite(stateMachine.GetSpriteParameters());
                oldState = stateMachine.GetStateString();
            }
        }

        public void ChangeState(IEnemyState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void TakeDamage()
        {
            currentState.TakeDamage(); // Delegate to current state
        }

        public void ChangeDirection()
        {
            currentState.ChangeDirection(); // Delegate to current state
        }

        // Public methods to interact with stateMachine
        public void ChangePose(EnemyPose pose)
        {
            stateMachine.ChangePose(pose);
        }

        public void ToggleDirection()
        {
            stateMachine.ChangeDirection();
        }

        public string GetStateString()
        {
            return stateMachine.GetStateString();
        }

        public virtual void Update(GameTime gameTime) // Change to virtual
        {
            if (!IsDead)
            {
                IncrementFrameCounter();
                currentState.Update(); // No parameters needed here
                UpdateTexture();
                enemySprite.Update();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draw if enemy is alive
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }

        public virtual void Attack() { }

        public abstract void Move();
       

    }
}
