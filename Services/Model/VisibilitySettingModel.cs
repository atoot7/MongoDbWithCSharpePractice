using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Model
{
    public class VisibilitySettingModel
    {
        public bool ShowEmail { get; set; }
        public bool Address { get; set; }
        public bool Mobile { get; set; }
        public bool Website { get; set; }
        public bool Eduction { get; set; }
        public bool Jobs { get; set; }
        public bool Skills { get; set; }
    }
}
