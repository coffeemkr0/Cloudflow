using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public class JobConfigurationController
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<JobConfiguration, IJobConfigurationMetaData>> _jobConfigurations;

        private CompositionContainer _container;
        #endregion

        #region Properties
        public string JobName { get; }

        public log4net.ILog JobConfigurationControllerLogger { get; }
        #endregion

        #region Constructors
        public JobConfigurationController(string jobName, string assemblyPath)
        {
            this.JobName = jobName;
            this.JobConfigurationControllerLogger = log4net.LogManager.GetLogger($"JobConfigurationController.{this.JobName}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assemblyPath));
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.JobConfigurationControllerLogger.Error(compositionException);
            }
        }
        #endregion

        #region Private Methods
        private Type GetConfigurationType()
        {
            foreach (Lazy<JobConfiguration, IJobConfigurationMetaData> i in _jobConfigurations)
            {
                if (i.Metadata.JobName == this.JobName)
                {
                    return i.Metadata.Type;
                }
            }

            return null;
        }
        #endregion

        #region Public Methods
        public JobConfiguration CreateNewConfiguration()
        {
            foreach (Lazy<JobConfiguration, IJobConfigurationMetaData> i in _jobConfigurations)
            {
                if (i.Metadata.JobName == this.JobName)
                {
                    return i.Value;
                }
            }

            return null;
        }

        public JobConfiguration LoadFromFile(string fileName)
        {
            var configurationType = this.GetConfigurationType();
            var configurationObject = JobConfiguration.LoadFromFile(configurationType, fileName);
            return (JobConfiguration)configurationObject;
        }
        #endregion
    }
}
