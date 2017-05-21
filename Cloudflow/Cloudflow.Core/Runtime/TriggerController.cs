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
    public class TriggerController
    {
        #region Private Members
        private CompositionContainer _triggersContainer;
        #endregion

        #region Properties
        public TriggerConfiguration TriggerConfiguration { get; }

        [Import(typeof(Trigger))]
        public List<Trigger> Triggers { get; set; }

        public log4net.ILog TriggerControllerLoger { get; }
        #endregion

        #region Constructors
        public TriggerController(TriggerConfiguration triggerConfiguration)
        {
            this.TriggerConfiguration = triggerConfiguration;
            this.TriggerControllerLoger = log4net.LogManager.GetLogger($"TriggerController.{triggerConfiguration.Name}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll"));
            _triggersContainer = new CompositionContainer(catalog);
            _triggersContainer.ComposeExportedValue<TriggerConfiguration>("TriggerConfiguration", triggerConfiguration);

            try
            {
                _triggersContainer.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.TriggerControllerLoger.Error(compositionException);
            }
        }
        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion
    }
}
