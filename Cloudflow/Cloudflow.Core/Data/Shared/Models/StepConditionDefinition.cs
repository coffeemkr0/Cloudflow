using System;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class StepConditionDefinition : ConfigurableExtensionDefinition
    {
        #region Constructors

        public StepConditionDefinition()
        {
            StepConditionDefinitionId = Guid.NewGuid();
        }

        #endregion

        #region Properties

        public Guid StepConditionDefinitionId { get; set; }

        public int Index { get; set; }

        public Guid StepDefinitionId { get; set; }

        [ScriptIgnore] public virtual StepDefinition StepDefinition { get; set; }

        #endregion
    }
}