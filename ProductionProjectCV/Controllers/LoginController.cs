using Services;
using Services.AggregateModel;
using Services.Model;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineResumePortal.Controllers
{

    public class LoginController : Controller
    {
        ServicesBl serviceBl = new ServicesBl();
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(ResumeModel model)
        {
            try
            {
                var password = EncryptDecrypt.Encrypt(model.Password);
                var result = serviceBl.CheckLogin(model.Username, password);
                //return View();
                if (result)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    var loggedUser = serviceBl.GetSavedDetails(model.Username);
                    SessionData session = new SessionData
                    {
                        _id = loggedUser._id,
                        Category = loggedUser.Category,
                        Email = loggedUser.Email,
                        FullName = loggedUser.FullName,
                        ProfileImageName = loggedUser.ProfileImageName,
                        PublicName = loggedUser.PublicName,
                        Username = loggedUser.Username,
                        Privacy = loggedUser.Privacy,
                        Password=loggedUser.Password
                    };
                    Session.Add("LoggedUser", session);
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                    {
                        ViewBag.Message = "Username or Password cannot be emply!";
                    }
                    ViewBag.Message = "Invalid Username or Password";
                    return View("Index");
                }
                return RedirectToAction("Index", "Home");
            }

            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.Message = "Username or Password cannot be emply!";
                }
                if (ex.Message.Contains("MongoConnectionException"))
                {
                    ViewBag.Message = "Connection Issues!";
                }

                return View("Index");
            }


        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                var result = serviceBl.GetSavedDetails(model.Username);
                model.Password = result.Password;
                var mailresult = SendEmail.Send(model.Username, EncryptDecrypt.Decrypt(model.Password),model.Username);
                if (mailresult)
                {
                    ViewBag.Message = "Reset password is sent to ur email.";
                    return View("Index");
                }
                else
                {
                    ViewBag.Message = "Technical Issue, Please try again later.";
                    return View("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "No username/email in database found.";
                return View("Index");
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //[NonAction]
        //public string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            return RedirectToAction("Index", "Login");
        }

    }
}