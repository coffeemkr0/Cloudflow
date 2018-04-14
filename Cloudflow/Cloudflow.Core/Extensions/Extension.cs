using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Linq;

namespace Cloudflow.Core.Extensions
{
    public abstract class Extension : IExtension
    {
        [Hidden]
        public string ExtensionId { get; }

        [Hidden]
        public Type ExtensionType { get; }

        public Extension()
        {
            var exportExtensionAttribute = GetType().GetCustomAttributes(typeof(ExportExtensionAttribute), true).
                Cast<ExportExtensionAttribute>()
                .SingleOrDefault();

            if (exportExtensionAttribute != null)
            {
                ExtensionId = exportExtensionAttribute.ExtensionId;
                ExtensionType = exportExtensionAttribute.ExtensionType;
            }
        }
    }
}
