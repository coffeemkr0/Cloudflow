using Cloudflow.Core.Data.Shared.Models;
using System;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class StepViewModel
    {
        #region Properties
        public Guid StepDefinitionId { get; set; }

        public bool Active { get; set; }

        public int Index { get; set; }

        public StepConfigurationViewModel ConfigurationViewModel { get; set; }

        public ConditionCollectionViewModel Conditions { get; set; }
        #endregion

        #region Constructors
        public StepViewModel()
        {
            Conditions = new ConditionCollectionViewModel();
        }
        #endregion

        #region Public Methods
        public static StepViewModel FromStepDefinition(StepDefinition stepDefinition, int index)
        {
            var model = new StepViewModel
            {
                StepDefinitionId = stepDefinition.StepDefinitionId,
                Index = index
            };

            model.ConfigurationViewModel = StepConfigurationViewModel.FromStepDefinition(stepDefinition);
            model.Conditions = ConditionCollectionViewModel.FromStepDefinition(stepDefinition, index);

            return model;
        }
        #endregion
    }
}