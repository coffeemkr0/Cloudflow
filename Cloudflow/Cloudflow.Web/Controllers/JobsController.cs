using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Web.ViewModels.Jobs;
using System.IO;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions;
using Cloudflow.Web.Utility;
using Cloudflow.Web.ViewModels.Shared;

namespace Cloudflow.Web.Controllers
{
    public class JobsController : Controller
    {
        #region Private Members
        private ServerDbContext _serverDbContext = new ServerDbContext();
        #endregion

        #region Private Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serverDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Actions
        // GET: Jobs
        public ActionResult Index()
        {
#if DEBUG
            _serverDbContext = new ServerDbContext(true, this.GetExtensionLibraries().First());
#else
            _databaseContext = new ServerDbContext();
#endif

            var model = new IndexViewModel();
            model.AgentConfigurations.AddRange(_serverDbContext.AgentConfigurations.ToList());
            foreach (var jobDefinition in _serverDbContext.JobDefinitions)
            {
                model.JobSummaries.Add(new JobSummaryViewModel(jobDefinition));
            }
            return View(model);
        }

        // GET: Jobs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobDefinition jobDefinition = _serverDbContext.JobDefinitions.Find(id);
            if (jobDefinition == null)
            {
                return HttpNotFound();
            }

            return View(jobDefinition);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            //TODO:Replace with a job selector
            var model = new JobDefinition();
            model.ExtensionId = Guid.Parse("3F6F5796-E313-4C53-8064-747C1989DA99");
            model.ExtensionAssemblyPath = this.GetExtensionLibraries().First();
            model.ConfigurationExtensionId = Guid.Parse("62A56D5B-07E5-41A3-A637-5E7C53FCF399");
            model.ConfigurationExtensionAssemblyPath = this.GetExtensionLibraries().First();

            var jobConfigurationController = new ExtensionConfigurationController(model.ConfigurationExtensionId, model.ConfigurationExtensionAssemblyPath);

            var jobConfiguration = jobConfigurationController.CreateNewConfiguration();
            jobConfiguration.Name = "New Job";

            model.Configuration = jobConfiguration.ToJson();

