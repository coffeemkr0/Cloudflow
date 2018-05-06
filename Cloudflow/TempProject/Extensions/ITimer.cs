using System;
using System.Timers;

namespace TempProject.Extensions
{
    public interface ITimer : IDisposable
    {
        double Interval { get; set; }
        void Start();

        void Stop();

        event ElapsedEventHandler Elapsed;
    }
}