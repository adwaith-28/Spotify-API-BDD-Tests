using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SpotifyApi.BDDTests.Services
{
    public class SpotifySearchService
    {
        private readonly HttpClient _client;

        public SpotifySearchService(string accessToken)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> SearchTrackAsync(string trackName)
        {
            var encodedTrack = System.Web.HttpUtility.UrlEncode(trackName);
            var url = $"https://api.spotify.com/v1/search?q={encodedTrack}&type=track&limit=1";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
