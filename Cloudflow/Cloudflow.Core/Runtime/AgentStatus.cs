using System.Runtime.Serialization;

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
