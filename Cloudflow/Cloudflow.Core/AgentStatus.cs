using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    [DataContract]
    public class AgentStatus
    {
        public enum AgentStatuses
        {
            NotRunning,
            Idle,
            Processing
        }

        #region Properties
        [DataMember]
        public AgentStatuses Status { get; set; }

        [DataMember]
        public string Message { get; set; }
        #endregion
    }
}
