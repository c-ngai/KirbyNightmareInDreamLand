using System;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class MouthfulMovement : PlayerMovement
    {
        public MouthfulMovement(Vector2 pos) : base(pos){}

        public override void Walk(bool isLeft)
        {
            //does nothing
        }

        public override void Run(bool isLeft)
        {
            //does nothing
        }
        //starts kirby is sliding
        public override void Attack(Player kirby)
        {
        }

        public override void Jump(bool isLeft)
        {
            Slide(isLeft);
        }

        public void AdjustSlide(Player kirby)
        {
        }
        public override void Adjust(Player kirby)
        {
            AdjustSlide(kirby);
            //if kirby collides with a wall while crouching the slide ends
            //AdjustX(kirby);  // Turning this off temporarily  -Mark
            AdjustY(kirby);
        }

    }
}