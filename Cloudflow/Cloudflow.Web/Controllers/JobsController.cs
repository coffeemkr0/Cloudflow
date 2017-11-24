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

        public ActionResult EditJob(Guid? id)
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
            return View(EditJobViewModel.FromJobDefinition(jobDefinition, this.GetExtensionLibraryFolder()));
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

        [HttpPost]
        public JsonResult CreateObjectCollectionItem(string propertyName, string objectFactoryAssemblyPath, string objectFactoryExtensionId,
            string factoryData, string instanceData)
        {
            var objectFactoryController = new ObjectFactoryController(objectFactoryAssemblyPath,
                Guid.Parse(objectFactoryExtensionId), factoryData);
            var newItem = objectFactoryController.CreateObject(instanceData);

            var model = new ObjectCollectionItemViewModel
            {
                DisplayText = JobEditorHelper.GetDisplayText(newItem),
                PropertyNameParts = (propertyName + "[0]").Split('.').ToList(),
                Value = newItem
            };

            var navigationItemView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
            "~/Views/ExtensionConfigurationEdits/ObjectCollectionNavigationItem.cshtml", model);

            var itemView = ViewHelpers.RenderRazorViewToString(this.ControllerContext,
            "~/Views/ExtensionConfigurationEdits/ObjectCollectionItem.cshtml", model);

            return Json(new { navigationItemView, itemView });
        }
        #endregion
    }
}
