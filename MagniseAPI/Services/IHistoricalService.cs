using MagniseAPI.Entities;
using MagniseAPI.Models;

namespace MagniseAPI.Services
{
    public interface IHistoricalService
    {
        Task<IEnumerable<PriceInfo>> GetHistoricalPricesAsync(Guid assetId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<AssetForCreationDto>> GetHistoricalAssetsAsync();
        Task<PriceInfo> GetHistoricalLastPricesAsync(Guid assetId, string provider);
    }
}
