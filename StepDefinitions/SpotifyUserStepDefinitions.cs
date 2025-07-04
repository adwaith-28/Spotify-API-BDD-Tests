using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SpotifyApi.BDDTests.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using SpotifyApi.BDDTests.Helpers;

namespace SpotifyApi.BDDTests.StepDefinitions
{
    [Binding]
    public class SpotifyUserStepDefinitions
    {
        private string _accessToken;
        private string _meResponse;

        [Given(@"I have logged in with Spotify")]
        public async Task GivenIHaveLoggedInWithSpotify()
        {
            var clientId = ConfigHelper.GetSpotifyClientId();
            var clientSecret = ConfigHelper.GetSpotifyClientSecret();
            var redirectUri = "http://127.0.0.1:8888/callback";
            var scopes = "user-read-private user-read-email";

            var server = new SpotifyOAuthServer(redirectUri);
            var serverTask = server.StartAsync();

            SpotifyAuthHelper.OpenSpotifyAuthPage(clientId, redirectUri, scopes);

            await serverTask;


            var code = server.AuthCode;
            Assert.IsNotNull(code);

            var (accessToken, _) = await SpotifyAuthHelper.ExchangeCodeForToken(code, clientId, clientSecret, redirectUri);
            Assert.IsNotNull(accessToken);

            _accessToken = accessToken;
        }


        [When(@"I request my profile")]
        public async Task WhenIRequestMyProfile()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me/");
            response.EnsureSuccessStatusCode();

            _meResponse = await response.Content.ReadAsStringAsync();
        }

        [Then(@"the response should contain my Spotify username")]
        public void ThenTheResponseShouldContainMySpotifyUsername()
        {
            var json = JObject.Parse(_meResponse);
            var displayName = json["display_name"]?.ToString();

            TestContext.Out.WriteLine("Display Name: " + displayName);
            Assert.IsFalse(string.IsNullOrEmpty(displayName), "Display name is missing in response.");
        }
    }
}
