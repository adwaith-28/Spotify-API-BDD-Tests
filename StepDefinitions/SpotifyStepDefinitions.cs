using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SpotifyApi.BDDTests.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpotifyApi.BDDTests.StepDefinitions
{
    [Binding]
    public class SpotifyStepDefinitions
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly SpotifyAuthService _authService = new SpotifyAuthService();
        private string _accessToken;
        private HttpResponseMessage _response;

        [Given(@"I have a valid client credentials token")]
        public async Task GivenIHaveAValidClientCredentialsToken()
        {
            _accessToken = await _authService.GetAccessTokenAsync();
        }

        [When(@"I search for track ""(.*)""")]
        public async Task WhenISearchForTrack(string track) { 

            var url = $"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(track)}&type=track&limit=1";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            _response = await _client.GetAsync(url);
        }

        [Then(@"the response should contain the track ""(.*)""")]
        public async Task ThenTheResponseShouldContainTheTrack(string expectedTrackName)
        {
            var content = await _response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var actualTrackName = json["tracks"]?["items"]?[0]?["name"]?.ToString();
            Assert.AreEqual(expectedTrackName, actualTrackName);
        }
    }
}