using Cloudflow.Web.ViewModels.Jobs;
using Cloudflow.Web.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Resources;
using Cloudflow.Core.Extensions.ExtensionAttributes;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ObjectCollectionEditViewModel
    {
        public Guid Id { get; set; }

        public string LabelText { get; set; }

        public List<String> PropertyNameParts { get; set; }

        public List<ObjectCollectionItemViewModel> Items { get; set; }

        public CategorizedItemSelectorViewModel NewItemSelectorModel { get; set; }

        public ObjectCollectionEditViewModel(PropertyInfo propertyInfo, ResourceManager resourceManager, string extensionLibrariesFolder)
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<ObjectCollectionItemViewModel>();
            this.PropertyNameParts = new List<string>();

            var categorizedItemSelectorAttribute = (CategorizedItemSelectorAttribute)propertyInfo.GetCustomAttribute(typeof(CategorizedItemSelectorAttribute));
            if(categorizedItemSelectorAttribute != null)
            {
                //categorizedItemSelectorAttribute.ExtensionLibraryFolder = extensionLibrariesFolder;
                var caption = resourceManager.GetString(categorizedItemSelectorAttribute.CaptionResourceName);
                var categoriesCaption = resourceManager.GetString(categorizedItemSelectorAttribute.CategoriesCaptionResourceName);

                this.NewItemSelectorModel = new CategorizedItemSelectorViewModel()
                {
                    Id = this.Id,
                    Caption = caption,
                    CategoriesCaption = categoriesCaption
                };

                this.NewItemSelectorModel.ItemCollection = categorizedItemSelectorAttribute.GetItems();
            }
        }
    }
}