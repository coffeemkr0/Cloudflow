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

        public ObjectCollectionEditViewModel(object collectionOwner, PropertyInfo propertyInfo, ResourceManager resourceManager)
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<ObjectCollectionItemViewModel>();
            this.PropertyNameParts = new List<string>();

            if (collectionOwner.GetType().GetInterfaces().Contains(typeof(ICategorizedItemFetcher)))
            {
                var categorizedItemSelectorAttribute = propertyInfo.GetCustomAttribute<CategorizedItemSelectorAttribute>();
                if (categorizedItemSelectorAttribute != null)
                {
                    var caption = resourceManager.GetString(categorizedItemSelectorAttribute.CaptionResourceName);
                    var categoriesCaption = resourceManager.GetString(categorizedItemSelectorAttribute.CategoriesCaptionResourceName);

                    this.CategorizedItemSelectorViewModel = new CategorizedItemSelectorViewModel()
                    {
                        Id = this.Id,
                        Caption = caption,
                        CategoriesCaption = categoriesCaption
                    };

                    this.CategorizedItemSelectorViewModel.CategorizedItemCollection = ((ICategorizedItemFetcher)collectionOwner).GetItems(propertyInfo.Name);
                }
            }
        }
    }
}