using Services;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineResumePortal.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        ServicesBl service = null;

        public RegisterController()
        {
            service = new ServicesBl();
        }
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ResumeModel model, string ConfirmPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Password != ConfirmPassword)
                    {
                        ViewBag.Message = "Confirm password doesnot match!";
                        return View();
                    }
                    //var password = MD5Hash(model.Password);
                    var password = EncryptDecrypt.Encrypt(model.Password);

                    //var dPassword = EncryptDecrypt.Decrypt(password);
                    var temppassword = model.Password;
                    model.Password = password;
                    model.Username = model.Email;
                    string msg;
                    var registerResult = service.Register(model, out msg);

                    if (!registerResult)
                    {
                        ViewBag.Message = msg;
                        return View();
                    }
                    var emailresult = SendEmail.Send(model.Username, temppassword,model.Username);
                    if (emailresult)
                    {
                        ViewBag.Message = "Welcome to resume portal! An email is sent to your email address.";
                    }
                    else
                    {
                        ViewBag.Message = "Welcome to resume portal! Due to some technical dificulties, email could not be sent!";
                    }
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    SessionData session = new SessionData
                    {
                        _id = model._id,
                        Password = model.Password,
                        Privacy = model.Privacy,
                        Category = model.Category,
                        Email = model.Email,
                        FullName = model.FullName,
                        ProfileImageName = model.ProfileImageName,
                        PublicName = model.PublicName,
                        Username = model.Username
                    };
                    Session.Add("LoggedUser", session);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Please Fill all required all fields!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}