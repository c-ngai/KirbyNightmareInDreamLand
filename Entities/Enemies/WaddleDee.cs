using Microsoft.Xna.Framework;
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

            UpdateTexture();
            currentState = new WaddleDeeWalkingState(this);
            //TO-DO: spawn facing the direction kirby is in
            stateMachine.ChangeDirection();
        }

        public override void Move()
        {
            //X movement logic. Moves until boundaries
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.WaddleDee.MOVE_SPEED;
                //change direction only if collide with block left/right
            }
            else
            {
                position.X += Constants.WaddleDee.MOVE_SPEED;
                
            }
        }
    }
}