using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Web.ViewModels.Jobs;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Steps;
using Cloudflow.Web.Utility;
using Cloudflow.Web.ViewModels.ExtensionConfigurationEdits;
using Cloudflow.Web.Utility.HtmlHelpers;

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

        private JsonResult GetNewTriggerResult(IConfigurableExtensionMetaData extension, string extensionAssemblyPath, ExtensionConfiguration configuration)
        {
            var triggerViewModel = new TriggerViewModel
            {
                TriggerDefinitionId = Guid.NewGuid(),
                ExtensionConfiguration = new ExtensionConfigurationViewModel
                {
                    ExtensionId = Guid.Parse(extension.ExtensionId),
                    ExtensionAssemblyPath = extensionAssemblyPath,
                    ConfigurationExtensionId = Guid.Parse(extension.ConfigurationExtensionId),
                    ConfigurationExtensionAssemblyPath = extensionAssemblyPath,
                    Configuration = configuration
                }
            };

            var navigationItem = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_TriggerNavigationItem", triggerViewModel);

            var itemEdit = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_TriggerEdit", triggerViewModel);

            return Json(new { navigationItem, itemEdit });
        }

        private JsonResult GetNewStepResult(Guid extensionId, string extensionAssemblyPath, IStepConfiguration configuration)
        {
            var stepViewModel = new StepViewModel
            {
                StepDefinitionId = Guid.NewGuid(),
                ExtensionConfiguration = new ExtensionConfigurationViewModel
                {
                    ExtensionId = extensionId,
                    ExtensionAssemblyPath = extensionAssemblyPath,
                    Configuration = configuration
                }
            };

            var navigationItem = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_StepNavigationItem", stepViewModel);

            var itemEdit = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_StepEdit", stepViewModel);

            return Json(new { navigationItem, itemEdit });
        }

        private JsonResult GetNewConditionResult(IConfigurableExtensionMetaData extension, string extensionAssemblyPath, 
            ExtensionConfiguration configuration, string propertyName)
        {
            var conditionViewModel = new ConditionViewModel
            {
                ConditionDefinitionId = Guid.NewGuid(),
                PropertyName = propertyName,
                ExtensionConfiguration = new ExtensionConfigurationViewModel
                {
                    ExtensionId = Guid.Parse(extension.ExtensionId),
                    ExtensionAssemblyPath = extensionAssemblyPath,
                    ConfigurationExtensionId = Guid.Parse(extension.ConfigurationExtensionId),
                    ConfigurationExtensionAssemblyPath = extensionAssemblyPath,
                    Configuration = configuration
                }
            };

            var navigationItem = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_ConditionNavigationItem", conditionViewModel);

            var itemEdit = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_ConditionEdit", conditionViewModel);

            return Json(new { navigationItem, itemEdit });
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

            var jobDefinition = _serverDbContext.JobDefinitions.Find(id);
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

        public ActionResult EditJob(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jobDefinition = _serverDbContext.JobDefinitions.Find(id);
            if (jobDefinition == null)
            {
                return HttpNotFound();
            }
            return View("Edit", EditJobViewModel.FromJobDefinition(jobDefinition, this.GetExtensionLibraryFolder()));
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJob(EditJobViewModel editJobViewModel)
        {
            editJobViewModel.Save(_serverDbContext);

            return RedirectToAction("EditJob", new { editJobViewModel.JobDefinitionId });
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jobDefinition = _serverDbContext.JobDefinitions.Find(id);
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
            var jobDefinition = _serverDbContext.JobDefinitions.Find(id);
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
            using (var serverDbContext = new ServerDbContext())
            {
                serverDbContext.Configuration.ProxyCreationEnabled = false;
                var jobDefinition = serverDbContext.JobDefinitions.Where(i => i.JobDefinitionId == id).
                    Include(i => i.TriggerDefinitions).Include(i => i.StepDefinitions).FirstOrDefault();
                if (jobDefinition == null)
                {
                    return HttpNotFound();
                }

                return Json(jobDefinition, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddConfigurableExtension(Guid extensionId, string extensionAssemblyPath, 
            ConfigurableExtensionTypes extensionType, string propertyName)
        {
            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionAssemblyPath);
            var extension = configurableExtensionBrowser.GetConfigurableExtension(extensionId);

            var extensionConfigurationController = new ExtensionConfigurationController(Guid.Parse(extension.ConfigurationExtensionId), extensionAssemblyPath);
            var configuration = extensionConfigurationController.CreateNewConfiguration();
            configuration.Name = extension.ExtensionName;

            switch (extensionType)
            {
                case ConfigurableExtensionTypes.Trigger:
                    return GetNewTriggerResult(extension, extensionAssemblyPath, configuration);
                case ConfigurableExtensionTypes.Step:
                    return GetNewStepResult(extension, extensionAssemblyPath, configuration);
                case ConfigurableExtensionTypes.Condition:
                    return GetNewConditionResult(extension, extensionAssemblyPath, configuration, propertyName);
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}
