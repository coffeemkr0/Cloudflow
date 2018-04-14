namespace Cloudflow.Core.Extensions
{
    public interface IConfigurableExtensionMetaData : IExtensionMetaData
    {
        string ExtensionName { get; }

        string ExtensionDescription { get; }

        string ConfigurationExtensionId { get; }

        byte[] Icon { get; }
    }
}
