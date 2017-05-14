using OnlineResumePortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionProjectCV.Controllers
{
    public class JsonReturnController : Controller
    {
        // GET: JsonReturn
        public JsonResult LevelList()
        {
            var obj = DropDownServicesEnum.LevelList().Select(x => new { name = x.Text, id = x.Value });
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}