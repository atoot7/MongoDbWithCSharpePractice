using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;

namespace OnlineResumePortal.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        ServicesBl serviceBl = new ServicesBl();
        // GET: Home
        public ActionResult Index()
        {
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            if (string.IsNullOrEmpty(loggedUser.ProfileImageName) || string.IsNullOrEmpty(loggedUser.PublicName) || string.IsNullOrEmpty(loggedUser.FullName))
            {
                var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
                loggedUser.Username = getInfo.Username;
                loggedUser._id = getInfo._id;
                loggedUser.PublicName = getInfo.PublicName;
                loggedUser.FullName = getInfo.FullName;
                loggedUser.Email = getInfo.Email;
                loggedUser.Category = getInfo.Category;
                loggedUser.ProfileImageName = getInfo.ProfileImageName;
                loggedUser.Password = getInfo.Password;
                loggedUser.Privacy = getInfo.Privacy;
                Session["LoggedUser"] = loggedUser;
            }
            return View();
        }
    }
}