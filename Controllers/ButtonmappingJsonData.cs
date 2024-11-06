using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KirbyNightmareInDreamLand.Controllers
{
    // Class for deserializing each buttonmapping data in the .json file.
    public class ButtonmappingJsonData
    {
        public string Button { get; set; }
        public string ExecutionType { get; set; }
        public string Command { get; set; }
    }

}
