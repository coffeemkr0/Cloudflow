using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class StringCollectionEditViewModel
    {
        public string PropertyName { get; set; }

        public IEnumerable<string> Items { get; set; }

        public StringCollectionEditViewModel()
        {
            this.Items = new List<string>();
        }
    }
}