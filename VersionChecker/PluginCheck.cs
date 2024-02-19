using System;
using Rage;
using System.Net;
using Newtonsoft.Json;
using SWLCallouts;
using SWLCallouts.VersionChecker;
using System.Text.Json.Serialization;

namespace SWLCallouts.VersionChecker
{
    public class Release
    {
        [JsonProperty("tag_name")]
        public string TagName { get; set; }
    }

    public class PluginCheck
    {
        public static bool isUpdateAvailable()
        {
            // Below is the version checker against the github releases //
            string curVersion = Settings.PluginVersion;
            string owner = "Scottywonderful"; // GitHub username
            string repo = "SWLCallouts"; // GitHub Repo Name
            Uri latestVersionUri = new Uri($"https://api.github.com/repos/{owner}/{repo}/releases/latest");

            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "request"); // GitHub API requires this user agent header
            string receivedData = string.Empty;
            string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

            try
            {
                receivedData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: Failed to check for an update.");
                Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                return false;
            }

            Release release = JsonConvert.DeserializeObject<Release>(receivedData);
            string latestVersion = release.TagName;

            if (latestVersion != curVersion)
            {
                string updateType = IsAlphaBeta(latestVersion) ? "Alpha/Beta" : "Stable";
                //Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~y~A new Update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~o~" + receivedData + "<br>~r~Please update to the latest build!");
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestVersion}~w~<br>Update Type: ~b~{updateType}~w~<br>~r~Please update to the latest build!");
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                Game.Console.Print("[LOG]: New Version:  " + latestVersion);
                Game.Console.Print("[LOG]: Update Type: " + updateType);
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                string updateType = IsAlphaBeta(latestVersion) ? "Alpha/Beta" : "Stable"; 
                if (IsAlphaBeta(curVersion))
                {
                    Game.DisplayNotification(icon, icon, "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                }
                else
                {
                    Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                }
                return false;
            }


            // IGNORE CODE BELOW TILL RELEASED ONTO LSPDFR //
            // Below is the version checker when I finally post to LSPDFR website //
            /*string curVersion = Settings.PluginVersion;
            Uri latestVersionUri = new Uri("https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=20730&textOnly=1");
            WebClient webClient = new WebClient();
            string receivedData = string.Empty;
            try
            {
                receivedData = webClient.DownloadString(latestVersionUri).Trim();
            }
            catch (WebException)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: Failed to check for an update.");
                Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                return false;
            }
            if (receivedData != Settings.PluginVersion)
            {
                Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~y~A new Update is available!", "Current Version: ~r~" + curVersion + "~w~<br>New Version: ~o~" + receivedData + "<br>~r~Please update to the latest build!");
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                Game.Console.Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                Game.Console.Print("[LOG]: New Version:  " + receivedData);
                Game.Console.Print();
                Game.Console.Print("================================================== SWLCallouts ===================================================");
                Game.Console.Print();
                return true;
            }
            else
            {
                Game.DisplayNotification(icon, icon, "SWLCallouts", "~y~Unstable Build", "This is an ~r~unstable build~w~ of SWLCallouts for testing. You may notice bugs while playing the unstable build.");
                Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                return false;
            }*/
        }

        internal static bool IsAlphaBeta(string version)
        {
            return version.StartsWith("A") || version.StartsWith("B") || version.EndsWith("A") || version.EndsWith("B");
        }

    }
}