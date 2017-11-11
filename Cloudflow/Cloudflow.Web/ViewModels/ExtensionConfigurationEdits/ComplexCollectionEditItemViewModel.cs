using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ComplexCollectionEditItemViewModel
    {
        public List<string> PropertyNameParts { get; set; }

        public object Value { get; set; }

        public ComplexCollectionEditItemViewModel()
        {
            this.PropertyNameParts = new List<string>();
        }
    }
}