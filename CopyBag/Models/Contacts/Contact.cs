using CopyBag.Models.CopyBin;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace CopyBag.Models.Contacts
{
    [Table("Contacts")]
    public class Contact : BaseClass
    {
        //public DateTime CreationDate { get; private set; }
        [MaxLength(250), Unique]
        public string FullName { get; set; }

        [ForeignKey(typeof(ContactGroup))]
        public int GroupId { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.CascadeDelete)]
        public ContactGroup ContactGroup { get; private set; }

        [OneToMany]
        public List<CopyItem> CopyItems { get; set; }

        public Contact()
        {
            CopyItems = new List<CopyItem>();
        }

        public Contact(string fullName, int groupId)
        {
            FullName = fullName;
            GroupId = groupId;
            CopyItems = new List<CopyItem>();
        }

        public void Edit(string fullName, int groupId)
        {
            FullName = fullName;
            GroupId = groupId;
        }

    }
}
