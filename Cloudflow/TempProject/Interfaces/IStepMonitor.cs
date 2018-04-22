using System;

namespace TempProject.Interfaces
{
    public interface IStepMonitor
    {
        void OnStepStarted(IStep step);

        void OnStepActivity(IStep step, string activity);

        void OnStepCompleted(IStep step);

        void OnStepDisposing(IStep step);
    }
}