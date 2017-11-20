using Cloudflow.Web.ViewModels.Jobs;
using Cloudflow.Web.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ObjectCollectionEditViewModel
    {
        public Guid Id { get; set; }

        public string LabelText { get; set; }

        public List<String> PropertyNameParts { get; set; }

        public List<ObjectCollectionItemViewModel> Items { get; set; }

        public CategorizedItemSelectorViewModel NewItemSelectorModel { get; set; }

        public ObjectCollectionEditViewModel(string extensionLibrariesFolder)
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<ObjectCollectionItemViewModel>();
            this.PropertyNameParts = new List<string>();

            this.NewItemSelectorModel = new CategorizedItemSelectorViewModel()
            {
                Id = this.Id,
                Caption = "Triggers",
                CategoriesCaption = "Libraries"
            };

            var triggerSelector = new TriggerSelectorAttribute()
            {
                ExtensionLibraryFolder = extensionLibrariesFolder
            };
            this.NewItemSelectorModel.ItemCollection = triggerSelector.GetItems();
        }
    }
}