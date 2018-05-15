using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Cloudflow.Core.Conditions;
using Cloudflow.Core.Exceptions;
using Cloudflow.Core.Steps;
using Cloudflow.Core.Triggers;

namespace Cloudflow.Core.ExtensionManagement
{
    public class ExtensionService : IExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<IStepConfiguration, IConfigurationMetaData>> StepConfigurations = null;
        [ImportMany] protected IEnumerable<Lazy<IStepDescriptor, IDescriptorMetaData>> StepDescriptors = null;
        [ImportMany] protected IEnumerable<Lazy<IStep, IStepMetaData>> Steps = null;

        [ImportMany]
        protected IEnumerable<Lazy<ITriggerConfiguration, IConfigurationMetaData>> TriggerConfigurations = null;
        [ImportMany] protected IEnumerable<Lazy<ITriggerDescriptor, IDescriptorMetaData>> TriggerDescriptors = null;
        [ImportMany] protected IEnumerable<Lazy<ITrigger, ITriggerMetaData>> Triggers = null;

        [ImportMany] protected IEnumerable<Lazy<IConditionConfiguration, IConfigurationMetaData>> ConditionConfigurations = null;
        [ImportMany] protected IEnumerable<Lazy<IConditionDescriptor, IDescriptorMetaData>> ConditionDescriptors = null;
        [ImportMany] protected IEnumerable<Lazy<ICondition, IConditionMetaData>> Conditions = null;

        readonly IConfigurationSerializer _configurationSerializer;

        public ExtensionService(IConfigurationSerializer configurationSerializer)
        {
            _configurationSerializer = configurationSerializer;
        }

        public ITrigger LoadTrigger(ICatalogProvider catalogProvider, Guid extensionId,
            ITriggerConfiguration configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeExportedValue("Configuration", configuration);

            container.ComposeParts(this);

            var triggerDescriptor = GetTriggerDescriptor(extensionId);

            foreach (var trigger in Triggers)
                if (trigger.Metadata.Type == triggerDescriptor.ExtensionType)
                    return trigger.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        public ICondition LoadCondition(ICatalogProvider catalogProvider, Guid extensionId, IConditionConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public ITriggerConfiguration LoadTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId,
            string configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var triggerDescriptor = GetTriggerDescriptor(extensionId);

            if (!string.IsNullOrEmpty(configuration))
                return (ITriggerConfiguration) _configurationSerializer.Deserialize(configuration,
                    triggerDescriptor.ConfigurationType);

            foreach (var triggerConfiguration in TriggerConfigurations)
                if (triggerConfiguration.Metadata.Type == triggerDescriptor.ConfigurationType)
                    return triggerConfiguration.Value;

            return null;
        }

        public IStep LoadStep(ICatalogProvider catalogProvider, Guid extensionId, IStepConfiguration configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeExportedValue("Configuration", configuration);

            container.ComposeParts(this);

            var stepDescriptor = GetStepDescriptor(extensionId);

            foreach (var step in Steps)
                if (step.Metadata.Type == stepDescriptor.ExtensionType)
                    return step.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        public IStepConfiguration LoadStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId,
            string configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var stepDescriptor = GetStepDescriptor(extensionId);

            if (!string.IsNullOrEmpty(configuration))
                return (IStepConfiguration) _configurationSerializer.Deserialize(configuration,
                    stepDescriptor.ConfigurationType);

            foreach (var stepConfiguration in StepConfigurations)
                if (stepConfiguration.Metadata.Type == stepDescriptor.ConfigurationType)
                    return stepConfiguration.Value;

            return null;
        }

        public IConditionConfiguration LoadConditionConfiguration(ICatalogProvider catalogProvider, Guid extensionId,
            string configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var conditionDescriptor = GetConditionDescriptor(extensionId);

            if (!string.IsNullOrEmpty(configuration))
                return (IConditionConfiguration)_configurationSerializer.Deserialize(configuration,
                    conditionDescriptor.ConfigurationType);

            foreach (var conditionConfiguration in ConditionConfigurations)
                if (conditionConfiguration.Metadata.Type == conditionDescriptor.ConfigurationType)
                    return conditionConfiguration.Value;

            return null;
        }

        public ITriggerConfiguration CreateNewTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var triggerDescriptor = GetTriggerDescriptor(extensionId);

            foreach (var triggerConfiguration in TriggerConfigurations)
                if (triggerConfiguration.Metadata.Type == triggerDescriptor.ConfigurationType)
                    return triggerConfiguration.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        public IStepConfiguration CreateNewStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var stepDescriptor = GetStepDescriptor(extensionId);

            foreach (var stepConfiguration in StepConfigurations)
                if (stepConfiguration.Metadata.Type == stepDescriptor.ConfigurationType)
                    return stepConfiguration.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        public IConditionConfiguration CreateNewConditionConfiguration(ICatalogProvider catalogProvider, Guid extensionId)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            var conditionDescriptor = GetConditionDescriptor(extensionId);

            foreach (var conditionConfiguration in ConditionConfigurations)
                if (conditionConfiguration.Metadata.Type == conditionDescriptor.ConfigurationType)
                    return conditionConfiguration.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

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

        private IConditionDescriptor GetConditionDescriptor(Guid extensionId)
        {
            foreach (var i in ConditionDescriptors)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            throw new ExtensionNotFoundException(extensionId);
        }

        public IEnumerable<ITriggerDescriptor> GetTriggerDescriptors(ICatalogProvider catalogProvider)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            return TriggerDescriptors.Select(i => i.Value);
        }

        public IEnumerable<IStepDescriptor> GetStepDescriptors(ICatalogProvider catalogProvider)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            return StepDescriptors.Select(i => i.Value);
        }

        public IEnumerable<IConditionDescriptor> GetConditionDescriptors(ICatalogProvider catalogProvider)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            return ConditionDescriptors.Select(i => i.Value);
        }
    }
}