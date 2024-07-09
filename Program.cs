
using ProjectBO4Launcher;

namespace ProjectBo4Launcher
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.Title = "Project BO4 Launcher";
            CheckFiles.CheckClientDlls();
            //CheckFiles.ProjectBo4DllsInGameDir();
            //CheckFiles.CheckLPCFiles();
            /*            if (!File.Exists("BlackOps4.exe"))
                        { 
                            Console.WriteLine("Couldn't find BlackOps4.exe, please run this appliction from your Black Ops 4 game directory.");
                            Console.ReadKey(true);
                            return;
                        }*/

            //await Updates.CheckForUpdates();

            /*            if (await Updates.CheckForUpdates())
                        {
                            Console.WriteLine("Need to Update");
                            HttpClient httpClient = new HttpClient();
                            var stream = await httpClient.GetStreamAsync()
                            using FileStream filestream = File.Create(path: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "update"));

                        }
                        else
                        {
                            Console.WriteLine("We are up to date");
                            Console.WriteLine(Path.GetFullPath(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "update")));
                        }*/
        }

    }
}
//assets.GetProperty("browser_download_url").ToString()
//var json = JsonDocument.Parse(stringres).RootElement.GetProperty("assets");

/*const string currentVersion = @"https://github.com/bodnjenie14/Project_-bo4_Launcher/releases/download/release/Project_BO4_Launcher_Update_1.0.17.4.4.zip";
const string updateURL = @"https://api.github.com/repos/bodnjenie14/Project_-bo4_Launcher/releases/latest";
var client = new HttpClient();
client.DefaultRequestHeaders.UserAgent.TryParseAdd("request"); // Returns 403 without this line, not sure why
var res = await client.GetAsync(updateURL);
if (res != null)
{
    var stringResponse = await res.Content.ReadAsStringAsync();
    var jsonResponse = JsonDocument.Parse(stringResponse).RootElement.GetProperty("assets");
    var assets = JsonSerializer.Deserialize<JsonElement>(jsonResponse[0]).GetProperty("browser_download_url").ToString();

    if (assets != currentVersion)
    {
        Console.WriteLine("update");
    }
    else
    {
        Console.WriteLine("uptodate");
    }
}*/
