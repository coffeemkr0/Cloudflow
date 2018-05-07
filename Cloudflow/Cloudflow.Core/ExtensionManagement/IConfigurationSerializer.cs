using System;

namespace Cloudflow.Core.ExtensionManagement
{
    public interface IConfigurationSerializer
    {
        string SerializeToString(object configuration);

        object Deserialize(string configurationData, Type configurationType);
    }
}
