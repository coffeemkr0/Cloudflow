using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ObjectFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("ExtensionConfiguration.Configuration.Name")]
    public class TriggerViewModel : ICategorizedItemFetcher
    {
        #region Private Members
        private CategorizedItemCollection _conditionExtensions;
        #endregion

        #region Properties
        [Hidden]
        public Guid TriggerDefinitionId { get; set; }

        [PropertyGroupAttribute("GeneralTabText")]
        [DisplayOrder(0)]
        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        [PropertyGroupAttribute("ConditionsTabText")]
        [DisplayOrder(1)]
        [CategorizedItemSelector("AddConditionCaption", "AddConditionCategoriesCaption", "E0F69CBB-4199-4F62-B344-97325B252B91")]
        public List<ConditionViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public TriggerViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
            this.Conditions = new List<ConditionViewModel>();
        }
        #endregion

        #region Private Methods

        #endregion

        #region Public Methods
        public void LoadExtensions(string extensionLibraryFolder)
        {
            _conditionExtensions = ConfigurableExtensionFetcher.GetConfigurableExtensions(extensionLibraryFolder, Core.Extensions.ConfigurableExtensionTypes.Condition);
        }

        public CategorizedItemCollection GetItems(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(this.Conditions):
                    return _conditionExtensions;
                default:
                    throw new ArgumentException($"{propertyName} is not implemented.");
            }
        }
        #endregion
    }
}