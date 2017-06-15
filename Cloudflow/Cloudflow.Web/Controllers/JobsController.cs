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
using Cloudflow.Web.HtmlHelpers;
using Cloudflow.Core.Extensions.Controllers;

namespace Cloudflow.Web.Controllers
{
    public class JobsController : Controller
    {
        private ServerDbContext _serverDbContext = new ServerDbContext();

        private string GetTestExtensionAssemblyPath()
        {
            var serverPath = Server.MapPath("~").TrimEnd(Path.DirectorySeparatorChar);
            var extensionsAssemblyPath = Directory.GetParent(serverPath).FullName;
            return Path.Combine(extensionsAssemblyPath, @"Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
        }

        // GET: Jobs
        public ActionResult Index()
        {
#if DEBUG
            _serverDbContext = new ServerDbContext(true, GetTestExtensionAssemblyPath());
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
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobDefinitionId,JobConfigurationExtensionId,JobConfigurationExtensionAssemblyPath,Configuration")] JobDefinition jobDefinition)
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

        [HttpPost]
        public JsonResult AddTrigger(Guid triggerId, int index)
        {
            var extensionAssemblyPath = GetTestExtensionAssemblyPath();

            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionAssemblyPath);
            var trigger = configurableExtensionBrowser.GetTrigger(triggerId);

            var triggerConfigurationViewModel = new ExtensionConfigurationViewModel();
            triggerConfigurationViewModel.Id = Guid.NewGuid();
            triggerConfigurationViewModel.Index = index;
            triggerConfigurationViewModel.ExtensionId = Guid.Parse(trigger.ConfigurationId);
            triggerConfigurationViewModel.ExtensionAssemblyPath = extensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(Guid.Parse(trigger.ConfigurationId),
                extensionAssemblyPath);
            triggerConfigurationViewModel.Configuration = extensionConfigurationController.CreateNewConfiguration();
            triggerConfigurationViewModel.Configuration.ExtensionId = triggerId;
            triggerConfigurationViewModel.Configuration.ExtensionAssemblyPath = extensionAssemblyPath;
            triggerConfigurationViewModel.Configuration.Name = "New Trigger";

            var triggerNavigationItemView = Utility.RenderRazorViewToString(this.ControllerContext, 
                "_TriggerNavigationItem", triggerConfigurationViewModel);
            var triggerConfigurationView = Utility.RenderRazorViewToString(this.ControllerContext,
                "_TriggerConfiguration", triggerConfigurationViewModel);

            return Json(new { triggerNavigationItemView, triggerConfigurationView });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serverDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
