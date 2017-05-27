using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public abstract class Extension : IExtension
    {
        public string Id { get; }

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
