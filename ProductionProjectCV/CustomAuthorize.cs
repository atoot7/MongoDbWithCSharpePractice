using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResumePortal
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    //you can change to any controller or html page.
        //    filterContext.Result = new RedirectResult("/UnAuthorize/Index");

        //}
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session?["LoggedUser"] != null)
            {
                var session = httpContext.Session["LoggedUser"];
                return true;
            }
            httpContext.Response.Redirect("~/Login/Index");
            return false;
        }
    }
}