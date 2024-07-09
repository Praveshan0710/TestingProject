using System.Security.Cryptography;

namespace ProjectBO4Launcher
{
    internal class CheckFiles
    {
        public static void CopyProjectBo4Dll()
        {
            //Depends on the user's choice
        }
        public static bool ProjectBo4DllsInGameDir()
        {
            ReadOnlySpan<string> projectBo4Dlls = ["UMPDC.dll", "d3d11.dll"];
            foreach (string dll in projectBo4Dlls)
            {
                if (File.Exists(dll))
                {
                    return true;
                }
            }
            return false;
        }
        public static void CheckLPCFiles()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string gameLPCDirPath = Path.Combine(currentDir, "LPC");
            string launcherResources = Path.Combine(currentDir, "project-bo4", "files");
            var lpcDir = new DirectoryInfo(Path.Combine(launcherResources, "LPC"));
            if (!Directory.Exists(gameLPCDirPath))
            {
                Directory.CreateDirectory(gameLPCDirPath);
            }
            foreach (FileInfo file in lpcDir.GetFiles())
            {
                string expectedGameLPCFilePath = Path.Combine(gameLPCDirPath, file.Name);
                if (!File.Exists(expectedGameLPCFilePath))
                {
                    file.CopyTo(expectedGameLPCFilePath);
                }
            }

        }
        public static void CheckClientDlls() // new thing
        {
            string clientDllsDir = @"project-bo4-data\files\clientDlls"; // New to Test in other funtions
            foreach (var dir in Directory.GetDirectories(clientDllsDir))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    Console.WriteLine(file);
                    SHA256 sha256 = SHA256.Create();
                    var stream = File.Open(file, FileMode.Open);
                    stream.Position = 0;
                    var hashVal = sha256.ComputeHash(stream);
                    PrintByteArray(hashVal); //Display
                }
            }
        }
        // Used for testing
        public static void PrintByteArray(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]:X2}");
                if ((i % 4) == 3) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }

}

