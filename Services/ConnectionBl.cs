using MongoDB.Driver;
using Services.Model;



namespace Services
{
    public static class ConnectionBl
    {
        public static IMongoCollection<ResumeModel> MongoDbConnection()
        {
            MongoClient client = new MongoClient();
            var db = client.GetDatabase("ResumePORTAL");
            var collection = db.GetCollection<ResumeModel>("ResumeAccounts");
            return collection;
        }

    }
}
                                  