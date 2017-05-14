using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OnlineResumePortal
{
    public class LoadMockData
    {
        public void TestMockDataLoad()
        {
            var lines = File.ReadAllLines(@"C:\MOCK_DATA.csv").Select(a => a.Split(';'));
            var csv = (from line in lines
                       select (from col in line
                               select col).ToArray()).Skip(1).ToArray();
            csv.Skip(1);
            foreach (var item in csv)
            {
                IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();

                var filter1 = Builders<ResumeModel>.Filter.Eq("Username", resume.Username) | Builders<ResumeModel>.Filter.Eq("Email", resume.Email);
                var filter2 = Builders<ResumeModel>.Filter.Eq("PublicName", resume.PublicName);
                var result1 = collection.Find(filter1);
                if (result1.Count() > 0)
                {
                    msg = "Username/Email already exists";
                    return false;
                }
                var result2 = collection.Find(filter2);
                if (result2.Count() > 0)
                {
                    msg = "Public name already exists;";
                    return false;
                }
                collection.InsertOne(resume);
                msg = "";
            }
        }
    }
}