using DrumLog.Common;
using DrumLog.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using static DrumLog.Database.Database;

namespace DrumLog.Models
{
    public class PracticeLogModel : BaseModel
    {
        #region Properties
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        public string SessionDescription { get; set; }
        public int StartingTempo { get; set; } 
        public int EndingTempo { get; set; }
        public int DurationInMinutes { get; set; }
        public string AdditionalNotes { get; set; }
        #endregion
        #region Constructors
        public PracticeLogModel()
        {
            
        }
        public PracticeLogModel(string userId, DateTime insertTimestamp, DateTime updateTimestamp, string sessionDescription, int startingTempo, int endingTempo, int durationInMinutes, string additionalNotes)
        {
            UserId = userId;
            InsertTimestamp = insertTimestamp;
            UpdateTimestamp = updateTimestamp;
            SessionDescription = sessionDescription;
            StartingTempo = startingTempo;
            EndingTempo = endingTempo;
            DurationInMinutes = durationInMinutes;
            AdditionalNotes = additionalNotes;
        }
        public PracticeLogModel(ObjectId id)
        {
            BsonDocument doc = DatabaseOperations.QuerySingle(AppConstants.Database.Collections.PRACTICE_LOG, "_id", id.ToString());
            PopulateProperties(doc);
        }
        #endregion
        #region Overridden Methods
        public override bool Save()
        {
            if (IsNew && string.IsNullOrEmpty(UserId))
                return InsertDocument();
            return UpdateDocument();
        }
        public override bool DeleteDocument()
        {
            return DatabaseOperations.DeleteOne(AppConstants.Database.Collections.PRACTICE_LOG, Id);
        }
        public override bool InsertDocument()
        {
            if (Id != BsonObjectId.Empty)
            {
                throw new Exception("PracticeLogModel object must not have an _id when inserting.");
            }

            Id = ObjectId.GenerateNewId();

            BsonDocument bsonDoc = new BsonDocument { { "_id", Id }, { "UserId", UserId },
                    { "InsertTimestamp", InsertTimestamp }, { "UpdateTimestamp", UpdateTimestamp },
                    { "SessionDescription", SessionDescription }, { "StartingTempo", StartingTempo },
                    { "EndingTempo", EndingTempo }, { "DurationInMinutes", DurationInMinutes },
                    { "AdditionalNotes", AdditionalNotes }
                };

            PracticeLogCollection.InsertOne(bsonDoc);

            return true;
        }
        public override bool UpdateDocument()
        {

            BsonDocument bsonDoc = new BsonDocument { { "UserId", UserId },
                { "InsertTimestamp", InsertTimestamp }, { "UpdateTimestamp", DateTime.Now },
                { "SessionDescription", SessionDescription }, { "StartingTempo", StartingTempo },
                { "EndingTempo", EndingTempo }, { "DurationInMinutes", DurationInMinutes },
                { "AdditionalNotes", AdditionalNotes }
            };

            return Database.DatabaseOperations.UpdateOne(AppConstants.Database.Collections.PRACTICE_LOG, bsonDoc, Id);
        }
        #endregion
        #region Public Methods
        public void LoadById(string id)
        {
            var practiceLogs = DatabaseOperations.Query(AppConstants.Database.Collections.PRACTICE_LOG,
                "_id", id, Sort.DESC);

            PopulateProperties(practiceLogs[0]);
        }
        #endregion
        #region Private Methods
        private void PopulateProperties(BsonDocument doc)
        {
            Id = new ObjectId(doc["_id"].ToString());
            UserId = doc["UserId"].ToString();
            InsertTimestamp = DateTime.Parse(doc["InsertTimestamp"].ToString());
            UpdateTimestamp = DateTime.Parse(doc["UpdateTimestamp"].ToString());
            SessionDescription = doc["SessionDescription"].ToString();
            StartingTempo = Convert.ToInt32(doc["StartingTempo"]);
            EndingTempo = Convert.ToInt32(doc["EndingTempo"]);
            DurationInMinutes = Convert.ToInt32(doc["DurationInMinutes"]);
            AdditionalNotes = doc["AdditionalNotes"].ToString();
        }
        #endregion
    }

    public class PracticeLogModelList : List<PracticeLogModel>
    {
        public PracticeLogModelList() { }
        public void LoadByUserId(string userId)
        {
            var practiceLogs = DatabaseOperations.Query(AppConstants.Database.Collections.PRACTICE_LOG,
                "UserId", userId,
                Database.Database.Sort.DESC);
            Populate(practiceLogs);
        }
        private void Populate(List<BsonDocument> practiceLogs)
        {
            foreach (BsonDocument doc in practiceLogs)
            {
                this.Add(new PracticeLogModel
                {
                    Id = new ObjectId(doc["_id"].ToString()),
                    UserId = doc["UserId"].ToString(),
                    InsertTimestamp = DateTime.Parse(doc["InsertTimestamp"].ToString()),
                    UpdateTimestamp = DateTime.Parse(doc["UpdateTimestamp"].ToString()),
                    SessionDescription = doc["SessionDescription"].ToString(),
                    StartingTempo = Convert.ToInt32(doc["StartingTempo"]),
                    EndingTempo = Convert.ToInt32(doc["EndingTempo"]),
                    DurationInMinutes = Convert.ToInt32(doc["DurationInMinutes"]),
                    AdditionalNotes = doc["AdditionalNotes"].ToString()
                });
            }
        }
    }
}
