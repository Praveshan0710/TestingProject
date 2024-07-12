using ProjectBO4Launcher;

namespace ProjectBo4Launcher
{
    internal class Program
    {
        public static void Main()
        {
            Console.Title = "Project BO4 Launcher Testing";

            //Check for launcher self update
            CheckFiles.CheckForMyUpdates();

            //Check for Blackops4
            if (!CheckFiles.isBlackOps4Dir())
            {
                Console.WriteLine("Couldn't find BlackOps4.exe, please run this appliction from your Black Ops 4 game directory.");
                Console.ReadKey(true);
                return;
            }

            //Check for updates to Dependancies (dlls)
            CheckFiles.CheckClientDlls();

            //Check for current .json settings
            LauncherSettings.GetClientSettings();

            //CheckFiles.CheckLPCFiles();

        }

    }
}
