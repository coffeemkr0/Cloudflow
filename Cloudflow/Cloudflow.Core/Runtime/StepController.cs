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
        IEnumerable<Lazy<Step, IStepMetaData>> _steps;
        #endregion

        #region Properties
        public StepConfiguration StepConfiguration { get; }

        public log4net.ILog StepControllerLogger { get; }
        #endregion

        #region Constructors
        public StepController(StepConfiguration stepConfiguration)
        {
            this.StepConfiguration = stepConfiguration;
            this.StepControllerLogger = log4net.LogManager.GetLogger($"StepController.{stepConfiguration.StepName}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll"));
            _stepsContainer = new CompositionContainer(catalog);
            _stepsContainer.ComposeExportedValue<StepConfiguration>("StepConfiguration", stepConfiguration);

            try
            {
                _stepsContainer.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.StepControllerLogger.Error(compositionException);
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
            foreach (Lazy<Step, IStepMetaData> i in _steps)
            {
                if (i.Metadata.StepName == this.StepConfiguration.StepName)
                {
                    i.Value.StepOutput += Value_StepOutput;
                    try
                    {
                        i.Value.Execute();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        i.Value.StepOutput -= Value_StepOutput;
                    }
                }
            }
        }
        #endregion
    }
}
