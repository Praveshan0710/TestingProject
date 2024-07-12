using ProjectBO4Launcher;

namespace ProjectBo4Launcher
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.Title = "Project BO4 Launcher";
            //Check for launcher self update

            //Check for Blackops4
            if (!CheckFiles.isBlackOps4Dir())
            {
                Console.WriteLine("Couldn't find BlackOps4.exe, please run this appliction from your Black Ops 4 game directory.");
                Console.ReadKey(true);
                return;
            }
            //Check for updaates to Dependancies (dlls)

            //Check for current .json settings
            LauncherSettings.GetClientSettings();
            //Updates.CheckForMyUpdates();

            //await CheckFiles.CheckClientDlls();

            //CheckFiles.ProjectBo4DllsInGameDir();

            //CheckFiles.CheckLPCFiles();

        }

    }
}
