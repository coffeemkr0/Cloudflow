using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Cloudflow.Core.Job testJob = Cloudflow.Core.Job.CreateTestJob();
            testJob.Enable();

            Console.WriteLine("The job has been enabled and its trigger has been initialized.");
            Console.ReadLine();
        }
    }
}
