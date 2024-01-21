namespace CopyBag.Infrastructure.Repositories
{
    public class DatabaseActions
    {
        DbContext _context;
        private IEnumerable<string> TableNames = ["CopyBins", "CopyItems", "Contacts", "ContactGroups"];
        public DatabaseActions(DbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task ResetDatabase()
        {
            await _context.InitAsync();
            foreach (var table in TableNames)
            {
                await _context.Database.ExecuteAsync($"DELETE FROM [{table}]");
            }
        }
    }
}
