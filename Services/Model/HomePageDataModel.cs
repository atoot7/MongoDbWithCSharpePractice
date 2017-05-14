using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Model
{
    public class HomePageDataModel
    {
        public int UserCount { get; set; }
        public int ResumeCount { get; set; }
        public int FirstTopCount { get; set; }
        public int SecoundTopCount { get; set; }
        public int ThirdTopCount { get; set; }
        public int FourthTopCount { get; set; }

        public string FirstTopCountName { get; set; }
        public string SecoundTopCountName { get; set; }
        public string ThirdTopCountName { get; set; }
        public string FourthTopCountName { get; set; }

    }
}
