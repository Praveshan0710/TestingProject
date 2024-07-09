namespace ProjectBO4Launcher
{
    internal class LauncherSettings
	{
/*		private string Playername = "Unknown Soldier";
		public string _playername
		{
            get
            {
                return Playername;
            }
            set
            {
                Playername = _playername; 
            }
        }
        private bool Reshade;
        public bool _reshade
        {
            get
            {

                return Reshade;
            }
            set
            {
                Reshade = _reshade;
            }
        }*/


        class PlayerSettings
        {
            // This is all we care about to launch the game
            string DemonWareServerIP;
            string PlayerName;
        }

        private static void GetLauncherSettings()
        {
            string settingsFile = "project-bo4.json";
            //Get and set these from project-bo4.json later
            if (!File.Exists(settingsFile))
            {
                File.Create(settingsFile);
            }
        }
    }


}
