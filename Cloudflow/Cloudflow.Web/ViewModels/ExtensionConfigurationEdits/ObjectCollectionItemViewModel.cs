using System;
using System.Collections.Generic;

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
            Id = Guid.NewGuid();
            PropertyNameParts = new List<string>();
        }
    }
}