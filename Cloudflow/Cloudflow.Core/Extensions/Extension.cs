using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Linq;

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
