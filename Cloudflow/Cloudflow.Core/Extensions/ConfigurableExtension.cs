using Cloudflow.Core.Extensions.ConfigurationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public abstract class ConfigurableExtension : Extension
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
