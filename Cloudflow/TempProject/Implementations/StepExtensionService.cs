using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class StepExtensionService : IStepExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<IStep, IExtensionMetaData>> StepExtensions = null;

        public StepExtensionService(ICatalogProvider catalogProvider, object stepConfiguration)
        {
            //Create the CompositionContainer with the parts in the catalog  
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            //Set the constructor parameter for Step extensions
            container.ComposeExportedValue("Configuration", stepConfiguration);

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