using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cloudflow.Core.Data;
using Cloudflow.Core.Data.Models;

namespace Cloudflow.Web.Controllers
{
    public class AgentConfigurationsController : Controller
    {
        private CoreDbContext db = new CoreDbContext();

        // GET: AgentConfigurations
        public ActionResult Index()
        {
            return View(db.AgentConfigurations.ToList());
        }

        // GET: AgentConfigurations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentConfiguration agentConfiguration = db.AgentConfigurations.Find(id);
            if (agentConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(agentConfiguration);
        }

        // GET: AgentConfigurations/Create
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: AgentConfigurations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Enabled,MachineName")] AgentConfiguration agentConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.AgentConfigurations.Add(agentConfiguration);
                db.SaveChanges();
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
            AgentConfiguration agentConfiguration = db.AgentConfigurations.Find(id);
            if (agentConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(agentConfiguration);
        }

        // POST: AgentConfigurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Enabled,MachineName")] AgentConfiguration agentConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentConfiguration);
        }

        // GET: AgentConfigurations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgentConfiguration agentConfiguration = db.AgentConfigurations.Find(id);
            if (agentConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(agentConfiguration);
        }

        // POST: AgentConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentConfiguration agentConfiguration = db.AgentConfigurations.Find(id);
            db.AgentConfigurations.Remove(agentConfiguration);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
