using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.Utility
{
    public static class ConfigurableExtensionFetcher
    {
        public const string TriggerExtensionCollectionId = "EA958C1A-C966-4534-80A5-CE8EC1D2D173";
        public const string StepExtensionCollectionId = "BC52AEB6-23F5-4F2F-9795-4103583A6BA5";
        public const string ConditionsExtensionCollectionId = "C171C6F2-56D0-4802-AB6E-144035A53772";

        public const string TriggerObjectFactoryExtensionId = "A530569D-546E-44CD-A957-39E330E12B9F";
        public const string StepObjectFactoryExtensionId = "8CAC8ED3-DA70-48B9-B720-361735114FAC";
        public const string ConditionObjectFactoryExtensionId = "E0F69CBB-4199-4F62-B344-97325B252B91";

        public static CategorizedItemCollection GetConfigurableExtensions(string extensionLibraryFolder, ConfigurableExtensionTypes extensionType)
        {
            var collectionId = "";
            var caption = "";
            var categoriesCaption = "";
            var objectFactoryAssemblyPath = typeof(ConfigurableExtensionFetcher).Assembly.CodeBase;
            var objectFactoryExtensionId = "";
            
            switch (extensionType)
            {
                case ConfigurableExtensionTypes.Trigger:
                    caption = Properties.Resources.AddTriggerCaption;
                    categoriesCaption = Properties.Resources.AddTriggerCategoriesCaption;
                    objectFactoryExtensionId = TriggerObjectFactoryExtensionId;
                    collectionId = TriggerExtensionCollectionId;
                    break;
                case ConfigurableExtensionTypes.Step:
                    caption = Properties.Resources.AddStepCaption;
                    categoriesCaption = Properties.Resources.AddStepCategoriesCaption;
                    objectFactoryExtensionId = StepObjectFactoryExtensionId;
                    collectionId = StepExtensionCollectionId;
                    break;
                case ConfigurableExtensionTypes.Condition:
                    caption = Properties.Resources.AddConditionCaption;
                    categoriesCaption = Properties.Resources.AddConditionCategoriesCaption;
                    objectFactoryExtensionId = ConditionObjectFactoryExtensionId;
                    collectionId = ConditionsExtensionCollectionId;
                    break;
                default:
                    throw new ArgumentException($"The extension type {extensionType} is not implemented.");
            }

            var itemCollection = new CategorizedItemCollection(Guid.Parse(collectionId), caption, categoriesCaption);

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