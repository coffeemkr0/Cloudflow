using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public abstract class TriggerConfiguration
    {
        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; }
        #endregion
    }
}
