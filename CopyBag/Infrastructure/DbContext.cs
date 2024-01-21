using CopyBag.Models.Contacts;
using SQLite;

namespace CopyBag.Infrastructure
{
    public class DbContext
    {
        private readonly string _dbPath;
        public SQLiteAsyncConnection Database;
        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

        public DbContext(string dbPath)
        {
            _dbPath = dbPath;
            InitAsync();
        }

        public async Task InitAsync()
        {
            if (Database is not null)
            {
                return;
            }
            Database = new SQLiteAsyncConnection(_dbPath, Flags);
            var resultfk = Database.ExecuteAsync("PRAGMA foreign_keys = ON;");
            var result = await Database.CreateTableAsync<Models.CopyBin.CopyItem>();
            var results = await Database.CreateTableAsync<Models.CopyBin.CopyFolder>();
            var resultb = await Database.CreateTableAsync<Models.Contacts.Contact>();
            var resultt = await Database.CreateTableAsync<ContactGroup>();
        }

    }
}
