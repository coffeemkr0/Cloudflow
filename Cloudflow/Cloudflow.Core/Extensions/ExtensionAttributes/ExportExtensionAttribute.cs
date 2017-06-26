using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true), MetadataAttribute]
    public class ExportExtensionAttribute : ExportAttribute, IExtensionMetaData
    {
        public string ExtensionId { get; set; }

        public Type ExtensionType { get; set; }

        public ExportExtensionAttribute(string extensionId, Type extensionType) : 
            this(extensionId, extensionType, typeof(IExtension))
        {

        }

        public ExportExtensionAttribute(string extensionId, Type extensionType, Type contractType) : base(contractType)
        {
            if (string.IsNullOrEmpty(extensionId))
                throw new ArgumentException("'extensionId' is required.", "extensionId");
            if (extensionType == null)
                throw new ArgumentException("'extensionType' is required.", "extensionType");

            this.ExtensionId = extensionId;
            this.ExtensionType = extensionType;
        }
    }
}
