using System.Net.WebSockets;

namespace MagniseAPI.Services
{
    public interface IWebSocketService
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}
