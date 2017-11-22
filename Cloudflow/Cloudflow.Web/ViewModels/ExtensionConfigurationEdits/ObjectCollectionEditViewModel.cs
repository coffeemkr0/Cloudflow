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

        public CategorizedItemSelectorViewModel CategorizedItemSelectorViewModel { get; set; }

        public ObjectCollectionEditViewModel(PropertyInfo propertyInfo, ResourceManager resourceManager, CategorizedItemSelectorAttribute.CategorizedItemCollection categorizedItemCollection)
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<ObjectCollectionItemViewModel>();
            this.PropertyNameParts = new List<string>();

            if(categorizedItemCollection != null)
            {
                var caption = resourceManager.GetString(categorizedItemCollection.CaptionResourceName);
                var categoriesCaption = resourceManager.GetString(categorizedItemCollection.CategoriesCaptionResourceName);

                this.NewItemSelectorModel = new CategorizedItemSelectorViewModel()
                {
                    Id = this.Id,
                    Caption = caption,
                    CategoriesCaption = categoriesCaption
                };

                this.NewItemSelectorModel.ItemCollection = categorizedItemSelectorAttribute.GetItems();
            }
            
            if(categorizedItemSelectorAttribute != null)
            {
                //categorizedItemSelectorAttribute.ExtensionLibraryFolder = extensionLibrariesFolder;
                
            }
        }
    }
}