using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Services;
using Services.Model;

namespace OnlineResumePortal
{
    public class DropDownServicesEnum
    {
        public static IEnumerable<SelectListItem> CategoryList()
        {
            var returnlist = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Choose Category" } };
            var statuses = from ProfileCategoryEnum s in Enum.GetValues(typeof(ProfileCategoryEnum))
                           select new { ID = s.ToString(), Name = s.ToString() };
            foreach(var item in statuses)
            {
                returnlist.Add(new SelectListItem { Value = item.ID, Text = item.Name });
            }
            return returnlist;
        }
        public static IEnumerable<SelectListItem> LevelList()
        {
            var returnlist = new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Choose Level" } };
            var statuses = from LevelEnum s in Enum.GetValues(typeof(LevelEnum))
                           select new { ID = s.ToString(), Name = s.ToString() };
            foreach (var item in statuses)
            {
                returnlist.Add(new SelectListItem { Value = item.ID, Text = item.Name });
            }
            return returnlist;
        }
    }
}
