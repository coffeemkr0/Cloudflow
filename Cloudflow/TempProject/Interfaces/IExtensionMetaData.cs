using System;

namespace TempProject.Interfaces
{
    public interface IExtensionMetaData
    {
        string ExtensionId { get; }

        Type ExtensionType { get; }
    }
}