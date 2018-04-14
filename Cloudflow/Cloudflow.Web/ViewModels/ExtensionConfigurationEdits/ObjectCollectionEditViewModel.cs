using System;
using System.Collections.Generic;
using System.Reflection;
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
            Id = Guid.NewGuid();
            Items = new List<ObjectCollectionItemViewModel>();
            PropertyNameParts = new List<string>();

            var categorizedItemSelectorAttribute = propertyInfo.GetCustomAttribute<CategorizedItemSelectorAttribute>();
            if (categorizedItemSelectorAttribute != null)
            {
                CategorizedItemSelectorId = categorizedItemSelectorAttribute.CollectionId.ToString();
            }
        }
    }
}