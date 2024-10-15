using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public interface IJumpable
    {
        void Jump();
        bool IsJumping { get; }
    }
}
