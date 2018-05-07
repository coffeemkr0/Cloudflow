namespace Cloudflow.Core.Agents
{
    public interface IAgent
    {
        AgentStatus AgentStatus { get; }

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