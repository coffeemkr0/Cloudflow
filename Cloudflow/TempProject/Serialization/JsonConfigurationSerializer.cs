using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TempProject.ExtensionService;

namespace TempProject.Serialization
{
    public class JsonConfigurationSerializer : IConfigurationSerializer
    {
        public object Deserialize(string configurationData, Type configurationType)
        {
            return JsonConvert.DeserializeObject(configurationData, configurationType);
        }

        public string SerializeToString(object configuration)
        {
            return JsonConvert.SerializeObject(configuration);
        }
    }
}
