using MagniseAPI.Entities;
using AutoMapper;

namespace MagniseAPI.Services
{

    public class DataProviderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public DataProviderBackgroundService(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper ??
              throw new ArgumentNullException(nameof(mapper));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await UpdateAssetsUseHistoryData();
            await UpdatePriceUseHistoryData();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var webSocketService = scope.ServiceProvider.GetRequiredService<IWebSocketService>();
                await webSocketService.StartAsync(stoppingToken);
            }
        }

        /// <summary>
        /// Updates the assets in the database using historical data if the database is empty.
        /// </summary>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdateAssetsUseHistoryData()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var assetsRepository = scope.ServiceProvider.GetRequiredService<IAssetsRepository>();

                if (assetsRepository.GetAssets().Any())
                {
                    return;
                }

                var historicalService = scope.ServiceProvider.GetRequiredService<IHistoricalService>();
                var assetsHistorical = await historicalService.GetHistoricalAssetsAsync();

                foreach (var asset in assetsHistorical)
                {
                    assetsRepository.AddAsset(_mapper.Map<Asset>(asset));
                    await assetsRepository.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Updates the prices in the database using historical price data if prices for assets are not yet recorded.
        /// </summary>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task UpdatePriceUseHistoryData()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var historicalService = scope.ServiceProvider.GetRequiredService<IHistoricalService>();
                var assetsHistorical = await historicalService.GetHistoricalAssetsAsync();
                var assetsRepository = scope.ServiceProvider.GetRequiredService<IAssetsRepository>();
                var pricesRepository = scope.ServiceProvider.GetRequiredService<IPricesRepository>();

                foreach (var asset in assetsHistorical)
                {
                    var lastPrice = await historicalService.GetHistoricalLastPricesAsync(asset.Id, "oanda");

                    if (lastPrice!=null && !pricesRepository.GetPrices(asset.Id).Any())
                    {
                        pricesRepository.AddPrice(asset.Id, lastPrice);
                        await pricesRepository.SaveAsync();
                    }
                }
            }
        }
    }
}


