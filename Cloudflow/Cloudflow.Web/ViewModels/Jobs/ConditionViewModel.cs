using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("ExtensionConfiguration.Configuration.Name")]
    public class ConditionViewModel
    {
        #region Properties
        [Hidden]
        public Guid ConditionDefinitionId { get; set; }

        [Hidden]
        public bool Deleted { get; set; }

        [Hidden]
        public bool Active { get; set; }

        [Hidden]
        public int Index { get; set; }

        [DisplayOrder(0)]
        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        [Hidden]
        public string ViewModelPropertyName { get; set; }
        #endregion

        #region Constructors
        public ConditionViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
        }
        #endregion
    }
}