using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ComplexCollectionEditItemViewModel
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public List<string> PropertyNameParts { get; set; }

        public string DisplayText { get; set; }

        public object Value { get; set; }

        public ComplexCollectionEditItemViewModel()
        {
            this.Id = Guid.NewGuid();
            this.PropertyNameParts = new List<string>();
        }
    }
}