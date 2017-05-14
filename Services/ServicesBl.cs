using MongoDB.Bson;
using MongoDB.Driver;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ServicesBl
    {
        public bool Register(ResumeModel resume, out string msg)
        {
            try
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
                return true;
            }
            catch (MongoException)
            {
                msg = "Somehthing went wrong";
                return false;
            }
        }

        public bool CheckLogin(string Username, string Password)
        {
            try
            {
                IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
                var filter = Builders<ResumeModel>.Filter.Eq("Username", Username) & Builders<ResumeModel>.Filter.Eq("Password", Password);
                var result = collection.Find(filter);
                return result.Count() > 0;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        //public async void Update(ResumeModel resume)
        //{
        //    IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
        //    var filter = Builders<ResumeModel>.Filter.Eq("_id", ObjectId.Parse(resume._id.ToString()));
        //    var result = await collection.ReplaceOneAsync(filter, resume);
        //}
        public async Task<bool> DoUpsertAsync(ResumeModel resume)
        {
            IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
            var filter = Builders<ResumeModel>.Filter.Eq("_id", ObjectId.Parse(resume._id.ToString()));

            var result = await collection.ReplaceOneAsync(
                filter: new BsonDocument("_id", resume._id),
                options: new UpdateOptions
                {
                    IsUpsert = true
                },
                            replacement: resume);
            if (result.ModifiedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public async Task<bool> DoUpsertResumeAsync(ResumeDetails resumedetail)
        //{
        //    var resume =new Services.Model.ResumeModel();
        //    resume.Resumes.Add(resumedetail);
        //    IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
        //    var filter = Builders<ResumeModel>.Filter.Eq("_id", ObjectId.Parse(resume._id.ToString()));

        //    var result = await collection.ReplaceOneAsync(
        //        filter: new BsonDocument("_id", resume._id),
        //        options: new UpdateOptions
        //        {
        //            IsUpsert = true
        //        },
        //                    replacement: resume);
        //    if (result.ModifiedCount > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public List<ResumeModel> List()
        {
            //get list
            List<ResumeModel> list = new List<ResumeModel>();
            try
            {
                IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
                List<ResumeModel> resumes = new List<ResumeModel>();
                resumes = collection.Find(new BsonDocument()).ToList();
                return resumes;
            }
            catch (System.Exception)
            {

                return list;
            }
        }
        public ResumeModel GetSavedDetails(string username)
        {
            //find first
            IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
            var resumes = new ResumeModel();
            var filter = Builders<ResumeModel>.Filter.Eq("Username", username);
            resumes = collection.Find(filter).FirstOrDefault();
            return resumes;
        }


        public ResumeModel FindResume(Guid resumeId)
        {
            //find first
            IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
            var resumes = new ResumeModel();
            var filter = Builders<ResumeModel>.Filter.Eq("Resumes._ResumeId", resumeId);
            resumes = collection.Find(filter).FirstOrDefault();
            return resumes;
        }

        public List<ResumeDetails> FindProfile(string searchString, string username)
        {
            try
            {
                IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
                List<ResumeModel> resumes = new List<ResumeModel>();
                List<ResumeDetails> result = new List<ResumeDetails>();
                if (string.IsNullOrEmpty(searchString))
                {
                    return result;
                }
                var filter = Builders<ResumeModel>.Filter.Ne("Username", username);
                resumes = collection.Find(filter).ToList();
                resumes = resumes.Where(x => x.FullName != null).ToList();
                var resumesList = resumes.Where(x => x.Username.ToUpper().Contains(searchString.ToUpper()) || x.Email.ToUpper().Contains(searchString.ToUpper()) || x.FullName.ToUpper().Contains(searchString.ToUpper())).ToList();

                if (resumesList.Any() && resumesList != null)
                {
                    foreach (var rdetails in resumesList)
                    {
                        if (rdetails.Resumes != null)
                        {
                            result.AddRange(rdetails.Resumes);
                        }
                    }
                }
                else
                {
                    foreach (var rdetails in resumes)
                    {
                        if (rdetails.Resumes != null)
                        {
                            foreach (var resume in rdetails.Resumes)
                            {
                                if (resume.ResumeTags != null)
                                {
                                    //var item = rdetails.Resumes.Where(x => x.ResumeTags.ToUpper().Contains(searchString.ToUpper()) || x.Phone.Contains(searchString) || x.ResumeTitle.ToUpper().Contains(searchString.ToUpper()) || x.Address.ToUpper().Contains(searchString.ToUpper()) && x.SetActive == "YES");
                                    var r = resume.ResumeTags.ToUpper().Contains(searchString.ToUpper());
                                    if (r)
                                    {
                                        result.Add(resume);
                                    }
                                }

                            }
                        }

                    }
                }

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public ResumeDetails GetPublicResume(string publicname)
        {
            var resumeDetails = new ResumeDetails();

            IMongoCollection<ResumeModel> collection = ConnectionBl.MongoDbConnection();
            var resumes = new ResumeModel();
            var filter = Builders<ResumeModel>.Filter.Eq("PublicName", publicname);
            resumes = collection.Find(filter).FirstOrDefault();
            if (resumes != null)
            {
                var list = resumes.Resumes;
                if (list != null)
                {
                    resumeDetails = resumes.Resumes.Where(x => x.SetActive.ToUpper() == "ON").FirstOrDefault();
                }
            }
            return resumeDetails;

        }

        public HomePageDataModel HomePageInfo()
        {
            HomePageDataModel model = new HomePageDataModel();
            var allusers = List();
            model.UserCount = allusers.Count();
            List<ResumeDetails> allresumes = new List<ResumeDetails>();
            int resumecount = 0;
            foreach (var resume in allusers)
            {
                if (resume.Resumes != null)
                {
                    resumecount += resume.Resumes.Count();
                }
            }
            model.ResumeCount = resumecount;

            if (!allusers.Any())
            {
                model.FirstTopCount = 0;
                model.SecoundTopCount = 0;
                model.ThirdTopCount = 0;
                model.FourthTopCount = 0;
                return model;
            }
            var cat =
            from p in allusers
            group p by p.Category into pgroup
            let count = pgroup.Count()
            orderby count
            select new { Count = count, category = pgroup.Key };

            model.FirstTopCountName = cat.LastOrDefault().category;
            model.FirstTopCount = cat.LastOrDefault().Count;


            allusers.RemoveAll(x => x.Category == model.FirstTopCountName);
            if (!allusers.Any())
            {
                model.SecoundTopCount = 0;
                model.ThirdTopCount = 0;
                model.FourthTopCount = 0;
                return model;
            }

            cat =
            from p in allusers
            group p by p.Category into pgroup
            let count = pgroup.Count()
            orderby count
            select new { Count = count, category = pgroup.Key };


            model.SecoundTopCountName = cat.LastOrDefault().category;
            model.SecoundTopCount = cat.LastOrDefault().Count;


            allusers.RemoveAll(x => x.Category == model.SecoundTopCountName);
            if (!allusers.Any())
            {
                model.ThirdTopCount = 0;
                model.FourthTopCount = 0;
                return model;
            }

            cat =
            from p in allusers
            group p by p.Category into pgroup
            let count = pgroup.Count()
            orderby count
            select new { Count = count, category = pgroup.Key };

            model.ThirdTopCountName = cat.LastOrDefault().category;
            model.ThirdTopCount = cat.LastOrDefault().Count;


            allusers.RemoveAll(x => x.Category == model.ThirdTopCountName);

            if (!allusers.Any())
            {
                model.FourthTopCount = 0;
                return model;
            }

            cat =
           from p in allusers
           group p by p.Category into pgroup
           let count = pgroup.Count()
           orderby count
           select new { Count = count, category = pgroup.Key };

            model.FourthTopCountName = cat.LastOrDefault().category;
            model.FourthTopCount = cat.LastOrDefault().Count;


            return model;
        }
    }

}
