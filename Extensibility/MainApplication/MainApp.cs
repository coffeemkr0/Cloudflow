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
    class MainApp
    {
        [ImportMany]
        IEnumerable<Lazy<Step, IStepData>> _steps;

        private CompositionContainer _container;

        public void Start()
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
        }

        public void ExecuteStep(string stepName)
        {
            foreach (Lazy<Step, IStepData> i in _steps)
            {
                if (i.Metadata.Name == stepName)
                {
                    i.Value.Name = stepName;
                    i.Value.Execute();
                }
            }
        }
    }
}
