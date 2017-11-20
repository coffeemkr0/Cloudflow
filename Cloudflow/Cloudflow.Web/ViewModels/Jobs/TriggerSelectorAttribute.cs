using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerSelectorAttribute : CategorizedItemSelectorAttribute
    {
        #region Properties
        public string ExtensionLibraryFolder { get; set; }
        #endregion

        #region Constructors
        public TriggerSelectorAttribute(string captionResourceName, string categoriesCaptionResourceName) 
            : base(captionResourceName, categoriesCaptionResourceName)
        {
            
        }
        #endregion

        #region Public Methods
        public override CategorizedItemCollection GetItems()
        {
            var itemCollection = new CategorizedItemCollection();

            foreach (var extensionLibraryFile in Directory.GetFiles(this.ExtensionLibraryFolder, "*.dll"))
            {
                var category = new CategorizedItemCollection.Category
                {
                    Name = FileVersionInfo.GetVersionInfo(extensionLibraryFile).ProductName
                };
                itemCollection.Categories.Add(category);

                var extensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryFile);
                foreach (var extension in extensionBrowser.GetConfigurableExtensions(ConfigurableExtensionTypes.Trigger))
                {
                    category.Items.Add(new CategorizedItemCollection.Category.Item
                    {
                        Name = extension.ExtensionName,
                        Description = extension.ExtensionDescription,
                        Icon = extension.Icon
                    });
                }
            }

            return itemCollection;
        }
        #endregion
    }
}