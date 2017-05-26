using Cloudflow.Core.Configuration;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Framework
{
    public abstract class Job
    {
        #region Properties
        public JobConfiguration JobConfiguration { get; }

        public log4net.ILog JobLogger { get; }
        #endregion

        #region Constructors
        public Job(JobConfiguration jobConfiguration)
        {
            this.JobConfiguration = jobConfiguration;
            this.JobLogger = log4net.LogManager.GetLogger($"Job.{jobConfiguration.JobName}");
        }
        #endregion

        #region Public Methods
        public abstract void Start();

        public abstract void Stop();
        #endregion
    }
}
