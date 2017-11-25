using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ObjectFactories
{
    public static class ConfigurableExtensionFetcher
    {
        public static CategorizedItemCollection GetConfigurableExtensions(string extensionLibraryFolder, ConfigurableExtensionTypes extensionType)
        {
            var itemCollection = new CategorizedItemCollection();
            var objectFactoryAssemblyPath = typeof(ConfigurableExtensionFetcher).Assembly.CodeBase;
            var objectFactoryExtensionId = "";
            switch (extensionType)
            {
                case ConfigurableExtensionTypes.Trigger:
                    objectFactoryExtensionId = "A530569D-546E-44CD-A957-39E330E12B9F";
                    break;
                case ConfigurableExtensionTypes.Step:
                    objectFactoryExtensionId = "8CAC8ED3-DA70-48B9-B720-361735114FAC";
                    break;
                case ConfigurableExtensionTypes.Condition:
                    objectFactoryExtensionId = "E0F69CBB-4199-4F62-B344-97325B252B91";
                    break;
                default:
                    throw new ArgumentException($"The extension type {extensionType} is not implemented.");
            }

            foreach (var extensionLibraryFile in Directory.GetFiles(extensionLibraryFolder, "*.dll"))
            {
                var category = new CategorizedItemCollection.Category
                {
                    Name = FileVersionInfo.GetVersionInfo(extensionLibraryFile).ProductName
                };
                itemCollection.Categories.Add(category);

                var extensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryFile);
                foreach (var extension in extensionBrowser.GetConfigurableExtensions(extensionType))
                {
                    var item = new CategorizedItemCollection.Category.Item
                    {
                        Name = extension.ExtensionName,
                        Description = extension.ExtensionDescription,
                        Icon = extension.Icon,
                        ObjectFactoryAssemblyPath = objectFactoryAssemblyPath,
                        ObjectFactoryExtensionId = Guid.Parse(objectFactoryExtensionId),
                        FactoryData = extensionLibraryFile,
                        InstanceData = extension.ExtensionId
                    };

                    item.InstanceData = extension.ExtensionId.ToString();

                    category.Items.Add(item);
                }
            }

            return itemCollection;
        }
    }
}