using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Linq;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true), MetadataAttribute]
    public class ExportConfigurableExtensionAttribute : ExportExtensionAttribute, IConfigurableExtensionMetaData
    {
        public string ExtensionName { get; set; }

        public string ExtensionDescription { get; set; }

        public string ConfigurationExtensionId { get; set; }

        public byte[] Icon { get; set; }

        public ExportConfigurableExtensionAttribute(string extensionId, Type extensionType, string extensionName, 
            string configurationExtensionId, string extensionDescription = "", string iconResourceName = "") :
            base(extensionId, extensionType, typeof(IConfigurableExtension))
        {
            if (string.IsNullOrWhiteSpace(configurationExtensionId))
                throw new ArgumentException("'configurationExtensionId' is required.", "configurationExtensionId");
            if (string.IsNullOrWhiteSpace(configurationExtensionId))
                throw new ArgumentException("'extensionName' is required.", "extensionName");

            this.ExtensionName = extensionName;
            this.ConfigurationExtensionId = configurationExtensionId;

            this.ExtensionDescription = extensionDescription;
            if (string.IsNullOrWhiteSpace(iconResourceName))
            {
                this.Icon = (byte[])new ImageConverter().ConvertTo(Properties.Resources.GenericExtensionIcon, typeof(byte[]));
            }
            else
            {
                var defaultResources = extensionType.Assembly.GetManifestResourceNames().FirstOrDefault(i => i.Contains(".Properties"));
                if (defaultResources != null)
                {
                    var resourceBaseName = defaultResources.Remove(defaultResources.LastIndexOf("."));
                    ResourceManager resourceManager = new ResourceManager(resourceBaseName, extensionType.Assembly);
                    var icon = (Bitmap)resourceManager.GetObject(iconResourceName);
                    this.Icon = (byte[])new ImageConverter().ConvertTo(icon, typeof(byte[]));
                }
                else
                {
                    this.Icon = (byte[])new ImageConverter().ConvertTo(Properties.Resources.GenericExtensionIcon, typeof(byte[]));
                }
            }
        }
    }
}
