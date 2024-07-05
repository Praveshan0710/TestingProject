using System.Text.Json;

namespace ProjectBo4Launcher
{
    internal class Updates
    {
        public static async Task<bool> ShouldUpdate()
        {
            Console.WriteLine("Checking for updates...");
            const string currentVersion = @"https://github.com/bodnjenie14/Project_-bo4_Launcher/releases/download/release/Project_BO4_Launcher_Update_1.0.17.4.4.zip"; // Will need to change each update
            const string updateURL = @"https://api.github.com/repos/bodnjenie14/Project_-bo4_Launcher/releases/latest";
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request"); // Returns 403 without this line, not sure why
            var res = await client.GetAsync(updateURL);
            if (await client.GetAsync(updateURL) != null) // Need to add warning for update failed but, let them continue to play without it
            {
                var stringResponse = await res.Content.ReadAsStringAsync();
                var jsonResponse = JsonDocument.Parse(stringResponse).RootElement.GetProperty("assets");
                var assets = JsonSerializer.Deserialize<JsonElement>(jsonResponse[0]).GetProperty("browser_download_url").ToString();
                return (currentVersion != assets);
            }
            else
            {
                return false;
            }
        }
    }
}
