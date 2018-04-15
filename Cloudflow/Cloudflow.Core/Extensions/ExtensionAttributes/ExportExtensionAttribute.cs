using System;
using System.ComponentModel.Composition;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public class ExportExtensionAttribute : ExportAttribute, IExtensionMetaData
    {
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

            ExtensionId = extensionId;
            ExtensionType = extensionType;
        }

        public string ExtensionId { get; set; }

        public Type ExtensionType { get; set; }
    }
}