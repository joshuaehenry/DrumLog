namespace DrumLog.Models
{
    public abstract class BaseModel
    {
        public abstract bool InsertDocument();
        public abstract bool UpdateDocument();
        public abstract bool DeleteDocument();
        public abstract bool Save();
        public bool IsNew { get; set; }
    }
}
