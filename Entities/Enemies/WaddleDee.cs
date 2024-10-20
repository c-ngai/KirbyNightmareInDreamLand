using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDee : Enemy
    {
        //Keep track of current frame
        //private int frameCounter = 0;
        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            //Set pose and sprite
            Debug.WriteLine("Hello from Waddle Dee:" + startPosition);

            UpdateTexture();
            currentState = new WaddleDeeWalkingState(this);
            //TO-DO: spawn facing the direction kirby is in
            stateMachine.ChangeDirection();
            xVel = Constants.WaddleDee.MOVE_SPEED;
        }

        public override void Move()
        {
            if (IsFalling) // Prioritize falling if in the air
            {
                Fall();
            }
            else
            {
                // X movement logic. Moves until boundaries
                if (stateMachine.IsLeft())
                {
                    position.X -= xVel;
                }
                else
                {
                    position.X += xVel;
                }
            }

        }
    }
}