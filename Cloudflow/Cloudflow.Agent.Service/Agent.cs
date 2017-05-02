using Cloudflow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Agent.Service
{
    public class Agent
    {
        #region Properties
        public List<Job> Jobs { get; set; }
        #endregion

        #region Constructors
        public Agent()
        {
            this.Jobs = new List<Job>();
        }
        #endregion

        #region Private Methods
        private void LoadJobs()
        {
            this.Jobs.Clear();
            this.Jobs.Add(Job.CreateTestJob("Test Job 1"));
            this.Jobs.Add(Job.CreateTestJob("Test Job 2"));
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            LoadJobs();
            foreach (var job in this.Jobs)
            {
                job.Start();
            }
        }

        public void Stop()
        {
            foreach (var job in this.Jobs)
            {
                job.Stop();
            }
        }
        #endregion
    }
}
