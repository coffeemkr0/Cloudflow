using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ComplexCollectionEditItemViewModel
    {
        public string PropertyName { get; set; }

        public string Value { get; set; }

        public int ItemIndex { get; set; }

        public ComplexCollectionEditItemViewModel()
        {
            
        }
    }
}