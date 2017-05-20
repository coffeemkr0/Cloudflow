using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public abstract class JobConfiguration
    {
        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<StepConfiguration> StepConfigurations { get; set; }

        public List<TriggerConfiguration> TriggerConfigurations { get; set; }
        #endregion

        #region Constructors
        public JobConfiguration()
        {
            this.Id = Guid.NewGuid();
            this.StepConfigurations = new List<StepConfiguration>();
            this.TriggerConfigurations = new List<TriggerConfiguration>();
        }
        #endregion
    }
}
