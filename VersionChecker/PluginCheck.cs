// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.7.2

#region

using System.Net.Http;

#endregion

namespace SWLCallouts.VersionChecker;

public class PluginCheck
{
    public static bool IsUpdateAvailable()
    {
        // Below is the version checker against the github releases //
        string curVersion = Settings.PluginVersion;
        string owner = "Scottywonderful";
        string repo = "SWLCallouts";
        Uri latestVersionUri = new($"https://api.github.com/repos/{owner}/{repo}/releases/latest");

        using HttpClient httpClient = new();
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

                            if (updateType == "Unstable")
                            {
                                NotifyP("3dtextures", "mpgroundlogo_cops", "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                            }
                            else if (updateType == "Stable")
                            {
                                NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~g~Latest ~w~Build", $"{Arrays.PluginLoadText.PickRandom()}");
                            }
                            return false;
                        }
                        else if (String.Compare(curVersion, latestVersion) > 0)
                        {
                            // Display a message thanking the user for helping with testing
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~p~Oh, I see you!", "~g~Thank you ~b~for helping ~p~me out ~o~by testing ~w~this wonderful script!");
                            GameFiber.Wait(5000);
                            NotifyP("3dtextures", "mpgroundlogo_cops", "SWLCallouts", "~o~Unstable Test Build", "This must be a ~r~unstable ~p~wonder build ~w~of SWLCallouts. You may notice ~o~bugs ~w~while playing this unstable build.");
                            return false;
                        }
                        else
                        {
                            NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestVersion}~w~<br>Current Type: ~b~{updateType}~w~<br>~r~Please update to the latest build!");

                            Print("================================================== SWLCallouts ===================================================");
                            Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                            Print("[LOG]: Current Version:  " + curVersion);
                            Print("[LOG]: New Version:  " + latestVersion);
                            Print("================================================== SWLCallouts ===================================================");
                            return true;
                        }
                    }
                }
            }
            else
            {
                NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

                Print("================================================== SWLCallouts ===================================================");
                Print("[WARNING]: Failed to check for an update.");
                Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Print("================================================== SWLCallouts ===================================================");

                return false;
            }
        }
        catch (HttpRequestException)
        {
            NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

            Print("================================================== SWLCallouts ===================================================");
            Print("[WARNING]: Failed to check for an update.");
            Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
            Print("================================================== SWLCallouts ===================================================");

            return false;
        }
        return false;
    }
}
