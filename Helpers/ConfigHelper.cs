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
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
        }

        public static string GetSpotifyClientId() => config["Spotfy:ClientId"];

        public static string GetSpotifyClientSecret() => config["Spotify:ClientSecret"];
    }
}