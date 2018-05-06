namespace TempProject.Agents
{
    public interface IAgentMonitor
    {
        void OnAgentStarted(IAgent agent);

        void OnAgentStopped(IAgent agent);

        void OnAgentActivity(IAgent agent, string activity);
    }
}