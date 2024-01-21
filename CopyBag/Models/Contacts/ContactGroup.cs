using SQLite;
using SQLiteNetExtensions.Attributes;

namespace CopyBag.Models.Contacts
{
    [Table("ContactGroups")]
    public class ContactGroup : BaseClass
    {
        public string GroupName { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
        public List<Contact> Contacts { get; set; }

        public ContactGroup()
        {
            Contacts = new List<Contact>();
        }

        public ContactGroup(string groupName)
        {
            GroupName = groupName;
            Contacts = new List<Contact>();
        }

        public void Edit(string groupName)
        {
            GroupName = groupName;
        }


    }
}