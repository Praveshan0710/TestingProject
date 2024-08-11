using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Settings
{
    internal class Files
    {
        public static bool ProjectBo4DllsInGameDir()
        {
            ReadOnlySpan<string> projectBo4Dlls = ["UMPDC.dll", "d3d11.dll"];

            foreach (string dll in projectBo4Dlls)
            {
                if (File.Exists(dll))
                    return true;
            }
            return false;
        }

        public static void CopyProjectBo4Dll()
        {
            //Depends on the user's choice
            throw new NotImplementedException();
        }

        public static void RemoveProjectBo4Dlls()
        {
            ReadOnlySpan<string> projectBo4Dlls = ["UMPDC.dll", "d3d11.dll"];
            foreach (var file in Directory.EnumerateFiles(@".\", "*.dll"))
            {
                if (projectBo4Dlls.Contains(Path.GetFileName(file)))
                {
                    File.Delete(file);
                    Debug.WriteLine($"Deleted {file}");
                }
            }
        }
        public static void CheckLPCFiles()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string gameLPCDirPath = Path.Combine(currentDir, "LPC");
            string launcherResources = Path.Combine(currentDir, "project-bo4-data", "files");
            var lpcDir = new DirectoryInfo(Path.Combine(launcherResources, "LPC"));
            if (!Directory.Exists(gameLPCDirPath))
                Directory.CreateDirectory(gameLPCDirPath);

            foreach (FileInfo file in lpcDir.GetFiles())
            {
                string expectedGameLPCFilePath = Path.Combine(gameLPCDirPath, file.Name);
                if (!File.Exists(expectedGameLPCFilePath))
                {
                    file.CopyTo(expectedGameLPCFilePath);
                }
            }

        }
        public static void CheckClientDlls() // testing
        {
            string clientDllsDir = @"project-bo4-data\files\clientDlls"; // New to Test in other funtions
            File.Delete("project-bo4-hashes.txt");
            foreach (var dir in Directory.GetDirectories(clientDllsDir))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    var stream = File.Open(file, FileMode.Open);
                    stream.Position = 0;
                    WriteFileHash(stream, file);
                    stream.Dispose();
                }
            }
            CompareHashFile();
        }
        private static void WriteFileHash(FileStream stream, string filename)
        {
            var sha1 = SHA1.Create();
            var hashString = BitConverter.ToString(sha1.ComputeHash((stream))).Replace("-", String.Empty);
            File.AppendAllText("project-bo4-hashes.txt", $"{hashString} {filename}\n");
            sha1.Dispose();
        }
        private static async void CompareHashFile()
        {
            var lines = File.ReadAllLines("launcher-hashes.txt");
            var updatedLines = File.ReadAllLines("updated.txt");  //Replace with logic to get the file to compare against for the internet
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != updatedLines[i])
                {
                    string filename = updatedLines[i].Substring(41); // discard the hash
                    Console.WriteLine($"Need to update {filename}");
#if !DEBUG
                    File.Delete(filename);
                    await Updates.DownloadUpdatedFile(filename);
#endif
                }
            }
        }

        public static void ExtractClientDLL(ref LauncherSettings launcherSettings)
        {
            const string clientDLLsDirectory = @"project-bo4-data\files\clientDlls\";
            if (launcherSettings.UseReShade)
                ZipFile.ExtractToDirectory(launcherSettings.onlineMode ?
                    clientDLLsDirectory + "reshade_mp.zip" :
                    clientDLLsDirectory + "reshade_solo.zip", @".\");
            else
                ZipFile.ExtractToDirectory(launcherSettings.onlineMode ?
                    clientDLLsDirectory + "mp.zip" :
                    clientDLLsDirectory + "solo.zip", @".\");
        }
    }

}

// Old function
/*        public static bool ProjectBo4DllsInGameDir() // Remove
        {
            ReadOnlySpan<string> projectBo4Dlls = ["UMPDC.dll", "d3d11.dll"];
            
            foreach (string dll in projectBo4Dlls)
            {
                if (File.Exists(dll))
                    return true;
            }
            return false;
        }*/

