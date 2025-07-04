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
    }
}