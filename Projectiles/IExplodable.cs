﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Sprites;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public interface IExplodable
    {
        public void BottomBlockCollision(Rectangle intersection);

        public void RightBlockCollision(Rectangle intersection);

        public void LeftBlockCollision(Rectangle intersection);

    }
}
