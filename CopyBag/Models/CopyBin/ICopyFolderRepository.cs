namespace CopyBag.Models.CopyBin
{
    public interface ICopyFolderRepository : IBaseRepository<CopyFolder>
    {
        Task<List<CopyFolder>> GetFoldersByContactIdAsync(int contactId);
        Task<string> GetContactNameByAsync(int contactId);
    }
}
