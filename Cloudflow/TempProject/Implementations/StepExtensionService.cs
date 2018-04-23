using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TempProject.Interfaces;
using TempProject.Tests.Steps;

namespace TempProject.Implementations
{
    public class StepExtensionService : IStepExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<IStep, IExtensionMetaData>> StepExtensions = null;

        public StepExtensionService(ICatalogProvider catalogProvider, IStepConfiguration stepConfiguration)
        {
            //Create the CompositionContainer with the parts in the catalog  
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            //Set the constructor parameter for Step extensions
            container.ComposeExportedValue("Configuration", stepConfiguration);

            //Fill the StepExtensions imports
            container.ComposeParts(this);
        }

        public IStep GetStep(Guid extensionId)
        {
            foreach (var i in StepExtensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            return null;
        }
    }
}