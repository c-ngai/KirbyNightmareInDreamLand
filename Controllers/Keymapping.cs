using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using KirbyNightmareInDreamLand.Commands;
using Microsoft.Xna.Framework.Input;

namespace KirbyNightmareInDreamLand.Controllers
{
    // PAYTON: this is temporary, pls change it as needed -Mark
    public class Keymapping
    {
        // Keys enum
        public Keys Key { get; set; }
        // ExecutionType enum
        public ExecutionType ExecutionType { get; set; }
        // Constructor info for respective command
        public ConstructorInfo CommandConstructorInfo { get; set; }
        // PlayerIndex int?
        public int? PlayerIndex { get; set; }
    }

}
