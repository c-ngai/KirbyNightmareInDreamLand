using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class PoppyBrosJr : Enemy
    {
        private int hopCounter = 0; //number of hops

        public PoppyBrosJr(Vector2 startPosition) : base(startPosition, EnemyType.PoppyBrosJr)
        {
            stateMachine.ChangePose(EnemyPose.Hop);
            ChangeState(new PoppyBrosJrHopState(this));
            //TO-DO: spawn facing the direction kirby is in
            yVel = 0;
            xVel = Constants.PoppyBrosJr.MOVE_SPEED;
        }

        public override void Move()
        {
            // Handles x movement. Walking back and forth until left/right boundary
            if (stateMachine.IsLeft())
            {
                position.X -= xVel;
            }
            else
            {
                position.X += xVel;
            }
            Hop();
        }

        private void Hop()
        {
            //Handles Y movement and calculates oscillation of hops.
            hopCounter++;
            float t = (float)hopCounter / Constants.PoppyBrosJr.HOP_FREQUENCY;

            // Smooth hopping math
            yVel = (float)(Math.Sin(t * Math.PI * 2) * Constants.PoppyBrosJr.HOP_HEIGHT / 2);
            position.Y -= yVel;

            // Reset hop counter for cycle
            if (hopCounter >= Constants.PoppyBrosJr.HOP_FREQUENCY)
            {
                hopCounter = 0;
                enemySprite.ResetAnimation(); // Mark addition: since hop is a non-looping animation that we want to repeat but we already have that sprite, just call ResetAnimation on it.
            }
        }
    }
}
