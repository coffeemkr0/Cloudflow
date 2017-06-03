using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ConfigurationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public abstract class Extension : IExtension
    {
        [Hidden]
        public string Id { get; }

        [Hidden]
        public Type Type { get; }

        public Extension()
        {
            var exportExtensionAttribute = this.GetType().GetCustomAttributes(typeof(ExportExtensionAttribute), true).
                Cast<ExportExtensionAttribute>()
                .SingleOrDefault();

            if (exportExtensionAttribute != null)
            {
                this.Id = exportExtensionAttribute.Id;
                this.Type = exportExtensionAttribute.Type;
            }
        }
    }
}
