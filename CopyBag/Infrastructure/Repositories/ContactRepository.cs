using CopyBag.Models.Contacts;

namespace CopyBag.Infrastructure.Repositories
{
    internal class ContactRepository : BaseRepository<Models.Contacts.Contact>, IContactRepository
    {
        DbContext _context;
        public ContactRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Models.Contacts.Contact>> GetContactsByGroupIdAsync(int groupId)
        {
            await _context.InitAsync();

            return await _context.Database.Table<Models.Contacts.Contact>()
                .Where(x => x.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<string> GetGroupNameByAsync(int groupId)
        {
            await _context.InitAsync();
            var item = await _context.Database.Table<ContactGroup>().FirstOrDefaultAsync(x => x.Id == groupId);
            return item.GroupName;
        }
    }
}
