namespace DrumLog.Common
{
    public static class AppSettings
    {
        public struct Database
        {
            public const string MONGO_CONNECTION_STRING = "mongodb+srv://drumlog_app:TTCX8rmPRSIYYZxs@cluster0.la8po.mongodb.net/?retryWrites=true&w=majority";
            public const string DATABASE_USER = "drumlog_app";
            public const string DATABASE_PASSWORD = "TTCX8rmPRSIYYZxs";
            public const string DATABASE_NAME = "drumlog";
        }
        
    }
}
