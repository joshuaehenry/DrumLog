using DrumLog.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DrumLog.Database
{
    public static class Database
    {
        #region Constants
        public struct Sort
        {
            public const string ASC = "1";
            public const string DESC = "-1";
        }
        #endregion
        #region Properties
        public static IMongoClient DrumLogClient
        {
            get
            {
                return new MongoClient(AppSettings.Database.MONGO_CONNECTION_STRING);
            }
        }
        public static IMongoDatabase DrumLogDatabase
        {
            get
            {
                return DrumLogClient.GetDatabase(AppSettings.Database.DATABASE_NAME);
            }
        }
        public static IMongoCollection<BsonDocument> PracticeLogCollection
        {
            get
            {
                return DrumLogDatabase.GetCollection<BsonDocument>(AppConstants.Database.Collections.PRACTICE_LOG);
            }
        }
        public static IMongoCollection<BsonDocument> UserCollection
        {
            get
            {
                return DrumLogDatabase.GetCollection<BsonDocument>(AppConstants.Database.Collections.USER);
            }
        }
        public static IMongoCollection<BsonDocument> GoalCollection
        {
            get
            {
                return DrumLogDatabase.GetCollection<BsonDocument>(AppConstants.Database.Collections.GOAL);
            }
        }
        #endregion
        #region Public Methods
        public static IMongoCollection<BsonDocument> GetCollection(string collection)
        {
            switch (collection)
            {
                case AppConstants.Database.Collections.PRACTICE_LOG:
                    return Database.PracticeLogCollection;
                case AppConstants.Database.Collections.USER:
                    return Database.UserCollection;
                case AppConstants.Database.Collections.GOAL:
                    return Database.GoalCollection;
                default:
                    throw new Exception("");
            }
        }
        #endregion
    }

    public static class DatabaseOperations
    {
        #region Properties
        public static bool OperationException = false;
        public static string OperationExceptionMessage = string.Empty;
        #endregion
        #region Public Methods
        public static BsonDocument QuerySingle(string collection, string filterFieldName, string filterFieldValue)
        {
            OperationException = false;
            OperationExceptionMessage = string.Empty;
            IMongoCollection<BsonDocument> oCollection = Database.GetCollection(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(filterFieldName, filterFieldValue);
            BsonDocument result = null;

            try
            {
                result = oCollection
                        .Find(filter)
                        .Sort(string.Format("{UpdateTimestamp: {0}}", Database.Sort.DESC))
                        .ToList()[0];
            }
            catch (Exception e)
            {
                OperationException = true;
                OperationExceptionMessage = string.Format("Error during QuerySingle (filterField={0}, filterFieldValue={1}, Collection={2}) - {3}.", filterFieldName, filterFieldValue, collection, e.Message);
            }

            return result;
        }
        public static List<BsonDocument> Query(string collection, 
            string filterFieldName, string filterFieldValue,
            string sortBy = "")
        {
            OperationException = false;
            OperationExceptionMessage = string.Empty;
            IMongoCollection<BsonDocument> oCollection = Database.GetCollection(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(filterFieldName, filterFieldValue);
            List<BsonDocument> result = null;

            try
            {
                if (sortBy == Database.Sort.ASC)
                    result = oCollection
                        .Find(filter)
                        .Sort(string.Format("{UpdateTimestamp: {0}}", Database.Sort.ASC))
                        .ToList();
                else
                    result = oCollection
                        .Find(filter)
                        .Sort(string.Format("{UpdateTimestamp: {0}}", Database.Sort.DESC))
                        .ToList();
            } catch (Exception e)
            {
                OperationException = true;
                OperationExceptionMessage = string.Format("Error during Query (filterField={0}, filterFieldValue={1}, Collection={2}) - {3}.", filterFieldName, filterFieldValue, collection, e.Message);
            }
            
            return result;
        }

        public static bool UpdateOne(string collection, BsonDocument updateDoc, ObjectId id)
        {
            OperationException = false;
            OperationExceptionMessage = string.Empty;
            IMongoCollection<BsonDocument> oCollection = Database.GetCollection(collection);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            long rowsUpdated = 0;

            try
            {
                rowsUpdated = oCollection.UpdateOne(filter, updateDoc).ModifiedCount;
            } catch (Exception e)
            {
                OperationException = true;
                OperationExceptionMessage = string.Format("Error during UpdateOne (ObjectId={0}, Collection={1}) - {2}.", id.ToString(), collection, e.Message);
                OperationExceptionMessage = string.Format("Error during UpdateOne (ObjectId={0}, Collection={1}) - {2}.", id.ToString(), collection, e.Message);
            }

            return rowsUpdated == 1;
        }

        public static bool DeleteOne(string collection, ObjectId id)
        {
            OperationException = false;
            OperationExceptionMessage = string.Empty;
            IMongoCollection<BsonDocument> oCollection = Database.GetCollection(collection);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            long rowsDeleted = 0;

            try
            {
                rowsDeleted = oCollection.DeleteOne(filter).DeletedCount;
            } catch (Exception e)
            {
                OperationException = true;
                OperationExceptionMessage = string.Format("Error during DeleteOne (ObjectId={0}, Collection={1}) - {2}.", id.ToString(), collection, e.Message);
                OperationExceptionMessage = string.Format("Error during UpdateOne (ObjectId={0}, Collection={1}) - {2}.", id.ToString(), collection, e.Message);
            }

            return rowsDeleted == 1;
        }
        #endregion
    }
}
