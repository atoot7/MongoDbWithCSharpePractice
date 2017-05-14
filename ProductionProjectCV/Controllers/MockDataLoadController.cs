using System.Web.Mvc;
using System.IO;
using OnlineResumePortal;

namespace ProductionProjectCV.Controllers
{
    public class MockDataLoadController : Controller
    {
        // GET: MockDataLoad
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MockData()
        {
            //Services.LoadMockData mockBl = new Services.LoadMockData();
            //mockBl.TestMockDataLoad();
            Services.LoadMockData loaddata = new Services.LoadMockData();
            loaddata.TestMockDataLoad();
            return RedirectToAction("Index");
        }
    }
}