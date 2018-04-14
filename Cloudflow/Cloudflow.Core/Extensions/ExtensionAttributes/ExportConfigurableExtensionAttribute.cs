using System;
using System.ComponentModel.Composition;
using System.Drawing;
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

            ConfigurationExtensionId = configurationExtensionId;

            var defaultResources = extensionType.Assembly.GetManifestResourceNames().FirstOrDefault(i => i.Contains("Properties.Resources"));

            if (defaultResources != null)
            {
                var resourceBaseName = defaultResources.Remove(defaultResources.LastIndexOf("."));
                var resourceManager = new ResourceManager(resourceBaseName, extensionType.Assembly);

                ExtensionName = resourceManager.GetString(extensionNameResource);
                ExtensionDescription = resourceManager.GetString(extensionDescriptionResource);

                var icon = resourceManager.GetObject(iconResource);
                if(icon != null)
                {
                    Icon = (byte[])new ImageConverter().ConvertTo((Bitmap)icon, typeof(byte[]));
                }
                else
                {
                    Icon = (byte[])new ImageConverter().ConvertTo(Properties.Resources.GenericExtensionIcon, typeof(byte[]));
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
