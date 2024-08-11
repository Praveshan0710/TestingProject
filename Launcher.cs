using System.Text.Json;
using System.Text.Json.Nodes;

namespace Settings
{
    internal class ClientSettings(string playerName, string demonwareIP)
    {
        public string PlayerName = playerName;
        public string DemonwareIP = demonwareIP;
    }

    public class LauncherSettings(bool UseReShade, bool onlineMode)
    {
        public bool UseReShade = UseReShade;
        public bool onlineMode = onlineMode;
    }
    internal class Launcher
    {
        private const string clientSettingsFile = "project-bo4.json";
        private const string launcherSettingsFile = @"project-bo4-data\files\launcher-settings.json";

        public static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
        };

        public static ClientSettings GetClientSettings()
        {
            if (!File.Exists(clientSettingsFile))
                CreateDefaultClientSettingsFile();

            string jsonString = File.ReadAllText(clientSettingsFile);

            JsonNode settingsObject = JsonNode.Parse(jsonString)!;

            var playerName = settingsObject["identity"]!["name"]?.ToString() ?? string.Empty;
            if (playerName.Length < 1)
                playerName = Environment.UserName;

            var demonwareIp = settingsObject["demonware"]!["ipv4"]?.ToString() ?? string.Empty;
            if (demonwareIp.Length < 7)
                demonwareIp = "78.157.42.107";

            return new ClientSettings(playerName, demonwareIp);
        }

        private static void CreateDefaultClientSettingsFile()
        {
            var defaultSettings = new
            {
                demonware = new { ipv4 = "78.157.42.107" }, // bodnjenie's server
                identity = new { name = Environment.UserName }
            };

            var settings = JsonSerializer.Serialize(defaultSettings, jsonOptions);
            
            File.WriteAllText(clientSettingsFile, settings);
        }

        public static void ChangeSettings(ref ClientSettings clientSettings)
        {
            string jsonString = File.ReadAllText(clientSettingsFile);
            JsonNode settingsObject = JsonNode.Parse(jsonString)!;
            settingsObject["identity"]!["name"] = clientSettings.PlayerName;
            settingsObject["demonware"]!["ipv4"] = clientSettings.DemonwareIP;
            File.WriteAllText(clientSettingsFile, JsonSerializer.Serialize(settingsObject, jsonOptions));
        }
        public static bool GetReshadePref()
        {

            if (!File.Exists(launcherSettingsFile))
            {
                // Create a default one
                CreateLauncherSettingsFile();
            }
            JsonNode jsonNode = JsonNode.Parse(File.ReadAllText(launcherSettingsFile))!;
            var reshadeString = jsonNode["UseReShade"]?.ToString() ?? string.Empty;
            return reshadeString == "true";
        }

        private static void CreateLauncherSettingsFile()
        {
            var launcherSettings = new
            {
                UseReShade = false,
            };

            File.WriteAllText(launcherSettingsFile,
                JsonSerializer.Serialize(launcherSettings, Launcher.jsonOptions));
        }

        public static void ChangeReShadePref(bool reshade)
        {
            JsonNode jsonNode = JsonNode.Parse(File.ReadAllText(launcherSettingsFile))!;
            jsonNode["UseReShade"] = reshade;
            File.WriteAllText(launcherSettingsFile, JsonSerializer.Serialize(jsonNode, Launcher.jsonOptions));
        }

        public static LauncherSettings GetLauncherSettings()
        {
            if (!File.Exists(launcherSettingsFile))
                CreateDefaultLauncherSettingsFile();

            JsonNode settingsObject = JsonNode.Parse(File.ReadAllText(launcherSettingsFile))!;

            bool UseReShade = settingsObject["UseReShade"]?.ToString() == "true";

            bool onlineMode = settingsObject["onlineMode"]?.ToString() == "true";

            return new LauncherSettings(UseReShade, onlineMode);
        }

        private static void CreateDefaultLauncherSettingsFile()
        {
            var defaultSettings = new
            {
                UseReShade = false,
                onlineMode = true
            };

            var settings = JsonSerializer.Serialize(defaultSettings, jsonOptions);

            File.WriteAllText(launcherSettingsFile, settings);
        }

        public static void UpdateLauncherSettingsFile(ref LauncherSettings launcherSettings)
        {
            JsonNode settingsObject = JsonNode.Parse(File.ReadAllText(launcherSettingsFile))!;
            settingsObject["UseReShade"] = launcherSettings.UseReShade;
            settingsObject["onlineMode"] = launcherSettings.onlineMode;
            File.WriteAllText(launcherSettingsFile, JsonSerializer.Serialize(settingsObject, jsonOptions));
        }
    }
}

