using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace SpotifyApi.BDDTests.Helpers
{
    public static class SpotifyAuthHelper
    {
        // Launches the browser with the Spotify login URL to initiate the Authorization Code Flow.
        public static void OpenSpotifyAuthPage(string clientId, string redirectUri, string scopes)
        {
            var state = Guid.NewGuid().ToString("N");

            var url = $"https://accounts.spotify.com/authorize/" +
                        $"?response_type=code" +
                        $"&client_id={Uri.EscapeDataString(clientId)}" +
                        $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                        $"&scope={Uri.EscapeDataString(scopes)}" +
                        $"&state={state}";

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });

            Console.WriteLine("Opened Spotify login page in default browser");
        }
            // Exchanges the authorization code for access and refresh tokens

            public static async Task<(string accessToken, string refreshToken)> ExchangeCodeForToken(string code, string clientId, string clientSecret, string redirectUri)
            {
                using var client = new HttpClient();

                var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token/");
                request.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", redirectUri)
                });

                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to exchange code. Status: {response.StatusCode}, Content: {content}");


                var json = JObject.Parse(content);
                var accessToken = json["access_token"]?.ToString();
                var refreshToken = json["refresh_token"]?.ToString();

                if (string.IsNullOrEmpty(accessToken))
                    throw new Exception("Access token is missing from response.");

                Console.WriteLine("Access Token Retrieved");
                Console.WriteLine(accessToken);

                return (accessToken, refreshToken);
            }

        }
}