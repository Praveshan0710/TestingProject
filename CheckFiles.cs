using System.Security.Cryptography;
using ProjectBo4Launcher;
using File = System.IO.File;

namespace ProjectBO4Launcher
{
    internal class CheckFiles
    {
        public static bool isBlackOps4Dir()
        {
            return File.Exists("BlackOps4.exe");
        }
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
        public static async Task CheckClientDlls() // new thing
        {
            string clientDllsDir = @"project-bo4-data\files\clientDlls"; // New to Test in other funtions
            File.Delete("launcher-hashes.txt");
            foreach (var dir in Directory.GetDirectories(clientDllsDir))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    using (var stream = File.Open(file, FileMode.Open))
                    {
                        stream.Position = 0;
                        WriteClientDllHashesToFile(stream, file);
                    }
                }
            }
            await CompareHashFile();
        }
        private static void WriteClientDllHashesToFile(FileStream stream, string filename)
        {
            using (var sha1 = SHA1.Create())
            {
                var hashString = BitConverter.ToString(sha1.ComputeHash((stream))).Replace("-", String.Empty);
                File.AppendAllText("launcher-hashes.txt", $"{hashString} {filename}\n");
            }
        }
        private static async Task CompareHashFile()
        {
            var lines = File.ReadAllLines("launcher-hashes.txt");
            var updatedLines = File.ReadAllLines("updated.txt");  //Replace with logic to get the file to compare against
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != updatedLines[i])
                {
                    string filename = updatedLines[i].Substring(41);
                    Console.WriteLine($"Need to update {filename}");
                    File.Delete(filename);
                    await Updates.DownloadUpdatedFile(filename);
                }
            }
        }
    }

}

