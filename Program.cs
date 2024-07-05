
namespace ProjectBo4Launcher
{
    internal class Program
    {
        public static async Task Main()
        {
            if (await Updates.ShouldUpdate())
            {
                Console.WriteLine("Need to Update");
            }
            else
            {
                Console.WriteLine("We are up to date");
            }
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
