using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KirbyNightmareInDreamLand.Controllers
{
    // Class for deserializing each keymapping data in the .json file.
    public class KeymappingJsonData
    {
        public string Key { get; set; }
        public string ExecutionType { get; set; }
        public string Command { get; set; }
    }

}
