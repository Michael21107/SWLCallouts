// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.8.4

#region

using System.Net.Http;
using System.Reflection;
using System.Text;

#endregion

namespace SWLCallouts;

public class Main : Plugin
{
    public override void Initialize()
    {
        Normal("SWLCallouts (Version:" + Assembly.GetExecutingAssembly().GetName().Version?.ToString() + ") initialised");
        Functions.OnOnDutyStateChanged += OnDutyStateChangedHandler;
        Normal("Go on duty to load SWLCallouts. Thanks for download it!");
    }

    private static void OnDutyStateChangedHandler(bool onDuty)
    {
        if (onDuty)
        {
            GameFiber.StartNew(() =>
            {
                // Loading INI and checking for updates //
                Normal("Loading INI file settings");
                Settings.LoadSettings();
                Normal("Adding console commands");
                Game.AddConsoleCommands();
                Normal("Registering callouts");
                SWLCalloutHandler.RegisterCallouts();
                AppDomain.CurrentDomain.DomainUnload += Cleanup;
                Normal("Checking loaded version");
                Print("=============================================== SWLCallouts by Scottywonderful ================================================");
                Print("[LOG]: Callouts and settings were loaded successfully.");
                Print("[LOG]: The config file was loaded successfully.");
                Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version?.ToString());
                Print("[LOG]: Checking for a new SWLCallouts version...");
                Print("=============================================== SWLCallouts by Scottywonderful ================================================");

                // Check for updates and display version information //
                Normal("Checking for updates and comparing..");
                PluginCheck.IsUpdateAvailable();

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
            Normal("Help messages enabled.");
            Game.DisplayHelp("You can change all ~y~keys~w~ in the ~g~SWLCallouts.ini~w~. Press ~b~" + Settings.EndCall + "~w~ to end a callout.", 5000);
        }
        else
        {
            Normal("Help messages disabled.");
            Settings.HelpMessages = false;
        }
    }

    // Helper method to get the icon for the department depending on internet connection //
    /*public static string GetIconForDepartment(string department)
    {
        string defaultImagePath = $"web_lossantospolicedept"; // Default image path/name if there's no internet connection or GitHub image isn't available //
        //string defaultImagePath = $"3dtextures"; // Default image path if there's no internet connection or GitHub image isn't available //
        //string defaultImageName = $"mpgroundlogo_cops"; // Default image Name if there's no internet connection or GitHub image isn't available //
    
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
    }*/

    private static void Cleanup(object sender, EventArgs e)
    {
        try
        {
            NotifyP("3dtextures", "mpgroundlogo_cops", "SWLCallouts", "~w~by ~p~SWL Creations", $"{Arrays.PluginUnloadText.PickRandom()}"); 
            // Unregistering Callouts and cleaning up //
            SWLCalloutHandler.DeregisterCallouts();

            Normal("Unloaded SWLCallouts Successfully.");
        }
        catch (Exception ex)
        {
            Error(ex, nameof(Cleanup));
        }
    }
    public override void Finally()
    {
        Normal("SWLCallouts has been cleaned up.");
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