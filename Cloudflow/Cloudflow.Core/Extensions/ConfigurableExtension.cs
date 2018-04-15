using System.Linq;
using Cloudflow.Core.Extensions.ExtensionAttributes;

namespace Cloudflow.Core.Extensions
{
    public abstract class ConfigurableExtension : Extension, IConfigurableExtension
    {
        #region Constructors

        public ConfigurableExtension()
        {
            var exportConfigurableExtensionAttribute = GetType()
                .GetCustomAttributes(typeof(ExportConfigurableExtensionAttribute), true)
                .Cast<ExportConfigurableExtensionAttribute>()
                .SingleOrDefault();

            if (exportConfigurableExtensionAttribute != null)
            {
                ExtensionName = exportConfigurableExtensionAttribute.ExtensionName;
                ExtensionDescription = exportConfigurableExtensionAttribute.ExtensionDescription;
                ConfigurationExtensionId = exportConfigurableExtensionAttribute.ConfigurationExtensionId;
                Icon = exportConfigurableExtensionAttribute.Icon;
            }
        }

        #endregion

        #region Properties

        public string ExtensionName { get; }

        public string ExtensionDescription { get; }

        public string ConfigurationExtensionId { get; }

        public byte[] Icon { get; set; }

        #endregion
    }
}