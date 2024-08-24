using System.Diagnostics;
using System.Reflection;

namespace ProjectBo4Launcher
{
    public sealed class Updates
    {
        public static async Task CheckLauncherUpdates()
        {
            Console.Write("Checking for updates... ");

            File.Delete("Project-BO4-Launcher.delete"); // Replace this

            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

            var client = new HttpClient();
                string updateVesion = await client.GetStringAsync(@"https://raw.githubusercontent.com/Praveshan0710/TestingProject/Testing/launcher-version/"); // version file
                if (currentVersion != updateVesion)
                    UpdateLauncher();

            client.Dispose();
        }
        public static async void UpdateLauncher()
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(@""); //This needs to be the updated launcher file (exe)

            FileStream fileStream = File.Create("Project-BO4-Launcher.update");
            stream.CopyTo(fileStream);
            fileStream.Dispose();
            client.Dispose();

            const string oldLauncherName = "Project-BO4-Launcher.delete";

            string? currentLauncherName = Environment.ProcessPath;

            const string newLauncherName = "Project-BO4-Launcher.exe";

            currentLauncherName ??= newLauncherName;

            File.Move(currentLauncherName, oldLauncherName); // Mark the old one to delete on next launch
            File.Move(newLauncherName, currentLauncherName);

            RemoveFileWithDelay(oldLauncherName);

            Process.Start(currentLauncherName);
            Environment.Exit(0);
        }
        public static async Task DownloadUpdatedFile(string filename)
        {
            const string gitRepo = @"https://github.com/Praveshan0710/"; //project-bo4-data/files/clientDlls/mp/d3d11.dll -example of a file to update
            var client = new HttpClient();
            var stream = await client.GetStreamAsync($"{gitRepo}{filename.Replace(@"\", @"/")}");

            FileStream fileStream = File.Create(filename);
            stream.CopyTo(fileStream);

            fileStream.Dispose();
            client.Dispose();

            Console.WriteLine($"{filename} was updated");
        }
        public static void RemoveFileWithDelay(string oldLauncher)
        {
            Process.Start(new ProcessStartInfo()
            {
                Arguments = $@"/C choice /C Y /N /D Y /T 3 & Del ""{oldLauncher}""",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            });
        }



        //Original Launcher Functions outdated
        /*        public static async Task CheckForUpdates()
                {
                    Console.WriteLine("Checking for updates...");
                    const string currentVersion = @"https://github.com/bodnjenie14/Project_-bo4_Launcher/releases/download/release/Project_BO4_Launcher_Update_1.0.17.4.4.zip"; // Will need to change each update
                    const string updateURL = @"https://api.github.com/repos/bodnjenie14/Project_-bo4_Launcher/releases/latest";
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd("request"); // Returns 403 without this line, not sure why
                    var res = await client.GetAsync(updateURL);
                    try
                    {
                        if (await client.GetAsync(updateURL) != null) // Need to add warning for update failed but, let them continue to play without it
                        {
                            var stringResponse = await res.Content.ReadAsStringAsync();
                            var jsonResponse = JsonDocument.Parse(stringResponse).RootElement.GetProperty("remoteVersion");
                            var remoteVersion = JsonSerializer.Deserialize<JsonElement>(jsonResponse[0]).GetProperty("browser_download_url").ToString();
                            if (currentVersion != remoteVersion)
                            {
                                Console.WriteLine("Need to update...\nDownloading newest version...");
                                DownloadLauncherUpdate();
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
                private static async void DownloadLauncherUpdate()
                {
                    using (var client = new HttpClient())
                    {
                        var stream = await client.GetStreamAsync(@"https://github.com/skills/introduction-to-github/archive/refs/heads/main.zip"); // For faster testing
                        string downloadPath = Path.Combine(Path.GetTempPath(), "ProjectBo4Update.zip");
                        using (FileStream filestream = File.Create(downloadPath))
                        {
                            stream.CopyTo(filestream);
                        }
                        Console.WriteLine("The update download was completed.");
                        ExtractLauncherUpdate(downloadPath);
                    }
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
                }*/
    }
}
