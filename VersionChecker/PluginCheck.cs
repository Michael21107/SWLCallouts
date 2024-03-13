// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.8.0

#region

using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

#endregion

namespace SWLCallouts.VersionChecker;

public class PluginCheck
{
    public static bool IsUpdateAvailable()
    {
        /////////////////////////////////////////////////////////
        ////**********GITHUB + LSPDFR CHECKER BELOW**********////
        /////////////////////////////////////////////////////////


        Log("Checking installed version type");
        string versionType = Settings.VersionType;
        string curType = (versionType == "Alpha" || versionType == "Beta" || versionType == "alpha" || versionType == "beta") ? "Unstable" : "Stable";

        // Below is the version checker against the github releases //
        Log("Checking installed version");
        string curVersion = Settings.PluginVersion;
        Log("Checking LSPDFR version");
        Uri latestPubVersionUri = new($"https://www.lcpdfr.com/applications/downloadsng/interface/api.php?do=checkForUpdates&fileId=46914&textOnly=1");
        Log("Checking Github version");
        string owner = "Scottywonderful";
        string repo = "SWLCallouts";
        Uri latestTestingVersionUri = new($"https://api.github.com/repos/{owner}/{repo}/releases/latest");

        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "request");

        try
        {
            Log("Get a response for Pub");
            HttpResponseMessage responsePub = httpClient.GetAsync(latestPubVersionUri).Result;
            Log("Get a response for Test");
            HttpResponseMessage responseTest = httpClient.GetAsync(latestTestingVersionUri).Result;

            if (curType == "Stable" && responsePub.IsSuccessStatusCode)
            {
                Log("Response from LSPDFR received");
                string receivedPubData = responsePub.Content.ReadAsStringAsync().Result;
                string latestPubVersion = receivedPubData.Trim();
                    
                if (latestPubVersion == curVersion)
                {
                    NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~g~Latest ~w~Build", $"{Arrays.PluginLoadText.PickRandom()}");
                    return false;
                }
                else if (String.Compare(curVersion, latestPubVersion) > 0)
                {
                    // Display a message thanking the user for helping with testing
                    NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", $"{Arrays.PluginLoadTestingSubtitle.PickRandom()}", "~r~[ERROR] ~w~Version Type issue!<br>~w~Please update this plugin ~r~ASAP~w~!<br>~y~Public release version detected!");
                    GameFiber.Wait(5000);
                    NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~o~Unstable Build", "This must be a mistake, please check as you are on a ~p~future ~y~test build ~w~as you may notice ~o~bugs ~w~while playing this unstable build version.");
                    return false;
                }
                else
                {
                    NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts ~y~Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestPubVersion}~w~<br>Please ~b~update ~w~to the latest build ~r~ASAP!");

                    Print("================================================== SWLCallouts ===================================================");
                    Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                    Print("[LOG]: Current Version:  " + curVersion);
                    Print("[LOG]: New Version:  " + latestPubVersion);
                    Print("================================================== SWLCallouts ===================================================");
                    return true;
                }
            }
            else if (curType == "Unstable" && responseTest.IsSuccessStatusCode)
            {
                Log("Response from Github received");
                string receivedTestData = responseTest.Content.ReadAsStringAsync().Result;
                Log("Response from LSPDFR received");
                string receivedPubData = responsePub.Content.ReadAsStringAsync().Result;
                string latestPubVersion = receivedPubData.Trim();

                string startTag = "\"tag_name\":\"";
                int startIndex = receivedTestData.IndexOf(startTag);
                if (startIndex != -1)
                {
                    startIndex += startTag.Length;
                    int endIndex = receivedTestData.IndexOf('"', startIndex);
                    if (endIndex != -1)
                    {
                        string latestTestVersion = receivedTestData.Substring(startIndex, endIndex - startIndex);

                        if (latestTestVersion == curVersion && String.Compare(curVersion, latestPubVersion) < 0)
                        {
                            NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts ~y~Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestPubVersion}~w~<br>Current Type: ~b~{curType}~w~<br>~r~Please update to the latest Public build from LSPDFR!");

                            Print("================================================== SWLCallouts ===================================================");
                            Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                            Print("[LOG]: Current Version:  " + curVersion);
                            Print("[LOG]: New Public Version:  " + latestPubVersion);
                            Print("================================================== SWLCallouts ===================================================");
                            return true;
                        }
                        else if (latestTestVersion == curVersion)
                        {
                            // Display a message thanking the user for helping with testing
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", $"{Arrays.PluginLoadTestingSubtitle.PickRandom()}", $"{Arrays.PluginLoadTestingText.PickRandom()}");
                            GameFiber.Wait(5000);
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~o~Unstable Test Build", "This must be a ~r~unstable ~p~wonder build ~w~of SWLCallouts. You may notice ~o~bugs ~w~while playing this unstable build.");
                            return false;
                        }
                        else if (String.Compare(curVersion, latestTestVersion) > 0)
                        {
                            // Display a message thanking the user for helping with testing
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", $"{Arrays.PluginLoadTestingSubtitle.PickRandom()}", $"{Arrays.PluginLoadTestingText.PickRandom()}");
                            GameFiber.Wait(5000);
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~o~Unstable Test Build", "This must be a ~r~unstable ~p~wonder build ~w~of SWLCallouts. You may notice ~o~bugs ~w~while playing this unstable build.");
                            return false;
                        }
                        else
                        {
                            NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts ~y~Warning", "~y~A new Update is available!", $"Current Version: ~r~{curVersion}~w~<br>New Version: ~o~{latestTestVersion}~w~<br>Current Type: ~b~{curType}~w~<br>~r~Please update to the latest Testing build from Github!");

                            Print("================================================== SWLCallouts ===================================================");
                            Print("[WARNING]: A new version of SWLCallouts is available! Update to the latest build or play on your own risk.");
                            Print("[LOG]: Current Version:  " + curVersion);
                            Print("[LOG]: New Testing Version:  " + latestTestVersion);
                            Print("================================================== SWLCallouts ===================================================");
                            return true;
                        }
                    }
                }

            }
            else
            {
                Log("Failed to receive current version type response");
                NotifyP("commonmenu", "mp_alerttriangle", "~w~SWLCallouts ~y~Warning", "~r~Failed to check for an update", "Please make sure you are ~y~connected~w~ to the internet or try to ~y~reload~w~ the plugin.");

                Print("================================================== SWLCallouts ===================================================");
                Print("[WARNING]: Failed to check for an update.");
                Print("[LOG]: Please make sure you are connected to the internet or try to reload the plugin.");
                Print("================================================== SWLCallouts ===================================================");

                return false;
            }
        }
        catch (HttpRequestException)
        {
            Log("No internet connection? - unable to connect to github.");
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
