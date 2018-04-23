using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TempProject.Interfaces;
using TempProject.Tests.Steps;

namespace TempProject.Implementations
{
    public class StepConfigurationExtensionService : StepConfigurationExtensionServiceShould
    {
        [ImportMany] protected IEnumerable<Lazy<IStepConfiguration, IExtensionMetaData>> Extensions = null;

        public StepConfigurationExtensionService(ICatalogProvider catalogProvider)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);
        }

        public IStepConfiguration GetConfiguration(Guid extensionId)
        {
            foreach (var i in Extensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            return null;
        }
    }
}