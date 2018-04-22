using System.Timers;
using TempProject.Implementations;

namespace TempProject.Tests.Triggers
{
    public class TestTimer : ITimer
    {
        public double Interval { get; set; }

        public event ElapsedEventHandler Elapsed;

        public void Dispose()
        {
        }

        public void Start()
        {
            Elapsed?.Invoke(this, null);
        }

        public void Stop()
        {
        }
    }
}