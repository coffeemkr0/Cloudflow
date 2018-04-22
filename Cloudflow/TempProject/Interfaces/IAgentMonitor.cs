using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IAgentMonitor
    {
        void OnAgentStarted(IAgent agent);

        void OnAgentStopped(IAgent agent);

        void OnAgentActivity(IAgent agent, string activity);
    }
}
