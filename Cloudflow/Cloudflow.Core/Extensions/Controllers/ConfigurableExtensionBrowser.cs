using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class ConfigurableExtensionBrowser
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;
        private CompositionContainer _container;
        private static readonly log4net.ILog _logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public ConfigurableExtensionBrowser(string extensionAssemblyPath)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(extensionAssemblyPath));
            _container = new CompositionContainer(catalog);
            _container.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", null);

            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
        #endregion

        #region Public Methods
        public List<IConfigurableExtensionMetaData> GetJobs()
        {
            return _extensions.Where(i => i.Metadata.Type.IsSubclassOf(typeof(Job))).Select(i => i.Metadata).ToList();
        }

        public List<IConfigurableExtensionMetaData> GetTriggers()
        {
            return _extensions.Where(i => i.Metadata.Type.IsSubclassOf(typeof(Trigger))).Select(i => i.Metadata).ToList();
        }

        public IConfigurableExtensionMetaData GetTrigger(Guid triggerId)
        {
            return GetTriggers().FirstOrDefault(i => Guid.Parse(i.Id) == triggerId);
        }

        public List<IConfigurableExtensionMetaData> GetSteps()
        {
            return _extensions.Where(i => i.Metadata.Type.IsSubclassOf(typeof(Step))).Select(i => i.Metadata).ToList();
        }
        #endregion
    }
}
