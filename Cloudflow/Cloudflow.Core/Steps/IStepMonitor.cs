namespace Cloudflow.Core.Steps
{
    public interface IStepMonitor
    {
        void OnStepStarted(IStep step);

        void OnStepActivity(IStep step, string activity);

        void OnStepCompleted(IStep step);
    }
}