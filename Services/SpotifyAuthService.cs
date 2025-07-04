using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SpotifyApi.BDDTests.Helpers;
using NUnit.Framework;

namespace SpotifyApi.BDDTests.Services
{
    public class SpotifyAuthService
    {
        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = ConfigHelper.GetSpotifyClientId();
            var clientSecret = ConfigHelper.GetSpotifyClientSecret();
            using var client = new HttpClient();

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            TestContext.Out.WriteLine("Access Token: " + json["access_token"]?.ToString());
            TestContext.Out.WriteLine("CLIENT ID FROM CONFIG: " + clientId);
            TestContext.Out.WriteLine("RAW TOKEN RESPONSE: " + content);

            return json["access_token"]?.ToString();
        }
    }
}