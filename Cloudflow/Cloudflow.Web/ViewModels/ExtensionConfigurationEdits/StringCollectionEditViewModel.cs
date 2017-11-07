using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class StringCollectionEditViewModel
    {
        public string LabelText { get; set; }

        public string PropertyName { get; set; }

        public List<StringCollectionEditItemViewModel> Items { get; set; }

        public StringCollectionEditViewModel()
        {
            this.Items = new List<StringCollectionEditItemViewModel>();
        }
    }
}