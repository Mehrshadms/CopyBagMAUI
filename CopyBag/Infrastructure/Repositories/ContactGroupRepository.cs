using CopyBag.Models.Contacts;

namespace CopyBag.Infrastructure.Repositories
{
    internal class ContactGroupRepository : BaseRepository<ContactGroup>, IContactGroupRepository
    {
        DbContext _context;
        public ContactGroupRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

    }
}
