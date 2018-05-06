namespace TempProject.Triggers
{
    public interface ITriggerMonitor
    {
        void OnTriggerStarted(ITrigger trigger);

        void OnTriggerStopped(ITrigger trigger);

        void OnTriggerFired(ITrigger trigger);
    }
}