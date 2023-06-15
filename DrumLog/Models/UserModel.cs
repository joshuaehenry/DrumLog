using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static DrumLog.Database.Database;

namespace DrumLog.Models
{
    public class UserModel : BaseModel
    {
        #region Constants
        #endregion
        #region Properties
        [BsonId]
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<GoalModel> Goals { get; set; }
        public DateTime InsertTimestamp { get; set; }
        public DateTime UpdateTimestamp { get; set; }
        #endregion
        #region Constructors
        public UserModel() { }
        public UserModel(ObjectId id, string email, string firstName, string lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        #endregion
        #region Overridden Methods
        public override bool DeleteDocument()
        {
            throw new NotImplementedException();
        }

        public override bool InsertDocument()
        {
            try
            {
                if (Id != BsonObjectId.Empty)
                {
                    throw new Exception("UserModel object must not have an _id when inserting.");
                }

                Id = ObjectId.GenerateNewId();

                BsonDocument bsonDoc = new BsonDocument { { "_id", Id },
                    { "InsertTimestamp", InsertTimestamp }, { "UpdateTimestamp", UpdateTimestamp },
                    { "FirstName", FirstName }, { "LastName", LastName },
                    { "Email", Email }
                };
                
                BsonArray goalIdElements = new BsonArray();

                foreach (GoalModel goal in Goals)
                {
                    goalIdElements.Add(new BsonDocument("GoalId", goal.Id));
                }

                bsonDoc.Add("GoalIds", goalIdElements);

                UserCollection.InsertOne(bsonDoc);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("{0}{1}", e.Message, e.StackTrace));
            }
        }

        public override bool Save()
        {
            if (IsNew)
                return InsertDocument();
            return UpdateDocument();
        }

        public override bool UpdateDocument()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Public Methods
        public void AddGoal(ObjectId goalId)
        {
            GoalIds.Add(goalId);
        }
        public void AddGoal(GoalModel goal)
        {
            GoalIds.Add(goal.Id);
        }
        public void GetGoals()
        {

        }
        #endregion
        #region Private Methods
        #endregion
    }
}
