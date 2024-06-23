namespace MagniseAPI.Services
{
    public interface IAssetsRepository
    {
        IEnumerable<Entities.Asset> GetAssets();
        Task<IEnumerable<Entities.Asset>> GetAssetsAsync();
        void AddAsset(Entities.Asset assetToAdd);
        Task<bool> SaveChangesAsync();
    }
}
