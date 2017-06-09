﻿using Cloudflow.Core.Configuration;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class StepController
    {
        #region Events
        public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);
        public event StepOutputEventHandler StepOutput;
        protected virtual void OnStepOutput(Step step, OutputEventLevels level, string message)
        {
            StepOutputEventHandler temp = StepOutput;
            if (temp != null)
            {
                temp(step, level, message);
            }
        }
        #endregion

        #region Private Members
        private CompositionContainer _stepsContainer;
        [ImportMany]
        IEnumerable<Lazy<IExtension, IExtensionMetaData>> _extensions = null;
        #endregion

        #region Properties
        public ExtensionConfiguration StepConfiguration { get; }

        public Step Step { get; }

        public log4net.ILog StepControllerLogger { get; }
        #endregion

        #region Constructors
        public StepController(ExtensionConfiguration stepConfiguration)
        {
            this.StepConfiguration = stepConfiguration;
            this.StepControllerLogger = log4net.LogManager.GetLogger($"StepController.{stepConfiguration.Name}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(stepConfiguration.ExtensionAssemblyPath));
            _stepsContainer = new CompositionContainer(catalog);
            _stepsContainer.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", stepConfiguration);

            try
            {
                _stepsContainer.ComposeParts(this);

                foreach (Lazy<IExtension, IExtensionMetaData> i in _extensions)
                {
                    if (Guid.Parse(i.Metadata.Id) == this.StepConfiguration.ExtensionId)
                    {
                        var step = (Step)i.Value;
                        step.StepOutput += Value_StepOutput;
                        this.Step = step;
                    }
                }
            }
            catch (Exception ex)
            {
                this.StepControllerLogger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private void Value_StepOutput(Step step, OutputEventLevels level, string message)
        {
            OnStepOutput(step, level, message);
        }
        #endregion

        #region Public Methods
        public void Execute()
        {
            try
            {
                this.Step.Execute();
            }
            catch (Exception ex)
            {
                this.StepControllerLogger.Error(ex);
            }
        }
        #endregion
    }
}