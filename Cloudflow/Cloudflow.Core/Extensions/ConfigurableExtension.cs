using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.Linq;

namespace Cloudflow.Core.Extensions
{
    public abstract class ConfigurableExtension : Extension, IConfigurableExtension
    {
        #region Properties
        public string ExtensionName { get; }

        public string ExtensionDescription { get; }

        public string ConfigurationExtensionId { get; }
        #endregion

        #region Constructors
        public ConfigurableExtension() : base()
        {
            var exportConfigurableExtensionAttribute = this.GetType().GetCustomAttributes(typeof(ExportConfigurableExtensionAttribute), true).
                Cast<ExportConfigurableExtensionAttribute>()
                .SingleOrDefault();

            if (exportConfigurableExtensionAttribute != null)
            {
                this.ExtensionName = exportConfigurableExtensionAttribute.ExtensionName;
                this.ExtensionDescription = exportConfigurableExtensionAttribute.ExtensionDescription;
                this.ConfigurationExtensionId = exportConfigurableExtensionAttribute.ConfigurationExtensionId;
            }
        }
        #endregion
    }
}
