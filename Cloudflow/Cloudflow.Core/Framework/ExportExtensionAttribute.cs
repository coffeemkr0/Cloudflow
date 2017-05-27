using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true), MetadataAttribute]
    public class ExportExtensionAttribute : ExportAttribute, IExtensionMetaData
    {
        public string ExtensionId { get; set; }

        public Type Type { get; set; }

        public ExportExtensionAttribute(string extensionId, Type type) : base(typeof(IExtension))
        {
            if (string.IsNullOrEmpty(extensionId))
                throw new ArgumentException("'extensionId' is required.", "extensionId");
            if (type == null)
                throw new ArgumentException("'type' is required.", "type");

            this.ExtensionId = extensionId;
            this.Type = type;
        }
    }
}
