// Author: Scottywonderful
// Date: 16th Feb 2024  ||  Last Modified: 29th Feb 2024
// Version: 0.4.2.0

using System;
using System.Net.Http;
using Rage;
using SWLCallouts;

namespace SWLCallouts.VersionChecker
{
    public class PluginCheck
    {
        public static bool IsUpdateAvailable()
        {
            // Below is the version checker against the github releases //
            string curVersion = Settings.PluginVersion;
            string owner = "Scottywonderful";
            string repo = "SWLCallouts";
            Uri latestVersionUri = new Uri($"https://api.github.com/repos/{owner}/{repo}/releases/latest");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "request");

                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(latestVersionUri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string receivedData = response.Content.ReadAsStringAsync().Result;

                        string startTag = "\"tag_name\":\"";
                        int startIndex = receivedData.IndexOf(startTag);
                        if (startIndex != -1)
                        {
                            startIndex += startTag.Length;
                            int endIndex = receivedData.IndexOf('"', startIndex);
                            if (endIndex != -1)
                            {
                                string latestVersion = receivedData.Substring(startIndex, endIndex - startIndex);

                                if (latestVersion == curVersion)
                                {
                                    string versionType = Settings.VersionType;
                                    string updateType = (versionType == "Alpha" || versionType == "Beta" || versionType == "alpha" || versionType == "beta") ? "Unstable" : "Stable";
                                    string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

                                    if (updateType == "Unstable")
                                    {
                                        Game.DisplayNotification(icon, icon, "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                                    }
                                    else
                                    {
                                        Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                                    }
                                    return false;
                                }
                                else
                                {
                                    string updateType = "Stable";
                                    Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestVersion}~w~<br>Update Type: ~b~{updateType}~w~<br>~r~Please update to the latest build!");

                                    Game.Console.Print();
                                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                                    Game.Console.Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                                    Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                                    Game.Console.Print("[LOG]: New Version:  " + latestVersion);
                                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                                    Game.Console.Print();
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

                        Game.Console.Print();
                        Game.Console.Print("================================================== SWLCallouts ===================================================");
                        Game.Console.Print("[WARNING]: Failed to check for an update.");
                        Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                        Game.Console.Print("================================================== SWLCallouts ===================================================");
                        Game.Console.Print();

                        return false;
                    }
                }
                catch (HttpRequestException)
                {
                    Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

                    Game.Console.Print();
                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                    Game.Console.Print("[WARNING]: Failed to check for an update.");
                    Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                    Game.Console.Print();

                    return false;
                }
                return false;
            }
        }
    }
}
