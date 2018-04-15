using System.Runtime.Serialization;

namespace Cloudflow.Core.Agents
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

        #region Constructors

        #endregion

        #region Properties

        [DataMember] public AgentStatuses Status { get; set; }

        #endregion
    }
}