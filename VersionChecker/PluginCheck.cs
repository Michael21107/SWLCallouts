// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.4.5

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
                                string versionType = Settings.VersionType;
                                string updateType = (versionType == "Alpha" || versionType == "Beta" || versionType == "alpha" || versionType == "beta") ? "Unstable" : "Stable";

                                if (latestVersion == curVersion)
                                {
                                    string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

                                    if (updateType == "Unstable")
                                    {
                                        Game.DisplayNotification(icon, icon, "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                                    }
                                    else if (updateType == "Stable")
                                    {
                                        Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                                    }
                                    return false;
                                }
                                else if (String.Compare(curVersion, latestVersion) > 0)
                                {
                                    // Display a message thanking the user for helping with testing
                                    string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs
                                    Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~p~Oh, I see you!", "~g~Thank you ~b~for helping ~p~me out ~o~by testing ~w~this wonderful script!");
                                    Game.DisplayNotification(icon, icon, "SWLCallouts", "~o~Unstable Test Build", "This must be a ~r~unstable ~p~wonder build ~w~of SWLCallouts. You may notice ~o~bugs ~w~while playing this unstable build.");
                                    return false;
                                }
                                else
                                {
                                    Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestVersion}~w~<br>Current Type: ~b~{updateType}~w~<br>~r~Please update to the latest build!");
                                    
                                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                                    Game.Console.Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                                    Game.Console.Print("[LOG]: Current Version:  " + curVersion);
                                    Game.Console.Print("[LOG]: New Version:  " + latestVersion);
                                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

                        Game.Console.Print("================================================== SWLCallouts ===================================================");
                        Game.Console.Print("[WARNING]: Failed to check for an update.");
                        Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                        Game.Console.Print("================================================== SWLCallouts ===================================================");

                        return false;
                    }
                }
                catch (HttpRequestException)
                {
                    Game.DisplayNotification("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

                    Game.Console.Print("================================================== SWLCallouts ===================================================");
                    Game.Console.Print("[WARNING]: Failed to check for an update.");
                    Game.Console.Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                    Game.Console.Print("================================================== SWLCallouts ===================================================");

                    return false;
                }
                return false;
            }
        }
    }
}
