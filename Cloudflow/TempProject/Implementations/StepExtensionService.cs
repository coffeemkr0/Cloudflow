using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class StepExtensionService : IStepExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<IStep, IExtensionMetaData>> StepExtensions = null;

        public StepExtensionService()
        {
            //An aggregate catalog that combines multiple catalogs  
            var catalog = new AggregateCatalog();

            //Adds all the parts found in the calling assembly
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetCallingAssembly());
            catalog.Catalogs.Add(assemblyCatalog);
            
            //Create the CompositionContainer with the parts in the catalog  
            var container = new CompositionContainer(catalog);

            //Set the constructor parameter for Step extensions
            container.ComposeExportedValue("ExtensionConfiguration", "Hello World!");

            //Fill the StepExtensions imports
            container.ComposeParts(this);
        }

        public IStep GetStep(Guid stepExtensionId)
        {
            foreach (var i in StepExtensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == stepExtensionId)
                    return i.Value;

            return null;
        }
    }
}