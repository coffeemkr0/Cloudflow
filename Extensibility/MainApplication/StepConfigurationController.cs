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

        public string StepName { get; }

        public StepConfigurationController(string stepName)
        {
            this.StepName = stepName;

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
        }

        private Type GetConfigurationType()
        {
            foreach (Lazy<StepConfiguration, IStepConfigurationData> i in _stepConfigurations)
            {
                if (i.Metadata.Name == this.StepName)
                {
                    return i.Metadata.Type;
                }
            }

            return null;
        }

        public StepConfiguration CreateNewConfiguration()
        {
            foreach (Lazy<StepConfiguration, IStepConfigurationData> i in _stepConfigurations)
            {
                if (i.Metadata.Name == this.StepName)
                {
                    return i.Value;
                }
            }

            return null;
        }

        public StepConfiguration LoadFromFile(string fileName)
        {
            var configurationType = this.GetConfigurationType();
            var configurationObject = StepConfiguration.LoadFromFile(configurationType, fileName);
            return (StepConfiguration)configurationObject;
        }
    }
}
