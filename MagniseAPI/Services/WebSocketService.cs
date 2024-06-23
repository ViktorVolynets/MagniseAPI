using MagniseAPI.Helpers;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace MagniseAPI.Services
{
    public class WebSocketService: IWebSocketService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IPricesRepository _pricesRepository;
        private readonly FintatechApiOptions _apiOptions;
        private readonly string _socketBaseUrl;

        public WebSocketService(IServiceProvider serviceProvider, IAssetsRepository assetsRepository, IPricesRepository pricesRepository, IOptions<FintatechApiOptions> apiOptions)
        {
            _serviceProvider = serviceProvider;
            _assetsRepository = assetsRepository;
            _pricesRepository = pricesRepository;
            _apiOptions = apiOptions.Value;
            _socketBaseUrl = @"wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token=";
        }

        /// <summary>
        /// Starts the background service to subscribe to real-time price updates via WebSocket for specified instruments.
        /// </summary>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StartAsync(CancellationToken stoppingToken)
        {
            var instrumentIds = _assetsRepository.GetAssets().Select(x => x.Id);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tokenService = scope.ServiceProvider.GetRequiredService<TokenService>();
                    var token = await tokenService.GetTokenAsync(_apiOptions.Realm, _apiOptions.ClientId, _apiOptions.Username, _apiOptions.Password);

                    using (var webSocket = new ClientWebSocket())
                    {
                        await webSocket.ConnectAsync(new Uri(string.Concat(_socketBaseUrl,token)), stoppingToken);

                        foreach (var instrumentId in instrumentIds)
                        {
                            var subscriptionMessage = new
                            {
                                type = "l1-subscription",
                                id = Guid.NewGuid().ToString(),
                                instrumentId,
                                provider = "simulation",
                                subscribe = true,
                                kinds = new[] { "last" }
                            };

                            var message = JsonSerializer.Serialize(subscriptionMessage);
                            var bytes = Encoding.UTF8.GetBytes(message);
                            await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, stoppingToken);
                        }

                        await ReceiveMessages(webSocket, stoppingToken);
                    }
                }
            }
        }

        /// <summary>
        /// Receives messages from the WebSocket and processes real-time price updates.
        /// </summary>
        /// <param name="webSocket">The WebSocket instance.</param>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ReceiveMessages(ClientWebSocket webSocket, CancellationToken stoppingToken)
        {
            var buffer = new byte[1024 * 4];
            while (!stoppingToken.IsCancellationRequested && webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), stoppingToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, stoppingToken);
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    ProcessMessage(message);
                }
            }
        }

        /// <summary>
        /// Processes a received WebSocket message containing real-time price updates.
        /// </summary>
        /// <param name="message">The JSON message received from the WebSocket.</param>
        /// <exception cref="NotSupportedException">Thrown if the message type is not supported or parsing fails.</exception>
        private void ProcessMessage(string message)
        {
            try
            {
                using var jsonDocument = JsonDocument.Parse(message);
                var root = jsonDocument.RootElement;

                if (root.TryGetProperty("type", out JsonElement typeElement) && typeElement.GetString() == "l1-update")
                {
                    var instrumentId = root.GetProperty("instrumentId").GetGuid();
                    var lastObject = root.GetProperty("last");

                    var price = lastObject.GetProperty("price").GetDecimal();
                    var timestamp = lastObject.GetProperty("timestamp").GetDateTime();

                    _pricesRepository.AddPrice(instrumentId, new Entities.PriceInfo(Guid.NewGuid(), instrumentId, price, timestamp));
                    _pricesRepository.SaveAsync();
                }
            }
            catch (JsonException ex)
            {
                throw new NotSupportedException($"Error parsing JSON: {ex.Message}");
            }
        }
    }
}
