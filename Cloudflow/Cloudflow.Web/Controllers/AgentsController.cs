using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Server.Models;
using Cloudflow.Web.ViewModels.Agents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class AgentsController : Controller
    {
        private ServerDbContext _serverDbContext = new ServerDbContext();

        // GET: Agents
        public ActionResult Index()
        {
#if DEBUG
            var serverPath = Server.MapPath("~").TrimEnd(Path.DirectorySeparatorChar);
            var extensionsAssemblyPath = Directory.GetParent(serverPath).FullName;
            extensionsAssemblyPath = Path.Combine(extensionsAssemblyPath, @"Cloudflow.Extensions\bin\debug\Cloudflow.Extensions.dll");
            _serverDbContext = new ServerDbContext(true, extensionsAssemblyPath);
#else
            _databaseContext = new ServerDbContext();
#endif

            IndexViewModel model = new IndexViewModel();

            model.AgentConfigurations.AddRange(_serverDbContext.AgentConfigurations.ToList());

            return View(model);
        }

        // GET: Agents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentConfiguration agentConfiguration = _serverDbContext.AgentConfigurations.Find(id);
            if (agentConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(agentConfiguration);
        }

        // GET: Agents/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Agents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Enabled,MachineName,Port")] AgentConfiguration agentConfiguration)
        {
            if (ModelState.IsValid)
            {
                _serverDbContext.AgentConfigurations.Add(agentConfiguration);
                _serverDbContext.SaveChanges();
                return Json(new { success = true });
            }

            return PartialView("_Create", agentConfiguration);
        }

        // GET: Agents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentConfiguration agentConfiguration = _serverDbContext.AgentConfigurations.Find(id);
            if (agentConfiguration == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", agentConfiguration);
        }

        // POST: Agents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Enabled,MachineName,Port")] AgentConfiguration agentConfiguration)
        {
            if (ModelState.IsValid)
            {
                _serverDbContext.Entry(agentConfiguration).State = EntityState.Modified;
                _serverDbContext.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("_Edit", agentConfiguration);
        }

        // GET: Agents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentConfiguration agentConfiguration = _serverDbContext.AgentConfigurations.Find(id);
            if (agentConfiguration == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", agentConfiguration);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentConfiguration agentConfiguration = _serverDbContext.AgentConfigurations.Find(id);
            _serverDbContext.AgentConfigurations.Remove(agentConfiguration);
            _serverDbContext.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult DownloadAgent()
        {
            var agentFilesFolder = Server.MapPath("~/AgentFiles");
            var agentZipFilePath = Path.Combine(Path.GetTempFileName());
            if (System.IO.File.Exists(agentZipFilePath))
            {
                System.IO.File.Delete(agentZipFilePath);
            }

            ZipFile.CreateFromDirectory(agentFilesFolder, agentZipFilePath);

            return File(agentZipFilePath, "application/zip", "Agent.zip");
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
