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
        private string _accessToken;
        private string _searchResponse;

        [Given(@"I have a valid client credential token")]
        public async Task GivenIHaveAValidClientCredentialsToken()
        {
            _accessToken = await SpotifyAuthManager.GetAccessTokenAsync();
            Assert.IsNotNull(_accessToken);
        }


        // ----------- TRACK SEARCH STEPS -----------


        [When(@"I search for the track ""(.*)""")]
        public async Task WhenISearchForTheTrack(string track) 
        {
            var searchService = new SpotifySearchService(_accessToken);
            _searchResponse = await searchService.SearchTrackAsync(track);
        }

        [Then(@"the response should contain the track ""(.*)""")]
        public async Task ThenTheResponseShouldContainTheTrack(string expectedTrackName)
        {
            var json = JObject.Parse(_searchResponse);
            var actualTrack = json["tracks"]?["items"]?[0]?["name"]?.ToString();

            TestContext.Out.WriteLine($"Expected: {expectedTrackName}, Actual: {actualTrack}");
            Assert.AreEqual(expectedTrackName, actualTrack);
        }


        // ----------- ARTIST SEARCH STEPS -----------


        [When(@"I search for the artist ""(.*)""")]
        public async Task WhenISearchForTheArtist(string artistName)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var encodedName = System.Web.HttpUtility.UrlEncode(artistName);
            var url = $"https://api.spotify.com/v1/search?q={encodedName}&type=artist&limit=1";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            _searchResponse = await response.Content.ReadAsStringAsync();
        }

        [Then(@"the response should contain the artist ""(.*)""")]
        public async Task ThenTheResponseShouldContainTheArtist(string expectedArtistName)
        {
            var json = JObject.Parse(_searchResponse);
            var actualArtist = json["artists"]?["items"]?[0]?["name"]?.ToString();

            TestContext.Out.WriteLine("Raw response: " + _searchResponse);
            TestContext.Out.WriteLine($"Expected: {expectedArtistName}, Actual: {actualArtist}");

            Assert.AreEqual(expectedArtistName, actualArtist);
        }


    }
}