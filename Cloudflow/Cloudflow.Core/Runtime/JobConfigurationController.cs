using Cloudflow.Core.Configuration;
using Cloudflow.Core.Data.Server.Models;
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
        IEnumerable<Lazy<JobConfiguration, IJobConfigurationMetaData>> _jobConfigurations = null;

        private CompositionContainer _container;
        #endregion

        #region Properties
        public Guid JobExtensionId { get; }

        public string JobConfigurationExtensionAssemlbyPath { get; }

        public log4net.ILog JobConfigurationControllerLogger { get; }
        #endregion

        #region Constructors
        public JobConfigurationController(Guid jobExtensionId, string jobConfigurationAssemblyPath)
        {
            this.JobExtensionId = jobExtensionId;
            this.JobConfigurationExtensionAssemlbyPath = jobConfigurationAssemblyPath;

            this.JobConfigurationControllerLogger = log4net.LogManager.GetLogger($"JobConfigurationController.{jobExtensionId}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(this.JobConfigurationExtensionAssemlbyPath));
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                this.JobConfigurationControllerLogger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private Type GetConfigurationType()
        {
            foreach (Lazy<JobConfiguration, IJobConfigurationMetaData> i in _jobConfigurations)
            {
                if (Guid.Parse(i.Metadata.JobExtensionId) == this.JobExtensionId)
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
                if (Guid.Parse(i.Metadata.JobExtensionId) == this.JobExtensionId)
                {
                    return i.Value;
                }
            }

            return null;
        }

        public JobConfiguration Load(string json)
        {
            var configurationType = this.GetConfigurationType();
            var configurationObject = JobConfiguration.Load(configurationType, json);
            return (JobConfiguration)configurationObject;
        }
        #endregion
    }
}
