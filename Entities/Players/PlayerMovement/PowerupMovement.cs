//using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{  
    public class PowerupMovement : PlayerMovement
    {
        public PowerupMovement(Vector2 pos) : base(pos)
        {}
        public override void Attack(Player kirby)
        {
            kirby.ChangePose(KirbyPose.Attacking);
        }
    }

}