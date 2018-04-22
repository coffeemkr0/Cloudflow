using System;

namespace TempProject.Interfaces
{
    public interface IJob : IDisposable
    {
        void Start();

        void Stop();
    }
}