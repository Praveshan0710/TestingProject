using System.IO.Compression;
using System.Reflection;
using System.Text.Json;

namespace ProjectBo4Launcher
{
    internal class Updates
    {
        public static async Task CheckForMyUpdates()
        {
            Version? currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            using (var client  = new HttpClient())
            {
                string updateVesion = await client.GetStringAsync(@"https://github.com/Praveshan0710/ProjectBO4Launcher/raw/Testing/"); // version file
            }
        }
        public static async Task DownloadUpdatedFile(string filename) // Should move to updates
        {
            const string gitRepo = @"https://github.com/Praveshan0710/ProjectBO4Launcher/raw/Testing/"; //project-bo4-data/files/clientDlls/mp/d3d11.dll -example of a file to update
            var client = new HttpClient();
            var stream = await client.GetStreamAsync($"{gitRepo}{filename.Replace(@"\", @"/")}");
            FileStream fileStream = File.Create(filename);
            stream.CopyTo(fileStream);
            Console.WriteLine($"{filename} was updated");
        }
        public static async Task CheckForUpdates()
        {
            Console.WriteLine("Checking for updates...");
            const string currentVersion = @"https://github.com/bodnjenie14/Project_-bo4_Launcher/releases/download/release/Project_BO4_Launcher_Update_1.0.17.4.4.zip"; // Will need to change each update
            const string updateURL = @"https://api.github.com/repos/bodnjenie14/Project_-bo4_Launcher/releases/latest";
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request"); // Returns 403 without this line, not sure why
            var res = await client.GetAsync(updateURL);
            try
            {
                res.EnsureSuccessStatusCode(); // Replace with IF to avoid crash if failed

                if (await client.GetAsync(updateURL) != null) // Need to add warning for update failed but, let them continue to play without it
                {
                    var stringResponse = await res.Content.ReadAsStringAsync();
                    var jsonResponse = JsonDocument.Parse(stringResponse).RootElement.GetProperty("assets");
                    var assets = JsonSerializer.Deserialize<JsonElement>(jsonResponse[0]).GetProperty("browser_download_url").ToString();
                    if (currentVersion != assets)
                    {
                        Console.WriteLine("Need to update...\nDownloading newest version...");
                        await DownloadLauncherUpdate();
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
        private static async Task DownloadLauncherUpdate()
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(@"https://github.com/skills/introduction-to-github/archive/refs/heads/main.zip"); // For faster testing
            //var stream = await client.GetStreamAsync(assets);
            string downloadPath = Path.Combine(Path.GetTempPath(), "ProjectBo4Update.zip");
            using FileStream filestream = File.Create(downloadPath);
            stream.CopyTo(filestream);
            Console.WriteLine("The update download was completed.");
            ExtractLauncherUpdate(downloadPath);
        }
        private static void ExtractLauncherUpdate(string archive)
        {
            Console.WriteLine("Copying Launcher files to Current Folder");
            string gameDir = Directory.GetCurrentDirectory();
            Environment.CurrentDirectory = Path.GetTempPath();
            ZipFile.ExtractToDirectory(archive, gameDir);
            Console.WriteLine("Launcher files copied.");
            Console.WriteLine("Removing temporary files");
            File.Delete(archive);
        }
    }
}
