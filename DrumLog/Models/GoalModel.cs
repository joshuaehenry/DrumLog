using DrumLog.Common;
using DrumLog.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrumLog.Models
{
    public class GoalModel : BaseModel
    {
        #region Constants
        #endregion
        #region Properties
        [BsonId]
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        public DateTime? TargetCompletedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        #endregion
        #region Constructors
        public GoalModel() { }
        public GoalModel(ObjectId id)
        {
            BsonDocument goalDoc = DatabaseOperations.QuerySingle(AppConstants.Database.Collections.GOAL, "_id", id.ToString());
            PopulateProperties(goalDoc);
        }
        public GoalModel(ObjectId id, string description, bool isCompleted)
        {
            Id = id;
            Description = description;
            IsCompleted = isCompleted;
        }
        #endregion
        #region Overridden Methods
        public override bool DeleteDocument()
        {
            throw new NotImplementedException();
        }
        public override bool InsertDocument()
        {
            throw new NotImplementedException();
        }
        public override bool Save()
        {
            throw new NotImplementedException();
        }
        public override bool UpdateDocument()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        private void PopulateProperties(BsonDocument doc)
        {
            Id = new ObjectId(doc["_id"].ToString());
            Description = doc["Description"].ToString();
            InsertTimestamp = DateTime.Parse(doc["InsertTimestamp"].ToString());
            UpdateTimestamp = DateTime.Parse(doc["UpdateTimestamp"].ToString());
            IsCompleted = Convert.ToBoolean(doc["SessionDescription"]);
            TargetCompletedDate = DateTime.Parse(doc["TargetEndDate"].ToString());
        }
        #endregion
    }
    public class GoalList : List<GoalModel>
    {
        public GoalList() { }
        public void LoadByListOfIds(List<ObjectId> ids)
        {
            // TODO
        }
        
    }
}
