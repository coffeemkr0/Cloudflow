using System;
using System.Linq;
using Cloudflow.Core.Extensions.ExtensionAttributes;

namespace Cloudflow.Core.Extensions
{
    public abstract class Extension : IExtension
    {
        public Extension()
        {
            var exportExtensionAttribute = GetType().GetCustomAttributes(typeof(ExportExtensionAttribute), true)
                .Cast<ExportExtensionAttribute>()
                .SingleOrDefault();

            if (exportExtensionAttribute != null)
            {
                ExtensionId = exportExtensionAttribute.ExtensionId;
                ExtensionType = exportExtensionAttribute.ExtensionType;
            }
        }

        [Hidden] public string ExtensionId { get; }

        [Hidden] public Type ExtensionType { get; }
    }
}