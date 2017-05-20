using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    [DataContract]
    public class AgentStatus
    {
        public enum AgentStatuses
        {
            NotRunning,
            Starting,
            Running,
            Stopping
        }

        #region Properties
        [DataMember]
        public AgentStatuses Status { get; set; }
        #endregion

        #region Constructors
        public AgentStatus()
        {

        }
        #endregion
    }
}
