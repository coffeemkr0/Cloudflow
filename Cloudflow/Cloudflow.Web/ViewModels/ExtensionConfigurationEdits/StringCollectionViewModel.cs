using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class StringCollectionViewModel
    {
        public string PropertyName { get; set; }

        public IEnumerable<string> Items { get; set; }

        public StringCollectionViewModel()
        {
            this.Items = new List<string>();
        }
    }
}