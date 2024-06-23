using MagniseAPI.Entities;
using System.Reflection;

namespace MagniseAPI.Services
{
    public interface IPricesRepository
    {
        IEnumerable<Entities.PriceInfo> GetPrices(Guid assetId);
        Task<IEnumerable<PriceInfo>> GetPricesAsync(Guid assetId);
        Task<IEnumerable<Entities.PriceInfo>> GetPricesAsync(IEnumerable<Guid> assetIds);
        void AddPrice(Guid assetId, PriceInfo price);
        Task<bool> PriceExistsAsync(Guid assetId);
        Task<bool> SaveAsync();
    }
}
