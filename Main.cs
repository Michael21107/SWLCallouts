// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.6.4

#region

using System.Net.Http;
using System.Reflection;
using System.Text;
using static SWLCallouts.ReqHelper;

#endregion

namespace SWLCallouts;

public class Main : Plugin
{
    public override void Initialize()
    {
        Log("Plugin");
        Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
    }

    private static void Functions_OnOnDutyStateChanged(bool onDuty)
    {
        bool isOnDuty = onDuty;
        if (isOnDuty)
        {
            GameFiber.StartNew(() =>
            {
                // Loading INI and checking for updates //
                Log("Loading INI file settings");
                Settings.LoadSettings();
                Log("Adding console commands");
                Game.AddConsoleCommands();
                Log("Registering callouts");
                SWLCalloutHandler.RegisterCallouts();
                Log("Checking loaded version");
                Print("=============================================== SWLCallouts by Scottywonderful ================================================");
                Print("[LOG]: Callouts and settings were loaded successfully.");
                Print("[LOG]: The config file was loaded successfully.");
                Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version?.ToString());
                Print("[LOG]: Checking for a new SWLCallouts version...");
                Print("=============================================== SWLCallouts by Scottywonderful ================================================");

                // Check for updates and display version information //
                Log("Checking for updates and comparing..");
                PluginCheck.IsUpdateAvailable();

                // Check version type //
                Log("Version checking, is it stable?");
                string versionType = Settings.VersionType;
                string updateType = (versionType == "Alpha" || versionType == "Beta" || versionType == "alpha" || versionType == "beta") ? "Unstable" : "Stable";

                // Display the notification for the currently installed build type //
                if (updateType == "Unstable")
                {
                    Log("Unstable Version Installed");
                    NotifyP(SWLicon, SWLicon, "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                }
                else
                {
                    Log("Stable Version Installed");
                    NotifyP(SWLicon, SWLicon, "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                }

                // Display help messages or set HelpMessages to false //
                SetHelpMessages();
            });
        }
    }

    static void SetHelpMessages()
    {
        GameFiber.Wait(300);
        if (Settings.HelpMessages)
        {
            Log("Help messages enabled.");
            Game.DisplayHelp("You can change all ~y~keys~w~ in the ~g~SWLCallouts.ini~w~. Press ~b~" + Settings.EndCall + "~w~ to end a callout.", 5000);
        }
        else
        {
            Log("Help messages disabled.");
            Settings.HelpMessages = false;
        }
    }

    // Helper method to get the icon for the department //
    public static string GetIconForDepartment(string department)
    {
        string defaultImagePath = $"web_lossantospolicedept"; // Default image path if there's no internet connection or GitHub image isn't available //

        try
        {
            using var client = new HttpClient();
            string user = "Scottywonderful";
            string repo = "SWLCallouts";
            string imagePath = $"https://raw.githubusercontent.com/{user}/{repo}/master/Images/{department}.png";

            // Attempt to make a request to check if internet connection is available //
            HttpResponseMessage response = client.GetAsync("https://www.google.com/").Result;

            if (response.IsSuccessStatusCode)
            {
                // Internet connection is available, return GitHub image path //
                return imagePath;
            }
            else
            {
                // Internet connection is not available, return default image path //
                return defaultImagePath;
            }
        }
        catch (HttpRequestException)
        {
            // Exception occurred, likely due to no internet connection, return default image path //
            return defaultImagePath;
        }
    }

    private static void Cleanup(object sender, EventArgs e)
    {
        try
        {
            NotifyP(SWLicon, SWLicon, "SWLCallouts", "~y~by SWL Creations", $"{PluginUnloadText.PickRandom()}"); 
            //
            SWLCalloutHandler.DeregisterCallouts();

            Log("Unloaded SWLCallouts Successfully.");
        }
        catch (Exception ex)
        {
            Error(ex, nameof(Cleanup));
        }
    }

    public override void Finally()
    {
        Log("SWLCallouts has been cleaned up");
    }

    public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
    {
        foreach (Assembly assembly in Functions.GetAllUserPlugins())
        {
            if (args.Name?.ToLower().Contains(assembly.GetName().Name?.ToLower()) ?? false)
            {
                return assembly;
            }
        }
        return null;
    }

    public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
    {
        foreach (Assembly assembly in Functions.GetAllUserPlugins())
        {
            AssemblyName an = assembly.GetName();
            if (an.Name?.ToLower() == Plugin.ToLower())
            {
                if (minversion == null || an.Version?.CompareTo(minversion) >= 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}