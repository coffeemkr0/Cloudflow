using System;
using TempProject.Agents;

namespace TempProject.Tests.Agents
{
    public class AgentMonitor : IAgentMonitor
    {
        public bool OnAgentStartedCalled { get; set; }
        public bool OnAgentStopCalled { get; set; }
        public bool OnAgentActivityCalled { get; set; }
        public bool ExceptionOccurred { get; set; }

        public void OnAgentStarted(IAgent agent)
        {
            OnAgentStartedCalled = true;
            Console.WriteLine($"[{agent.GetClassName()}] Agent started");
        }

        public void OnAgentStopped(IAgent agent)
        {
            OnAgentStopCalled = true;
            Console.WriteLine($"[{agent.GetClassName()}] Agent stopped");
        }

        public void OnAgentActivity(IAgent agent, string activity)
        {
            OnAgentActivityCalled = true;
            Console.WriteLine($"[{agent.GetClassName()}] Agent activity - {activity}");

            if (activity.Contains("Exception")) ExceptionOccurred = true;
        }
    }
}