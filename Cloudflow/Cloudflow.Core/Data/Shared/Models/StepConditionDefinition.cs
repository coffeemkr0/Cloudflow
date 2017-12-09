using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class StepConditionDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        public Guid StepConditionDefinitionId { get; set; }

        public int Index { get; set; }

        public Guid StepDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual StepDefinition StepDefinition { get; set; }
        #endregion

        #region Constructors
        public StepConditionDefinition()
        {
            this.StepConditionDefinitionId = Guid.NewGuid();
        }
        #endregion
    }
}
