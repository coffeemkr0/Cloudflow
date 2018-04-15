using System;

namespace Cloudflow.Core.Extensions
{
    public interface IExtensionMetaData
    {
        string ExtensionId { get; }

        Type ExtensionType { get; }
    }
}