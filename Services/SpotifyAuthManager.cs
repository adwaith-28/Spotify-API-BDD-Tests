using Newtonsoft.Json.Linq;
using SpotifyApi.BDDTests.Helpers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace SpotifyApi.BDDTests.Services
{
    public static class SpotifyAuthManager
    {
        private static string _cachedToken = null;
        private static DateTime _tokenExpiryTime;

        public static async Task<string> GetAccessTokenAsync()
        {
            if(_cachedToken != null && DateTime.UtcNow < _tokenExpiryTime)
            {
                var remainingTime = _tokenExpiryTime - DateTime.UtcNow;
                TestContext.Out.WriteLine($"Using cached access token. Time remaining: {remainingTime.Minutes}m {remainingTime.Seconds}s");
                return _cachedToken;
            }

            var clientId = ConfigHelper.GetSpotifyClientId();
            var clientSecret = ConfigHelper.GetSpotifyClientSecret();

            using var client = new HttpClient();
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            var token = json["access_token"]?.ToString();
            var expiresInSeconds = json["expires_in"]?.ToObject<int>() ?? 3600;

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Spotify token is null. Response: " + content);

            _cachedToken = token;
            _tokenExpiryTime = DateTime.UtcNow.AddSeconds(expiresInSeconds - 60);

            TestContext.Out.WriteLine("New access token retrieved and cached.");
            TestContext.Out.WriteLine($"Expires in approximately: {expiresInSeconds - 60}s (actual Spotify says {expiresInSeconds}s)");
            TestContext.Out.WriteLine($"Will expire at: {_tokenExpiryTime.ToLocalTime()}");
            TestContext.Out.WriteLine("Access Token: " + json["access_token"]?.ToString());
            TestContext.Out.WriteLine("CLIENT ID FROM CONFIG: " + clientId);
            TestContext.Out.WriteLine("RAW TOKEN RESPONSE: " + content);
            return _cachedToken;
        }
    }
}