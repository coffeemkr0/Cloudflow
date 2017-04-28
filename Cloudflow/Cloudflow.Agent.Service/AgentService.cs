using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cloudflow.Core;

namespace Cloudflow.WcfServiceLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class AgentService : IAgentService
    {
        private static readonly log4net.ILog _logger =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Job _job;

        public AgentStatus GetAgentStatus()
        {
            if(_job == null)
            {
                return new AgentStatus()
                {
                    Status = AgentStatus.AgentStatuses.Idle,
                    Message = ""
                };
            }
            else
            {
                return new AgentStatus()
                {
                    Status = AgentStatus.AgentStatuses.Processing,
                    Message = ""
                };
            }
        }

        public void EnableJob()
        {
            if(_job == null)
            {
                _logger.Debug("Enabling the job");
                _job = Job.CreateTestJob();
                _job.Enable();
                _logger.Info("Job enabled");
            }
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
