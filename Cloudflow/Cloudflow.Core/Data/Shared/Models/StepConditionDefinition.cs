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
        [Index("IX_StepConditionDefinitionId_Index", 1, IsUnique = true)]
        public Guid StepConditionDefinitionId { get; set; }

        [Index("IX_StepConditionDefinitionId_Index", 2, IsUnique = true)]
        public int Index { get; set; }

        public Guid StepDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual StepDefinition StepDefinition { get; set; }
        #endregion
    }
}
