using System.Collections.Generic;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class StringCollectionEditViewModel
    {
        public string LabelText { get; set; }

        public string PropertyName { get; set; }

        public List<StringCollectionEditItemViewModel> Items { get; set; }

        public StringCollectionEditViewModel()
        {
            Items = new List<StringCollectionEditItemViewModel>();
        }
    }
}