using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Services
{
    public class LoadMockData
    {
        public void TestMockDataLoad()
        {
            var lines = File.ReadAllLines(@"~\App_Data\MOCK_DATA.csv").Select(a => a.Split(';'));
            var csv = (from line in lines
                       select (from col in line
                               select col).Skip(1).ToArray() // skip the first column
                      ).Skip(2).ToArray();
        }
    }
}
