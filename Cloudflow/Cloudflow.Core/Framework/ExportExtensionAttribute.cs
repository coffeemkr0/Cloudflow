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
        public string Id { get; set; }

        public Type Type { get; set; }

        public ExportExtensionAttribute(string id, Type type) : base(typeof(IExtension))
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("'id' is required.", "id");
            if (type == null)
                throw new ArgumentException("'type' is required.", "type");

            this.Id = id;
            this.Type = type;
        }
    }
}
