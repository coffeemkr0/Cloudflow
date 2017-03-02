using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cloudflow.Core;

namespace Cloudflow.WcfServiceLibrary
{
    public class AgentService : IAgentService
    {
        public AgentStatus GetAgentStatus()
        {
            return new AgentStatus()
            {
                Status = AgentStatus.AgentStatuses.Idle,
                Message = ""
            };
        }

        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
