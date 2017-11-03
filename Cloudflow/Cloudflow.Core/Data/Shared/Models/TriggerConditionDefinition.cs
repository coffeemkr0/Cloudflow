using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class TriggerConditionDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        [Index("IX_TriggerConditionDefinitionId_Index", 1, IsUnique = true)]
        public Guid TriggerConditionDefinitionId { get; set; }

        [Index("IX_TriggerConditionDefinitionId_Index", 2, IsUnique = true)]
        public int Index { get; set; }

        public string Configuration { get; set; }

        public Guid TriggerDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual TriggerDefinition TriggerDefinition { get; set; }
        #endregion

        #region Constructors
        public TriggerConditionDefinition()
        {
            this.TriggerConditionDefinitionId = Guid.NewGuid();
        }
        #endregion
    }
}
