using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true), MetadataAttribute]
    public class ExportConfigurableExtensionAttribute : ExportExtensionAttribute
    {
        public string ConfigurationId { get; set; }

        public ExportConfigurableExtensionAttribute(string id, Type type, string configurationId) : base(id, type)
        {
            if (string.IsNullOrEmpty(configurationId))
                throw new ArgumentException("'configurationId' is required.", "configurationId");

            this.ConfigurationId = configurationId;
        }
    }
}
