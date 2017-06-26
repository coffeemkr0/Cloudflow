using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Resources;

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
                this.Icon = (byte[])new ImageConverter().ConvertTo(Properties.Resources.GenericExtensionIcon, typeof(byte[]));

                //using (Stream stream = ExtensionType.Assembly.
                //           GetManifestResourceStream("Timer.png"))
                //{
                //    using (Bitmap bitmap = new Bitmap(stream))
                //    {
                //        this.Icon = (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[]));
                //    }
                //}
            }
        }
    }
}
