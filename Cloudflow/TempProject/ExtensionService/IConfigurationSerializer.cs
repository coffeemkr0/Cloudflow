using System;

namespace TempProject.ExtensionService
{
    public interface IConfigurationSerializer
    {
        string SerializeToString(object configuration);

        object Deserialize(string configurationData, Type configurationType);
    }
}
