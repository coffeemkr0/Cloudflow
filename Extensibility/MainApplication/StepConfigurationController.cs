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
    public class StepConfigurationController
    {
        [ImportMany]
        IEnumerable<Lazy<StepConfiguration, IStepConfigurationData>> _stepConfigurations;

        private CompositionContainer _container;

        public StepConfiguration GetStepConfiguration(string stepName)
        {
            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();

            //Adds all the parts found in the Extensions assembly
            catalog.Catalogs.Add(new AssemblyCatalog(@"..\..\..\Extensions\bin\debug\Extensions.dll"));

            //Create the CompositionContainer with the parts in the catalog  
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object  
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

            foreach (Lazy<StepConfiguration, IStepConfigurationData> i in _stepConfigurations)
            {
                if (i.Metadata.Name == stepName)
                {
                    return i.Value;
                }
            }

            return null;
        }
    }
}
