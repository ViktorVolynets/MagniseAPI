using MagniseAPI.Entities;
using MagniseAPI.Helpers;
using MagniseAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MagniseAPI.Services
{
    public class HistoricalService : IHistoricalService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;
        private readonly ILogger<HistoricalService> _logger;
        private readonly FintatechApiOptions _apiOptions;
        private readonly string _serviceUrlBase = "https://platform.fintacharts.com";
      public HistoricalService(HttpClient httpClient, TokenService tokenService, ILogger<HistoricalService> logger, IOptions<FintatechApiOptions> apiOptions)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
            _logger = logger;
            _apiOptions = apiOptions.Value;
        }

        /// <summary>
        /// Authenticates the HTTP client with the API using the configured credentials.
        /// </summary>
        /// <returns>Task representing the asynchronous operation.</returns>
        private async Task AuthenticateRequestAsync()
        {
            var token = await _tokenService.GetTokenAsync(_apiOptions.Realm, _apiOptions.ClientId, _apiOptions.Username, _apiOptions.Password);
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Retrieves historical assets data asynchronously from the external API.
        /// </summary>
        /// <returns>A collection of historical assets data.</returns>
        public async Task<IEnumerable<AssetForCreationDto>> GetHistoricalAssetsAsync()
        {
            await AuthenticateRequestAsync();

            try
            {
                var response = await _httpClient.GetAsync($"{_serviceUrlBase}/api/instruments/v1/instruments");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Received JSON: {jsonString}");


                    var assetsResponse = JsonSerializer.Deserialize<HistoricalAssetsResponse>(jsonString);
                    return assetsResponse?.Data ?? Enumerable.Empty<AssetForCreationDto>();
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching historical assets: {ex.Message}");
            }

            return Enumerable.Empty<AssetForCreationDto>();
        }

        /// <summary>
        /// Retrieves historical prices data asynchronously from the external API within a specified date range.
        /// </summary>
        /// <param name="assetId">The ID of the asset for which to retrieve historical prices.</param>
        /// <param name="startDate">The start date of the historical price range.</param>
        /// <param name="endDate">The end date of the historical price range.</param>
        /// <returns>A collection of historical price information.</returns>
        public async Task<IEnumerable<PriceInfo>> GetHistoricalPricesAsync(Guid assetId, DateTime startDate, DateTime endDate)
        {
            await AuthenticateRequestAsync();

            try
            {
                var response = await _httpClient.GetAsync($"{_serviceUrlBase}/api/historical/{assetId}?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}");

                if (response.IsSuccessStatusCode)
                {
                    var prices = await response.Content.ReadFromJsonAsync<IEnumerable<PriceInfo>>();
                    return prices ?? Enumerable.Empty<PriceInfo>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching historical prices: {ex.Message}");
            }

            return Enumerable.Empty<PriceInfo>();
        }

        /// <summary>
        /// Retrieves the last historical price data asynchronously for a specific asset and provider.
        /// </summary>
        /// <param name="assetId">The ID of the asset for which to retrieve the last price.</param>
        /// <param name="provider">The name of the provider for which to retrieve the last price.</param>
        /// <returns>The last historical price information for the asset and provider.</returns>
        public async Task<PriceInfo> GetHistoricalLastPricesAsync(Guid assetId, string provider)
        {
            await AuthenticateRequestAsync();

            int interval = 1;
            string periodicity = "minute";
            int barsCount = 1;

            var url = $"{_serviceUrlBase}/api/bars/v1/bars/count-back?instrumentId={assetId}&provider={provider}&interval={interval}&periodicity={periodicity}&barsCount={barsCount}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // Parse the JSON directly
                    var data = JsonSerializer.Deserialize<JsonElement>(json);

                    if (data.TryGetProperty("data", out var dataArray) && dataArray.GetArrayLength() > 0)
                    {
                        var firstItem = dataArray[0];

                        // Map the JSON fields to your PriceInfo object
                        var priceInfo = new PriceInfo(Guid.NewGuid(), 
                            assetId, 
                            firstItem.GetProperty("c").GetDecimal(),
                            DateTime.Parse(firstItem.GetProperty("t").GetString()));

                        return priceInfo;
                    }
                }
                else
                {
                    _logger.LogError($"HTTP request failed with status code {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching last price: {ex.Message}");
            }

            return null;
        }
    }     
}
