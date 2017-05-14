using Services;
using Services.AggregateModel;
using Services.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResumePortal.Controllers
{
    [CustomAuthorize]
    public class ManageProfileController : Controller
    {
        ServicesBl serviceBl = new ServicesBl();
        // GET: ManageProfile
        [HttpGet]
        public ActionResult Index()
        {
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
            return View(getInfo);
        }
        [HttpPost]
        public ActionResult Index(ResumeModel instance, HttpPostedFileBase ImageFile)
        {
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);

            if (!string.IsNullOrEmpty(instance.PublicName))
            {
                var model = serviceBl.GetPublicResume(instance.PublicName);
                if (model != null)
                {
                    if (model._ResumeId == new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        getInfo.PublicName = instance.PublicName;
                    }
                }
                else
                {
                    getInfo.PublicName = instance.PublicName;
                }
            }
            else
            {
                ModelState.AddModelError("PublicName", "Public already taken, try differen Name!");
                return View(getInfo);
            }


            getInfo.FullName = instance.FullName;

            string extension = "";
            if (ImageFile != null)
            {
                bool folderExists = Directory.Exists(Server.MapPath("~/Image/ResumeImages/"));
                if (!folderExists)
                {
                    Directory.CreateDirectory(Server.MapPath("~/Image/ResumeImages/"));
                }
                string path = Server.MapPath("~/Image/ResumeImages/");
                path = string.Format("{0}{1}.jpg", path, getInfo._id.ToString());
                ImageFile.SaveAs(path);

                extension = Path.GetExtension(path);
                getInfo.ProfileImageName = getInfo._id.ToString() + extension;
                //ImageFile.SaveAs()
            }



            var result = serviceBl.DoUpsertAsync(getInfo);
            if (result.Exception == null)
            {
                ViewBag.Message = "Progress Saved!";
            }
            else
            {
                ViewBag.Message = "Something went wrong!";
            }
            var attach_model = new ResumeModel();
            return View(attach_model);
        }

        public ActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ViewBag.Message = "Confirm password doesnot match!";
                    return View();
                }
                else
                {
                    var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
                    var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
                    if (model.Password != EncryptDecrypt.Decrypt(getInfo.Password))
                    {
                        ViewBag.Message = "Wrong password! Please try again.";
                        return View();
                    }
                    getInfo.Password = EncryptDecrypt.Encrypt(model.NewPassword);
                    var result = serviceBl.DoUpsertAsync(getInfo);
                    if (result.Exception == null)
                    {
                        ViewBag.Message = "Progress Saved!";
                        ChangePasswordModel cmodel = new ChangePasswordModel();
                        return View(cmodel);
                    }
                    else
                    {
                        ViewBag.Message = "Something Went Wrong!";
                        return View();
                    }
                }
            }
            ViewBag.Message = "Please Fill All Required fields!";
            return View();
        }

        public ActionResult SetVisibility()
        {
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
            VisibilitySettingModel model = new VisibilitySettingModel();
            model = getInfo.Privacy;
            return View(model);
        }

        [HttpPost]
        public ActionResult SetVisibility(VisibilitySettingModel model)
        {
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
            getInfo.Privacy = model;
            var result = serviceBl.DoUpsertAsync(getInfo);
            if (result.Exception == null)
            {
                ViewBag.Message = "Privacy Updated";
                return View(model);
            }
            else
            {
                ViewBag.Message = "Privacy was not updated! Something went wrong, consult tech team.";
                return View(model);
            }
        }
    }
}