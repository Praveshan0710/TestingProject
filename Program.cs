using System.Diagnostics;
using Settings;
using Windows.Win32;

namespace ProjectBo4Launcher
{
    internal class Program
    {
        public static void Main()
        {
            PInvoke.AllocConsole();
            Console.Title = "Project BO4 Launcher Testing";
            Console.CursorVisible = false;

            //Check for launcher self update
            try
            {
                Updates.CheckLauncherUpdates().RunSynchronously();
            }
            catch
            {
                DisplayMessage("Failed to update, skipping the update\n", ConsoleColor.Red);
            }

            //Check for Blackops4
            if (!File.Exists("BlackOps4.exe"))
            {
                DisplayMessage("Couldn't find BlackOps4.exe, please run this appliction from your Black Ops 4 game directory.", ConsoleColor.Red);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return;
            }

            //Check for updates to Dependancies (dlls)
            //Files.CheckClientDlls();

            //Check for current settings
            var clientsettings = Launcher.GetClientSettings();
            var launcherSettings = Launcher.GetLauncherSettings();

            //Options
            string? readInput;
            do
            {
                DisplayMessage($"Current Player Name: {clientsettings.PlayerName}\n" +
                               $"Current server IP: {clientsettings.DemonwareIP}\n" +
                               $"ReShade {(launcherSettings.UseReShade ? "On": "Off")}\n" +
                               $"{(launcherSettings.onlineMode ? "Online": "Offline")} Mode\n", ConsoleColor.Cyan);

                //Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("[N] Change your Player name");
                Console.WriteLine("[O] Toggle Online/Offline Mode");
                Console.WriteLine("[I] Change server IP");
                Console.WriteLine("[R] Toggle ReShade");
                Console.WriteLine("[P] Launch Project-Bo4");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.N:
                        Console.Clear();
                        Console.Write("Please enter your new name: ");
                        readInput = Console.ReadLine();
                        if (readInput != null && readInput.Trim() != string.Empty)
                        {
                            clientsettings.PlayerName = readInput;
                            Launcher.ChangeSettings(ref clientsettings);
                            DisplayMessage($"Player name {readInput}", ConsoleColor.Green);
                        }
                        else
                            DisplayMessage("Player name was not changed", ConsoleColor.Red);

                        WaitForInput();
                        Console.Clear();
                        break;

                    case ConsoleKey.O:
                        launcherSettings.onlineMode = !launcherSettings.onlineMode;
                        Launcher.UpdateLauncherSettingsFile(ref launcherSettings);
                        Console.Clear();
                        break;


                    case ConsoleKey.I:
                        Console.Clear();
                        Console.Write("Specify the server IP Address to connect to: ");
                        readInput = Console.ReadLine();
                        if (readInput != null && readInput.Trim() != string.Empty)
                        {
                            clientsettings.DemonwareIP = readInput;
                            Launcher.ChangeSettings(ref clientsettings);
                            DisplayMessage($"New server IP set: {readInput}", ConsoleColor.Green);
                        }
                        else
                            DisplayMessage("Server IP was not changed", ConsoleColor.Red);
                        WaitForInput();
                        Console.Clear();
                        break;

                    case ConsoleKey.R:
                        Console.Clear();
                        launcherSettings.UseReShade = !launcherSettings.UseReShade;
                        Launcher.UpdateLauncherSettingsFile(ref launcherSettings);
                        break;

                    case ConsoleKey.P:
                        {
                            PInvoke.FreeConsole();
                            Files.RemoveProjectBo4Dlls();
                            Files.ExtractClientDLL(ref launcherSettings);
                            Files.CheckLPCFiles();

                            //Final DLL Check
                            if (!Files.ProjectBo4DllsInGameDir())
                                MissingDLLExit();

                            //Launch Project BO4
                            var game = Process.Start("BlackOps4.exe");
                            //Hide until the game exits to try to remove the client dll afterward // To use bnet afterwards
                            game.WaitForExit();
                            Debug.WriteLine("Exited");
                            ReadOnlySpan<string> files = ["d3d11.dll", "UMPDC.dll"];
                            foreach (var file in files)
                                File.Delete(file);
                            return;
                        }

                    default:
                        Console.Clear();
                        break;
                }
            } while(true);
        }

        public static void DisplayMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void WaitForInput()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);
        }
        public static void MissingDLLExit()
        {
            var response = PInvoke.MessageBox(Windows.Win32.Foundation.HWND.Null,
                                             "Could not find required Project-Bo4 DLL\n" +
                                            "You may need to adjust your antivirus settings\n" +
                                            "Press OK for more information",
                                            "Error",
                                            Windows.Win32.UI.WindowsAndMessaging.MESSAGEBOX_STYLE.MB_OKCANCEL
                                            | Windows.Win32.UI.WindowsAndMessaging.MESSAGEBOX_STYLE.MB_SERVICE_NOTIFICATION
                                            | Windows.Win32.UI.WindowsAndMessaging.MESSAGEBOX_STYLE.MB_ICONERROR);

            if (response == Windows.Win32.UI.WindowsAndMessaging.MESSAGEBOX_RESULT.IDOK)
            {
                Process.Start(new ProcessStartInfo(@"https://shield-bo4.gitbook.io/document/launcher-guide/how-to-add-game-folder-exception-in-windows-defender") { UseShellExecute = true });
            }

            Environment.Exit(0);
        }
    }
}
