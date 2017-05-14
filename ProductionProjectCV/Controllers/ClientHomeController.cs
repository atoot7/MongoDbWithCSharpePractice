using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;

namespace OnlineResumePortal.Controllers
{
    [AllowAnonymous]
    public class ClientHomeController : Controller
    {
        ServicesBl serviceBl = new ServicesBl();
        // GET: ClientHome
        public ActionResult Index()
        {
            var info = serviceBl.HomePageInfo();
            return View(info);
        }
    }
}