            return View(model);
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExtensionId,ExtensionAssemblyPath,ConfigurationExtensionId,ConfigurationExtensionAssemblyPath,Configuration")] JobDefinition jobDefinition)
        {
            if (ModelState.IsValid)
            {
                jobDefinition.JobDefinitionId = Guid.NewGuid();
                _serverDbContext.JobDefinitions.Add(jobDefinition);
                _serverDbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobDefinition);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDefinition jobDefinition = _serverDbContext.JobDefinitions.Find(id);
            if (jobDefinition == null)
            {
                return HttpNotFound();
            }
            return View(EditViewModel.FromJobDefinition(jobDefinition));
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel editViewModel)
        {
            editViewModel.Save(_serverDbContext);

            return RedirectToAction("Edit", new { editViewModel.JobConfigurationViewModel.Id });
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobDefinition jobDefinition = _serverDbContext.JobDefinitions.Find(id);
            if (jobDefinition == null)
            {
                return HttpNotFound();
            }
            return View(jobDefinition);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            JobDefinition jobDefinition = _serverDbContext.JobDefinitions.Find(id);
            _serverDbContext.JobDefinitions.Remove(jobDefinition);
            _serverDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult JobDefinition(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (ServerDbContext serverDbContext = new ServerDbContext())
            {
                serverDbContext.Configuration.ProxyCreationEnabled = false;
                JobDefinition jobDefinition = serverDbContext.JobDefinitions.Where(i => i.JobDefinitionId == id).
                    Include(i => i.TriggerDefinitions).Include(i => i.StepDefinitions).FirstOrDefault();
                if (jobDefinition == null)
                {
                    return HttpNotFound();
                }

                return Json(jobDefinition, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Triggers()
        {
            var model = new ExtensionBrowserViewModel(this.GetExtensionLibrariesPath(), ConfigurableExtensionTypes.Trigger);
            return PartialView("ExtensionBrowser", model);
        }

        public ActionResult Steps()
        {
            var model = new ExtensionBrowserViewModel(this.GetExtensionLibrariesPath(), ConfigurableExtensionTypes.Step);
            return PartialView("ExtensionBrowser", model);
        }

        public ActionResult Conditions(string viewModelPropertyName)
        {
            var model = new ExtensionBrowserViewModel(this.GetExtensionLibrariesPath(), ConfigurableExtensionTypes.Condition);
            return PartialView("ExtensionBrowser", model);
        }

        [HttpPost]
        public JsonResult AddTrigger(Guid triggerId, int index)
        {
            var extensionAssemblyPath = this.GetExtensionLibraries().First();

            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionAssemblyPath);
            var trigger = configurableExtensionBrowser.GetConfigurableExtension(triggerId);
            
            var triggerViewModel = new TriggerViewModel();
            triggerViewModel.Id = Guid.NewGuid();
            triggerViewModel.Index = index;
            triggerViewModel.ExtensionId = Guid.Parse(trigger.ExtensionId);
            triggerViewModel.ExtensionAssemblyPath = extensionAssemblyPath;
            triggerViewModel.ConfigurationExtensionId = Guid.Parse(trigger.ConfigurationExtensionId);
            triggerViewModel.ConfigurationExtensionAssemblyPath = extensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(Guid.Parse(trigger.ConfigurationExtensionId),
                extensionAssemblyPath);

            triggerViewModel.Configuration = extensionConfigurationController.CreateNewConfiguration();
            triggerViewModel.Configuration.Name = "New Trigger";

            var triggerNavigationItemView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
                "_TriggerNavigationItem", triggerViewModel);
            var triggerView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
                "_Trigger", triggerViewModel);

            return Json(new { triggerNavigationItemView, triggerView });
        }

        [HttpPost]
        public JsonResult AddStep(Guid stepId, int index)
        {
            var extensionAssemblyPath = this.GetExtensionLibraries().First();

            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionAssemblyPath);
            var step = configurableExtensionBrowser.GetConfigurableExtension(stepId);

            var stepConfigurationViewModel = new ExtensionConfigurationViewModel();
            stepConfigurationViewModel.Id = Guid.NewGuid();
            stepConfigurationViewModel.Index = index;
            stepConfigurationViewModel.ExtensionId = Guid.Parse(step.ExtensionId);
            stepConfigurationViewModel.ExtensionAssemblyPath = extensionAssemblyPath;
            stepConfigurationViewModel.ConfigurationExtensionId = Guid.Parse(step.ConfigurationExtensionId);
            stepConfigurationViewModel.ConfigurationExtensionAssemblyPath = extensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(Guid.Parse(step.ConfigurationExtensionId),
                extensionAssemblyPath);

            stepConfigurationViewModel.Configuration = extensionConfigurationController.CreateNewConfiguration();
            stepConfigurationViewModel.Configuration.Name = "New Step";


            var stepNavigationItemView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
                "_StepNavigationItem", stepConfigurationViewModel);
            var stepConfigurationView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
                "_StepConfiguration", stepConfigurationViewModel);

            return Json(new { stepNavigationItemView, stepConfigurationView });
        }

        [HttpPost]
        public JsonResult AddCondition(Guid conditionId, int index, string viewModelPropertyName)
        {
            var extensionAssemblyPath = this.GetExtensionLibraries().First();

            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionAssemblyPath);
            var condition = configurableExtensionBrowser.GetConfigurableExtension(conditionId);

            var conditionConfigurationViewModel = new ConditionConfigurationViewModel
            {
                ViewModelPropertyName = viewModelPropertyName
            };
            conditionConfigurationViewModel.Id = Guid.NewGuid();
            conditionConfigurationViewModel.Index = index;
            conditionConfigurationViewModel.ExtensionId = Guid.Parse(condition.ExtensionId);
            conditionConfigurationViewModel.ExtensionAssemblyPath = extensionAssemblyPath;
            conditionConfigurationViewModel.ConfigurationExtensionId = Guid.Parse(condition.ConfigurationExtensionId);
            conditionConfigurationViewModel.ConfigurationExtensionAssemblyPath = extensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(Guid.Parse(condition.ConfigurationExtensionId),
                extensionAssemblyPath);

            conditionConfigurationViewModel.Configuration = extensionConfigurationController.CreateNewConfiguration();
            conditionConfigurationViewModel.Configuration.Name = "New Condition";


            var conditionNavigationItemView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
                "_ConditionNavigationItem", conditionConfigurationViewModel);
            var conditionConfigurationView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
                "_ConditionConfiguration", conditionConfigurationViewModel);

            return Json(new { conditionNavigationItemView, conditionConfigurationView });
        }
        #endregion
    }
}
