using System.Text.Json;
using System.Text.Json.Nodes;

namespace ProjectBO4Launcher
{
    public class ClientSettings(string PlayerName, string IpAddress)
    {
        private string _playerName { get { return _playerName; } set { _playerName = PlayerName; } }
        private string _IpAddress { get { return _IpAddress;  } set { _IpAddress = IpAddress; } }
        public string PlayerName { get { return _playerName; } }
        public string IpAddress { get { return _IpAddress; } }
    }
    internal class LauncherSettings
    {
        public static void GetClientSettings()
        {
            string settingsFile = "project-bo4.json";
            if (!File.Exists(settingsFile))
            {
                CreateDefaultClientSettings(ref settingsFile);
            }

            string? jsonString = File.ReadAllText(settingsFile);

            JsonNode? jsonObject = JsonNode.Parse(jsonString);

#pragma warning disable CS8602 // Dereference of a possibly null reference. // I have no clue.
            var playerName = jsonObject["identity"]["name"].ToString();
            if (playerName.Length < 1)
            {
                playerName = Environment.UserName;
            }

            var demonwareIp = jsonObject["demonware"]["ipv4"].ToString();
            if (demonwareIp.Length < 7)
            {
                demonwareIp = "78.157.42.107";
            }
            ClientSettings clientettings = new ClientSettings(playerName, demonwareIp);

        }
        private static void CreateDefaultClientSettings(ref string settingsFile)
        {
            var defaultSettings = new
            {
                demonware = new { ipv4 = "78.157.42.107" }, // bodnjenie's server
                identity = new { name = Environment.UserName }
            };
            var settings = JsonSerializer.Serialize(defaultSettings, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(settingsFile, settings);
        }
    }
}

