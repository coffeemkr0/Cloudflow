namespace TempProject.Interfaces
{
    public interface ITriggerMonitor
    {
        void OnTriggerStarted(ITrigger trigger);

        void OnTriggerStopped(ITrigger trigger);

        void OnTriggerDisposed(ITrigger trigger);

        void OnTriggerFired(ITrigger trigger);
    }
}