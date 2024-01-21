using SQLite;

namespace CopyBag.Models
{
    public class BaseClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastSeen { get; set; }

        public BaseClass()
        {
            CreationDate = DateTime.Now;
            LastSeen = CreationDate;
        }
    }
}
