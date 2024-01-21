namespace CopyBag.Models.CopyBin
{
    public interface ICopyItemRepository : IBaseRepository<CopyItem>
    {
        Task<List<CopyItem>> GetCopyItemsByFolderIdAsync(int folderId);
        Task<string> GetFolderNameByAsync(int folderId);
        Task<string> GetFirstCopyTextByAsync(int id);
        Task<string> GetSecondCopyTextByAsync(int id);
    }
}
