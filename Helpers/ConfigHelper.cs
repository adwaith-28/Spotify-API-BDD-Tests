using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace SpotifyApi.BDDTests.Helpers
{
    public class ConfigHelper
    {
        private static IConfigurationRoot config;

        static ConfigHelper()
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
        }

        public static string GetSpotifyClientId() => config["Spotify:ClientId"];

        public static string GetSpotifyClientSecret() => config["Spotify:ClientSecret"];
    }
}