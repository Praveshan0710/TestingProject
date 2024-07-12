using System.Text.Json;
using System.Text.Json.Nodes;

namespace ProjectBO4Launcher
{
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
/*        private static string GetJsonValue(ref string settingsFile)
        {
            //string? jsonString = File.ReadAllText(settingsFile);

            JsonNode? jsonObject = JsonNode.Parse(File.ReadAllText(settingsFile));
            
        }*/
    }
}

