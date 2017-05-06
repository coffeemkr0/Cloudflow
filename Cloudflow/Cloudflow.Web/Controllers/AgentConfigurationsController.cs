using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class AgentConfigurationsController : Controller
    {
        private ServerDbContext _serverDbContext = new ServerDbContext();

        // GET: AgentConfigurations/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: AgentConfigurations/Create
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

        // GET: AgentConfigurations/Edit/5
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

        // POST: AgentConfigurations/Edit/5
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

        // GET: AgentConfigurations/Delete/5
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

        // POST: AgentConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentConfiguration agentConfiguration = _serverDbContext.AgentConfigurations.Find(id);
            _serverDbContext.AgentConfigurations.Remove(agentConfiguration);
            _serverDbContext.SaveChanges();
            return Json(new { success = true });
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
