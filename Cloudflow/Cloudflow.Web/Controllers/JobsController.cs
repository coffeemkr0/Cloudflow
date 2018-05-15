using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Cloudflow.Core;
using Cloudflow.Core.Conditions;
using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.ExtensionManagement;
using Cloudflow.Core.Serialization;
using Cloudflow.Web.ViewModels.Jobs;
using Cloudflow.Core.Steps;
using Cloudflow.Core.Triggers;
using Cloudflow.Web.Utility;

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

        private JsonResult GetNewTriggerResult(Guid extensionId, string assemblyPath, ITriggerConfiguration configuration)
        {
            var triggerViewModel = new TriggerViewModel
            {
                TriggerDefinitionId = Guid.NewGuid(),
                ConfigurationViewModel = new TriggerConfigurationViewModel
                {
                    ExtensionId = extensionId,
                    AssemblyPath = assemblyPath,
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
                ConfigurationViewModel = new StepConfigurationViewModel
                {
                    ExtensionId = extensionId,
                    AssemblyPath = extensionAssemblyPath,
                    Configuration = configuration
                }
            };

            var navigationItem = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_StepNavigationItem", stepViewModel);

            var itemEdit = ViewHelpers.RenderRazorViewToString(ControllerContext,
            "_StepEdit", stepViewModel);

            return Json(new { navigationItem, itemEdit });
        }

        private JsonResult GetNewConditionResult(Guid extensionId, string extensionAssemblyPath, 
            IConditionConfiguration configuration, string propertyName)
        {
            var conditionViewModel = new ConditionViewModel
            {
                ConditionDefinitionId = Guid.NewGuid(),
                PropertyName = propertyName,
                //ExtensionConfiguration = new ConditionConfigurationViewModel
                //{
                //    ExtensionId = extensionId,
                //    AssemblyPath = extensionAssemblyPath,
                //    Configuration = configuration
                //}
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
            var model = new JobDefinition();
            model.Name = "New Job";

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

            return RedirectToAction("EditJob", new { editJobViewModel.JobDefinition.JobDefinitionId });
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
            var assemblyCatalogProvider = new AssemblyCatalogProvider(extensionAssemblyPath);
            var extensionService = new ExtensionService(new JsonConfigurationSerializer());

            switch (extensionType)
            {
                case ConfigurableExtensionTypes.Trigger:
                    var triggerConfiguration =
                        extensionService.CreateNewTriggerConfiguration(assemblyCatalogProvider, extensionId);
                    return GetNewTriggerResult(extensionId, extensionAssemblyPath, triggerConfiguration);
                case ConfigurableExtensionTypes.Step:
                    var stepConfiguration =
                        extensionService.CreateNewStepConfiguration(assemblyCatalogProvider, extensionId);
                    return GetNewStepResult(extensionId, extensionAssemblyPath, stepConfiguration);
                case ConfigurableExtensionTypes.Condition:
                    return GetNewConditionResult(extensionId, extensionAssemblyPath, null, propertyName);
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}
