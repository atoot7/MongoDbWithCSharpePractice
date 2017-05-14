using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OnlineResumePortal.Controllers
{
    public class ResumesController : Controller
    {
        ServicesBl serviceBl = new ServicesBl();
        // GET: Resumes
        public ActionResult Public (string id)
        {
            var model = serviceBl.GetPublicResume(id);
            if (model != null)
            {
                if (model._ResumeId == new Guid("00000000-0000-0000-0000-000000000000"))
                    return RedirectToAction("Index", "NotFound");
            }
            else {
                return RedirectToAction("Index", "NotFound");
            }
            return View("Index",model);
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult FindResumes(/*string queryString*/)
        {

            //var result = serviceBl.FindProfile(queryString);
            return View();
        }
        [CustomAuthorize]
        [HttpPost]
        public ActionResult FindResumes(string queryString)
        {
          var  loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var result = serviceBl.FindProfile(queryString, loggedUser.Username);
            //return View();
            return PartialView("_PartialFindResumes", result);

        }

       }
}