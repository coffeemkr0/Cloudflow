using System;

namespace Cloudflow.Core.ExtensionManagement
{
    public interface IConfigurationMetaData
    {
        Type Type { get; }
    }
}