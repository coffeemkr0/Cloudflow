# Cloudflow

<h2>Overview</h2>
Cloudflow is an automation framework, or workflow system, that is configured and managed through a web application yet it performs the actual work on agents that can be installed on local computers.

<h2>Terminology</h2>
<dl>
  <dt>Job</dt>
    <dd>A job is the container for a configured workflow. It contains all of the configuration that is needed to process the workflow, such as triggers and steps.</dd>
  <dt>Trigger</dt>
    <dd>A trigger is something that initiates a workflow. Triggers can be fired asynchronously for the same job.</dd>
  <dt>Step</dt>
    <dd>A step is part of a workflow that performs an action. Steps are executed sequentially.</dd>
  <dt>Agent</dt>
    <dd>An agent is a program that is executed on local PC. The agent is the program that does the actual work that is configured in a job.</dd>
  <dt>Run</dt>
    <dd>A run is what is created when a trigger initiates a workflow.</dd>
  <dt>SignalR</dt>
    <dd>SignarlR is the Microsoft technology that is used to communicate between the web application's client side javascript and the local agents.</dd>
  <dt>Extensible</dt>
    <dd>Cloudflow is meant to be as extensible as possible. Each part of a job is implemented as a class that can be extended in a custom assembly.</dd>
  <dt>Configurable Extension</dt>
    <dd>A configurable extension is an extensible class that has a configuration propety that is also an extensible class. For example, a Step is an extensible class that has an ExtensionConfiguration property that is also extensible.</dd>
  <dt>Extension Attributes</dt>
    <dd>There are attribute classes that are implemented in Cloudflow so that they can be used to decorate propeties in an extension. These attributes can be used to control how the configuration for an extension is rendered in the web application.</dd>
</dl>

<h2>Projects</h2>
<h3>Cloudflow.Web</h3>
This is the ASP.NET MVC web application that a user interacts with to configure jobs and agents.

<h3>Cloudflow.Core</h3>
A class library that contains the core functionality of the application.

<h3>Cloudflow.Agent.Desktop</h3>
A simple console application that hosts an instance of the Agent class that is implemented in the Cloudflow.Core project.

<h3>Cloudflow.Agent.WindowsService</h3>
A simple windows service that hosts an instance of the Agent class that is implemented in the Cloudflow.Core project.

<h3>Cloudflow.Extensions</h3>
A class library that contains implementations of extensions, such as jobs, triggers and steps. Cloudflow does not implement any classes directly in the core project, every part of the workflow is treated as an extension. This project is basically treated like a custom assembly that a developer created that is just always available for use for every user.

<h3>Cloudflow.Agent.Setup</h3>
A simple windows forms application that can be ran on a PC to install/setup an agent on a PC.
