using Cloudflow.Core.Configuration;
using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class TriggerController
    {
        #region Events
        public delegate void TriggerFiredEventHandler(Trigger trigger, Dictionary<string, object> triggerData);
        public event TriggerFiredEventHandler TriggerFired;
        protected virtual void OnTriggerFired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            TriggerFiredEventHandler temp = TriggerFired;
            if (temp != null)
            {
                temp(trigger, triggerData);
            }
        }
        #endregion

        #region Private Members
        private CompositionContainer _triggersContainer;
        [ImportMany]
        IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;
        #endregion

        #region Properties
        public ExtensionConfiguration TriggerConfiguration { get; }

        public log4net.ILog TriggerControllerLoger { get; }

        public Trigger Trigger { get; }
        #endregion

        #region Constructors
        public TriggerController(ExtensionConfiguration triggerConfiguration)
        {
            this.TriggerConfiguration = triggerConfiguration;
            this.TriggerControllerLoger = log4net.LogManager.GetLogger($"TriggerController.{triggerConfiguration.Name}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(triggerConfiguration.ExtensionAssemblyPath));
            _triggersContainer = new CompositionContainer(catalog);
            _triggersContainer.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", triggerConfiguration);

            try
            {
                _triggersContainer.ComposeParts(this);

                foreach (Lazy<IConfigurableExtension, IConfigurableExtensionMetaData> i in _extensions)
                {
                    if (Guid.Parse(i.Metadata.Id) == this.TriggerConfiguration.ExtensionId)
                    {
                        this.Trigger = (Trigger)i.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.TriggerControllerLoger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private void Trigger_Fired(Trigger trigger, Dictionary<string, object> triggerData)
        {
            OnTriggerFired(trigger, triggerData);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.Trigger.Fired += Trigger_Fired;
            this.Trigger.Start();
        }

        public void Stop()
        {
            this.Trigger.Fired -= Trigger_Fired;
            this.Trigger.Stop();
        }
        #endregion
    }
}
