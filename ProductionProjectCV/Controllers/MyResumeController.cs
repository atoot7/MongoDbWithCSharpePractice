using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineResumePortal.Controllers
{
    [CustomAuthorize]
    public class MyResumeController : Controller
    {
        //Services.Model.SessionData loggedUser = new Services.Model.SessionData();
        Services.ServicesBl serviceBl = null;
        public MyResumeController()
        {
            serviceBl = new Services.ServicesBl();
        }


        public ActionResult Preview(Guid resumeId)
        {
            Services.Model.SessionData loggedUser = new Services.Model.SessionData();
            var model = new Services.Model.ResumeDetails();
            loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
            //var guid = new Guid(resumeId);
            var resumePreview = getInfo.Resumes.Where(x => x._ResumeId == resumeId).FirstOrDefault();
            ViewBag.ResumePreview = resumePreview;
            return View(resumePreview);
        }
        public ActionResult PublicPreview(Guid resumeId)
        {
            var resummodel = serviceBl.FindResume(resumeId);
            var model = resummodel.Resumes.FirstOrDefault(x => x._ResumeId == resumeId);
            var profilePrivacy = resummodel.Privacy;
            if (profilePrivacy != null)
            {
                model.Privacy = profilePrivacy;
            }
            else
            {
                model.Privacy = new Services.Model.VisibilitySettingModel();
            }
            return View("Preview", model);
        }

        // GET: MyResume
        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult Index()
        {
            Services.Model.SessionData loggedUser = new Services.Model.SessionData();
            loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
            if (getInfo.Resumes != null)
            {
                var resumelist = getInfo.Resumes;
                ViewBag.ResumeList = resumelist;
            }
            return View();
        }
        public ActionResult Form()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Services.Model.ResumeDetails model, HttpPostedFileBase ImageFile)
        {
            if (string.IsNullOrEmpty(model.ResumeTitle))
            {
                ModelState.AddModelError("Title", "Title is requiried!");
                return View("Form", model);
            }
            var LoggerUser = new Services.Model.SessionData();
            LoggerUser = (Services.Model.SessionData)Session["LoggedUser"];
            var resumeresult = serviceBl.GetSavedDetails(LoggerUser.Username).Resumes;
            if (model.SetActive != null)
            {
                if (model.SetActive == "on")
                {
                    if (resumeresult != null)
                    {
                        foreach (var r in resumeresult)
                        {
                            r.SetActive = "Off";
                        }
                    }
                }
            }
            else
            {
                model.SetActive = "off";
            }
            var loguser = new Services.Model.ResumeModel()
            {
                _id = LoggerUser._id,
                Username = LoggerUser.Username,
                PublicName = LoggerUser.PublicName,
                Email = LoggerUser.Email,
                Privacy = LoggerUser.Privacy,
                Category = LoggerUser.Category,
                FullName = LoggerUser.FullName,
                ProfileImageName = LoggerUser.ProfileImageName,
                Password = LoggerUser.Password

            };
            loguser._id = LoggerUser._id;
            loguser.Resumes = resumeresult;
            //LoggerUser.Resumes = resumeresult;
            if (model != null)
                model._ResumeId = Guid.NewGuid();
            string extension = "";
            if (ImageFile != null)
            {
                bool folderExists = Directory.Exists(Server.MapPath("~/Image/ResumeImages/"));
                if (!folderExists)
                {
                    Directory.CreateDirectory(Server.MapPath("~/Image/ResumeImages/"));
                }
                string path = Server.MapPath("~/Image/ResumeImages/");
                path = string.Format("{0}{1}.jpg", path, model._ResumeId.ToString());
                ImageFile.SaveAs(path);

                extension = Path.GetExtension(path);
                //ImageFile.SaveAs()
            }
            var ImageName = new Services.Model.Image();
            ImageName.Name = model._ResumeId.ToString() + extension;
            model.Image = ImageName;
            if (model.EducationInfos != null)
            {
                model.EducationInfos.FirstOrDefault()._EducationId = Guid.NewGuid();
            }
            if (model.JobHistories != null)
            {
                model.JobHistories.FirstOrDefault()._HistoryId = Guid.NewGuid();
            }
            if (model.Skills != null)
            {
                model.Skills.FirstOrDefault()._SkillId = Guid.NewGuid();
            }
            if (model.Languages != null)
            {
                model.Languages.FirstOrDefault()._LanguageId = Guid.NewGuid();
            }
            if (loguser.Resumes == null)
            {
                loguser.Resumes = new List<Services.Model.ResumeDetails>();
            }
            loguser.Resumes.Add(model);
            var result = serviceBl.DoUpsertAsync(loguser);
            if (result.Exception == null)
            {
                ViewBag.Message = "Progress Saved!";
                var getInfo = serviceBl.GetSavedDetails(LoggerUser.Username);
                if (getInfo.Resumes != null)
                {
                    var resumelist = getInfo.Resumes;
                    ViewBag.ResumeList = resumelist;
                }
                return View("Index");
            }
            else
            {
                ViewBag.Message = "Something went wrong !Progress not Saved!";
                var getInfo = serviceBl.GetSavedDetails(LoggerUser.Username);
                if (getInfo.Resumes != null)
                {
                    var resumelist = getInfo.Resumes;
                    ViewBag.ResumeList = resumelist;
                }
                return View("Index");
            }
        }
        public ActionResult Delete(string Id)
        {
            var guid = new Guid(Id);
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);
            var resumes = getInfo.Resumes;
            var resumeTobeDeleted = resumes.Where(x => x._ResumeId == guid).FirstOrDefault();
            resumes.Remove(resumeTobeDeleted);
            getInfo.Resumes = resumes;
            var result = serviceBl.DoUpsertAsync(getInfo);
            if (result.Exception == null)
            {
                ViewBag.Message = "Progress Saved!";
                //return View("Index");
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Something went wrong !Progress not Saved!";
                //return View("Index");
                return RedirectToAction("Index");


            }
        }
        public ActionResult SetActive(string resumeId)
        {
            var guid = new Guid(resumeId);
            var loggedUser = (Services.Model.SessionData)HttpContext.Session?["LoggedUser"];
            var getInfo = serviceBl.GetSavedDetails(loggedUser.Username);

            foreach (var resumes in getInfo.Resumes)
            {
                if (resumes._ResumeId == guid)
                {
                    resumes.SetActive = "on";
                }
                else
                {
                    resumes.SetActive = "off";
                }
            }

            var result = serviceBl.DoUpsertAsync(getInfo);
            if (result.Exception == null)
            {
                ViewBag.Message = "Progress Saved!";
                //return View("Index");
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Something went wrong !Progress not Saved!";
                //return View("Index");
                return RedirectToAction("Index");


            }
        }
    }
}