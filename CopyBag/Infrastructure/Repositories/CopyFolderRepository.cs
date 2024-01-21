using CopyBag.Models.CopyBin;

namespace CopyBag.Infrastructure.Repositories
{
    internal class CopyFolderRepository : BaseRepository<CopyFolder>, ICopyFolderRepository
    {
        DbContext _context;
        public CopyFolderRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<string> GetContactNameByAsync(int contactId)
        {
            await _context.InitAsync();
            var item = await _context.Database.Table<Models.Contacts.Contact>().FirstOrDefaultAsync(x => x.Id == contactId);
            return item.FullName;
        }

        public async Task<List<CopyFolder>> GetFoldersByContactIdAsync(int contactId)
        {
            await _context.InitAsync();
            return await _context.Database.Table<CopyFolder>()
                .Where(x => x.ContactId == contactId)
                .ToListAsync();
        }
    }
}
