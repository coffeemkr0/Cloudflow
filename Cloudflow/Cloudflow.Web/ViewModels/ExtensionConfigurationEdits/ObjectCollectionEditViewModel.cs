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

        public string CategorizedItemSelectorId { get; set; }

        public string LabelText { get; set; }

        public List<String> PropertyNameParts { get; set; }

        public List<ObjectCollectionItemViewModel> Items { get; set; }

        public ObjectCollectionEditViewModel(PropertyInfo propertyInfo)
        {
            this.Id = Guid.NewGuid();
            this.Items = new List<ObjectCollectionItemViewModel>();
            this.PropertyNameParts = new List<string>();

            var categorizedItemSelectorAttribute = propertyInfo.GetCustomAttribute<CategorizedItemSelectorAttribute>();
            if (categorizedItemSelectorAttribute != null)
            {
                this.CategorizedItemSelectorId = categorizedItemSelectorAttribute.CollectionId.ToString();
            }
        }
    }
}