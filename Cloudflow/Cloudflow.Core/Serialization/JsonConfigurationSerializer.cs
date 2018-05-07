using System;
using Cloudflow.Core.ExtensionManagement;
using Newtonsoft.Json;

namespace Cloudflow.Core.Serialization
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
