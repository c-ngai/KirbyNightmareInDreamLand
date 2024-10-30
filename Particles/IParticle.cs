using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Particles
{
    public interface IParticle
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);
        bool isDone();
    }
}
