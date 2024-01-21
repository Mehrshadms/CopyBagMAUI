namespace CopyBag.Models.Contacts
{
    public interface IContactRepository : IBaseRepository<CopyBag.Models.Contacts.Contact>
    {
        Task<List<CopyBag.Models.Contacts.Contact>> GetContactsByGroupIdAsync(int groupId);
        Task<string> GetGroupNameByAsync(int groupId);
    }
}
