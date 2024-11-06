using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using KirbyNightmareInDreamLand.Commands;
using Microsoft.Xna.Framework.Input;

namespace KirbyNightmareInDreamLand.Controllers
{
    // PAYTON: this is temporary, pls change it as needed -Mark
    public class Buttonmapping
    {
        // Keys enum
        public Buttons Button { get; set; }
        // ExecutionType enum
        public ExecutionType ExecutionType { get; set; }
        // Constructor info for respective command
        public ConstructorInfo CommandConstructorInfo { get; set; }
    }

}
