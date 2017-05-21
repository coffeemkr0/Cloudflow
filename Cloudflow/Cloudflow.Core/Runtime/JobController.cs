using Cloudflow.Core.Configuration;
using Cloudflow.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public class JobController
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<Job, IJobMetaData>> _jobs;
        private CompositionContainer _jobsContainer;
        #endregion

        #region Properties
        public JobConfiguration JobConfiguration { get; }

        public log4net.ILog JobControllerLoger { get; }
        #endregion

        #region Constructors
        public JobController(JobConfiguration jobConfiguration)
        {
            this.JobConfiguration = jobConfiguration;
            this.JobControllerLoger = log4net.LogManager.GetLogger($"JobController.{jobConfiguration.Name}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll"));
            _jobsContainer = new CompositionContainer(catalog);
            _jobsContainer.ComposeExportedValue<JobConfiguration>("JobConfiguration", jobConfiguration);

            try
            {
                _jobsContainer.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.JobControllerLoger.Error(compositionException);
            }
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            foreach (Lazy<Job, IJobMetaData> i in _jobs)
            {
                if (i.Metadata.Name == this.JobConfiguration.Name)
                {
                    i.Value.Start();
                }
            }
        }

        public void Stop()
        {
            foreach (Lazy<Job, IJobMetaData> i in _jobs)
            {
                if (i.Metadata.Name == this.JobConfiguration.Name)
                {
                    i.Value.Stop();
                }
            }
        }
        #endregion
    }
}
