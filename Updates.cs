using System.Text.Json;

namespace ProjectBo4Launcher
{
    internal class Updates
    {
        public static async Task CheckForUpdates() // Definitly replacing this with a version file method if people actually use my one
        {
            Console.WriteLine("Checking for updates...");
            const string currentVersion = @"https://github.com/bodnjenie14/Project_-bo4_Launcher/releases/download/release/Project_BO4_Launcher_Update_1.0.17.4.4.zip"; // Will need to change each update
            const string updateURL = @"https://api.github.com/repos/bodnjenie14/Project_-bo4_Launcher/releases/latest";
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request"); // Returns 403 without this line, not sure why
            var res = await client.GetAsync(updateURL);
            try
            {
                res.EnsureSuccessStatusCode();

                if (await client.GetAsync(updateURL) != null) // Need to add warning for update failed but, let them continue to play without it
                {
                    var stringResponse = await res.Content.ReadAsStringAsync();
                    var jsonResponse = JsonDocument.Parse(stringResponse).RootElement.GetProperty("assets");
                    var assets = JsonSerializer.Deserialize<JsonElement>(jsonResponse[0]).GetProperty("browser_download_url").ToString();
                    if (currentVersion != assets)
                    {
                        Console.WriteLine("Need to update...\nDownloading newest version...");
                        await UpdateLancher();
                    }
                    else
                    {
                        Console.WriteLine("Up to date");
                        
                    }
                }
                else
                {
                    Console.WriteLine("Something went wrong when checking for updates");
                }
            }
            catch (SystemException)
            {
                Console.WriteLine("Failed to check for updates");
            }
        }
        private static async Task UpdateLancher()
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(@"https://github.com/skills/introduction-to-github/archive/refs/heads/main.zip"); // For faster testing
            //var stream = await client.GetStreamAsync(assets);
            using FileStream filestream = File.Create(path: Path.Combine(Path.GetTempPath(), "ProjectBo4Update.zip"));
            stream.CopyTo(filestream);
            Console.WriteLine("The download was completed");
        }
    }
}
