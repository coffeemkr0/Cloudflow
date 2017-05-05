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
            Starting,
            Idle,
            Processing,
            Stopping
        }

        #region Properties
        [DataMember]
        public AgentStatuses Status { get; set; }

        [DataMember]
        public string Runs { get; set; }
        #endregion

        #region Constructors
        public AgentStatus()
        {

        }
        #endregion
    }
}
