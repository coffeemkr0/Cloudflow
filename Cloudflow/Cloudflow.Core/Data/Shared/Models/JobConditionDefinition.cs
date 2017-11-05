using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class JobConditionDefinition : ConfigurableExtensionDefinition
    {
        #region Properties
        [Index("IX_JobConditionDefinitionId_Index", 1, IsUnique = true)]
        public Guid JobConditionDefinitionId { get; set; }

        [Index("IX_JobConditionDefinitionId_Index", 2, IsUnique = true)]
        public int Index { get; set; }

        public Guid JobDefinitionId { get; set; }

        [ScriptIgnore]
        public virtual JobDefinition JobDefinition { get; set; }
        #endregion
    }
}
