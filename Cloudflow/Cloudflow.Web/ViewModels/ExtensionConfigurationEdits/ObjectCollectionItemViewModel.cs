using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.ExtensionConfigurationEdits
{
    public class ObjectCollectionItemViewModel
    {
        public Guid Id { get; set; }

        public bool Active { get; set; }

        public List<string> PropertyNameParts { get; set; }

        public string DisplayText { get; set; }

        public object Value { get; set; }

        public ObjectCollectionItemViewModel()
        {
            this.Id = Guid.NewGuid();
            this.PropertyNameParts = new List<string>();
        }
    }
}