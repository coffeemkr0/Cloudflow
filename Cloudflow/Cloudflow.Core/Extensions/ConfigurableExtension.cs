using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.Linq;

namespace Cloudflow.Core.Extensions
{
    public abstract class ConfigurableExtension : Extension, IConfigurableExtension
    {
        #region Properties
        public string ConfigurationId { get; }
        #endregion

        #region Constructors
        public ConfigurableExtension() : base()
        {
            var exportConfigurableExtensionAttribute = this.GetType().GetCustomAttributes(typeof(ExportConfigurableExtensionAttribute), true).
                Cast<ExportConfigurableExtensionAttribute>()
                .SingleOrDefault();

            if (exportConfigurableExtensionAttribute != null)
            {
                this.ConfigurationId = exportConfigurableExtensionAttribute.ConfigurationId;
            }
        }
        #endregion
    }
}
