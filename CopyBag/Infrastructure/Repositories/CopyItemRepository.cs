using CopyBag.Models.CopyBin;

namespace CopyBag.Infrastructure.Repositories
{
    public class CopyItemRepository : BaseRepository<CopyItem>, ICopyItemRepository
    {
        DbContext _context;
        public CopyItemRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<CopyItem>> GetCopyItemsByFolderIdAsync(int folderId)
        {
            await _context.InitAsync();
            return await _context.Database.Table<CopyItem>()
                .Where(x => x.FolderId == folderId)
                .ToListAsync();
        }

        public async Task<string> GetFirstCopyTextByAsync(int id)
        {
            await _context.InitAsync();
            var item = await _context.Database.Table<CopyItem>().FirstOrDefaultAsync(x => x.Id == id);
            return item.CopyTextOne;
        }

        public async Task<string> GetSecondCopyTextByAsync(int id)
        {
            await _context.InitAsync();
            var item = await _context.Database.Table<CopyItem>().FirstOrDefaultAsync(x => x.Id == id);
            return item.CopyTextTwo;
        }

        public async Task<string> GetFolderNameByAsync(int folderId)
        {
            await _context.InitAsync();
            var item = await _context.Database.Table<CopyFolder>().FirstOrDefaultAsync(x => x.Id == folderId);
            return item.FolderName;
        }
    }
}
