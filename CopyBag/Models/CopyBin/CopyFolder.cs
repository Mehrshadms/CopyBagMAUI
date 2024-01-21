using SQLite;
using SQLiteNetExtensions.Attributes;


namespace CopyBag.Models.CopyBin
{
    [Table("CopyBins")]
    public class CopyFolder : BaseClass
    {
        public string FolderName { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<CopyItem> CopyItems { get; set; }

        [ForeignKey(typeof(Contacts.Contact))]
        public int ContactId { get; set; }

        public CopyFolder()
        {
            CopyItems = new List<CopyItem>();
        }

        public CopyFolder(string folderName, int contactId)
        {
            FolderName = folderName;
            ContactId = contactId;
            CopyItems = new List<CopyItem>();
        }

        public void Edit(string folderName, int contactId)
        {
            FolderName = folderName;
            ContactId = contactId;
        }
    }
}
