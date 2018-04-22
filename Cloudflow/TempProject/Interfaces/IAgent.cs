using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IAgent
    {
        void Start(IAgentMonitor agentMonitor);

        void Stop();
    }

    public static class AgentExtensions
    {
        public static string GetClassName(this IAgent agent)
        {
            return agent.GetType().Name;
        }
    }
}
