using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class StepController
    {
        [ImportMany]
        IEnumerable<Lazy<Step, IStepData>> _steps;

        private CompositionContainer _container;

        public Step Step { get; }

        public StepConfiguration StepConfiguration { get; }

        public StepController(StepConfiguration stepConfiguration)
        {
            this.StepConfiguration = stepConfiguration;

            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();

            //Adds all the parts found in the Extensions assembly
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Extensions\bin\debug\Extensions.dll"));

            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Set the parameter value of the constructor for the step
            _container.ComposeExportedValue<StepConfiguration>("StepConfiguration", this.StepConfiguration);

            //Fill the imports of this object  
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            foreach (Lazy<Step, IStepData> i in _steps)
            {
                if (i.Metadata.Name == this.StepConfiguration.StepName)
                {
                    this.Step = i.Value;
                }
            }
        }
    }
}
