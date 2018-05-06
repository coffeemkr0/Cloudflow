using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class ExtensionService : IExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<ITrigger, ITriggerMetaData>> Triggers = null;
        [ImportMany] protected IEnumerable<Lazy<ITriggerDescriptor, IDescriptorMetaData>> TriggerDescriptors = null;
        [ImportMany] protected IEnumerable<Lazy<ITriggerConfiguration, IConfigurationMetaData>> TriggerConfigurations = null;

        [ImportMany] protected IEnumerable<Lazy<IStep, ITriggerMetaData>> Steps = null;
        [ImportMany] protected IEnumerable<Lazy<IStepDescriptor, IDescriptorMetaData>> StepDescriptors = null;
        [ImportMany] protected IEnumerable<Lazy<IStepConfiguration, IConfigurationMetaData>> StepConfigurations = null;

        private ITriggerDescriptor GetTriggerDescriptor(Guid extensionId)
        {
            foreach (var i in TriggerDescriptors)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        private IStepDescriptor GetStepDescriptor(Guid extensionId)
        {
            foreach (var i in StepDescriptors)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        public ITrigger LoadTrigger(ICatalogProvider catalogProvider, Guid extensionId, ITriggerConfiguration configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeExportedValue("Configuration", configuration);

            container.ComposeParts(this);

            var triggerDescriptor = GetTriggerDescriptor(extensionId);

            foreach (var trigger in Triggers)
            {
                if (trigger.Metadata.Type == triggerDescriptor.ExtensionType)
                {
                    return trigger.Value;
                }
            }

            throw new ExtensionNotFoundException(extensionId);
        }

        public ITriggerConfiguration LoadTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId, string configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var triggerDescriptor = GetTriggerDescriptor(extensionId);

            if (!string.IsNullOrEmpty(configuration))
            {
                //TODO:This deserialization should not be implemented in this service - abstract this out to a dependency
                return (ITriggerConfiguration) JsonConvert.DeserializeObject(configuration,
                    triggerDescriptor.ConfigurationType);
            }

            foreach (var triggerConfiguration in TriggerConfigurations)
            {
                if (triggerConfiguration.Metadata.Type == triggerDescriptor.ConfigurationType)
                {
                    return triggerConfiguration.Value;
                }
            }

            return null;
        }

        public IStep LoadStep(ICatalogProvider catalogProvider, Guid extensionId, IStepConfiguration configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeExportedValue("Configuration", configuration);

            container.ComposeParts(this);

            var stepDescriptor = GetStepDescriptor(extensionId);

            foreach (var step in Steps)
            {
                if (step.Metadata.Type == stepDescriptor.ExtensionType)
                {
                    return step.Value;
                }
            }
            
            throw new ExtensionNotFoundException(extensionId);
        }

        public IStepConfiguration LoadStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId, string configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var stepDescriptor = GetStepDescriptor(extensionId);

            if (!string.IsNullOrEmpty(configuration))
            {
                //TODO:This deserialization should not be implemented in this service - abstract this out to a dependency
                return (IStepConfiguration) JsonConvert.DeserializeObject(configuration,
                    stepDescriptor.ConfigurationType);
            }

            foreach (var stepConfiguration in StepConfigurations)
            {
                if (stepConfiguration.Metadata.Type == stepDescriptor.ConfigurationType)
                {
                    return stepConfiguration.Value;
                }
            }

            return null;
        }

        public ITriggerConfiguration CreateNewTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var triggerDescriptor = GetTriggerDescriptor(extensionId);

            foreach (var triggerConfiguration in TriggerConfigurations)
            {
                if (triggerConfiguration.Metadata.Type == triggerDescriptor.ConfigurationType)
                {
                    return triggerConfiguration.Value;
                }
            }

            throw new ExtensionNotFoundException(extensionId);
        }

        public IStepConfiguration CreateNewStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var stepDescriptor = GetStepDescriptor(extensionId);

            foreach (var stepConfiguration in StepConfigurations)
            {
                if (stepConfiguration.Metadata.Type == stepDescriptor.ConfigurationType)
                {
                    return stepConfiguration.Value;
                }
            }

            throw new ExtensionNotFoundException(extensionId);
        }
    }
}