using System.Text.Json.Serialization;
using System.Text.Json;

namespace MagniseAPI.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private DateTime _tokenExpirationTime;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves an access token asynchronously using OAuth password grant flow.
        /// </summary>
        /// <param name="realm">The realm associated with the token.</param>
        /// <param name="clientId">The client ID used for authentication.</param>
        /// <param name="username">The username used for authentication.</param>
        /// <param name="password">The password associated with the username.</param>
        /// <returns>The access token string.</returns>
        /// <exception cref="HttpRequestException">Thrown if the HTTP request fails.</exception>
        public async Task<string> GetTokenAsync(string realm, string clientId, string username, string password)
        {
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpirationTime > DateTime.UtcNow)
            {
                return _accessToken;
            }

            var tokenEndpoint = $"https://platform.fintacharts.com/identity/realms/{realm}/protocol/openid-connect/token";
            var requestContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("username", username),
            new KeyValuePair<string, string>("password", password)
            });

            var response = await _httpClient.PostAsync(tokenEndpoint, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

            _accessToken = tokenResponse.AccessToken;
            _tokenExpirationTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            return _accessToken;
        }

        public class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("refresh_expires_in")]
            public int RefreshExpiresIn { get; set; }

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }

            [JsonPropertyName("scope")]
            public string Scope { get; set; }
        }
    }
}
