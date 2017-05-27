using Cloudflow.Core.Data;
using Cloudflow.Core.Data.Server;
using Cloudflow.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}