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

        public ExportConfigurableExtensionAttribute(string extensionId, Type extensionType, string configurationExtensionId,
            string extensionNameResource, string extensionDescriptionResource = "", string iconResource = "") :
            base(extensionId, extensionType, typeof(IConfigurableExtension))
        {
            if (string.IsNullOrWhiteSpace(configurationExtensionId))
                throw new ArgumentException("'configurationExtensionId' is required.", "configurationExtensionId");
            if (string.IsNullOrWhiteSpace(extensionNameResource))
                throw new ArgumentException("'extensionNameResource' is required.", "extensionNameResource");

            this.ConfigurationExtensionId = configurationExtensionId;

            var defaultResources = extensionType.Assembly.GetManifestResourceNames().FirstOrDefault(i => i.Contains("Properties.Resources"));

            if (defaultResources != null)
            {
                var resourceBaseName = defaultResources.Remove(defaultResources.LastIndexOf("."));
                ResourceManager resourceManager = new ResourceManager(resourceBaseName, extensionType.Assembly);

                this.ExtensionName = resourceManager.GetString(extensionNameResource);
                this.ExtensionDescription = resourceManager.GetString(extensionDescriptionResource);

                var icon = resourceManager.GetObject(iconResource);
                if(icon != null)
                {
                    this.Icon = (byte[])new ImageConverter().ConvertTo((Bitmap)icon, typeof(byte[]));
                }
                else
                {
                    this.Icon = (byte[])new ImageConverter().ConvertTo(Properties.Resources.GenericExtensionIcon, typeof(byte[]));
                }
            }
            else
            {
                throw new InvalidOperationException($"Could not find a default resources file for {extensionType.Assembly.FullName}",
                    new Exception("Assemblies that contain extensions must have a default resources file that contains meta data for the extensions in the assembly."));
            }
        }
    }
}
