﻿// Author: Scottywonderful
// Date: 16th Feb 2024  ||  Last Modified: 29th Feb 2024
// Version: 0.4.2.0

using Rage;
using LSPD_First_Response.Mod.API;
using SWLCallouts.Callouts;
using SWLCallouts.VersionChecker;
using System;
using System.Reflection;
using System.Drawing;

namespace SWLCallouts
{
    public class Main : Plugin
    {
        public override void Finally()
        {
            Game.LogTrivial("SWLCallouts has been cleaned up");
        }

        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
            Settings.LoadSettings();
            // Subscribe to LSPDFR plugin crash event
            //LSPD_First_Response.Mod.API.Events.OnLSPDFRCrash += HandleLSPDFRCrash; //TO BE ADDED
            //Functions.OnLSPDFRCrash += OnLSPDFRCrash;
        }
        // TO BE ADDED -- Crash detection //
        /*internal void HandleLSPDFRCrash(object sender, EventArgs e)
        {
            // You can find all textures/images in OpenIV
            string department = Settings.Department;
            string icon = GetIconForDepartment(department);

            // Array of random crash response messages
            // string[] crashResponses = {
            //    "Ahh, I see you have crashed.",
            //    "Crash detected! Contacting Emergency Services... JOKES!",
            //    "We have detected a crash, please report this so we can see."
            //};

            // Select a random response from the array
            //string randomResponse = crashResponses[new Random().Next(crashResponses.Length)];

            // Display the random crash response to the player
            //Game.DisplayNotification(icon ?? "", icon ?? "", "SWLCallouts", "~r~Crash Detected!\n", +randomResponse);
            //Game.DisplaySubtitle(randomResponse, 5000);

            // Log the crash message
            //Game.Console.Print("SWLCallouts -- LSPDFR plugin has crashed. Response: " + randomResponse);

            // Display a message to the player when LSPDFR plugin crashes
            Game.DisplayNotification(icon ?? "", icon ?? "", "SWLCallouts", "~r~Crash Detected", "I believe I detected a crash! Was this from me? If so, please report it so I can be fixed!"); 
            Game.DisplaySubtitle("Ahh, I see you have crashed.", 5000);
            Game.Console.Print("SWLCallouts -- LSPDFR plugin has crashed.");
            DisplayRandomCrashMessage();
        }*/

        static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                GameFiber.StartNew(() =>
                {
                    RegisterCallouts();
                    Game.Console.Print("=============================================== SWLCallouts by Scottywonderful ================================================");
                    Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                    Game.Console.Print("[LOG]: The config file was loaded successfully.");
                    Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version?.ToString());
                    Game.Console.Print("[LOG]: Checking for a new SWLCallouts version...");
                    Game.Console.Print("=============================================== SWLCallouts by Scottywonderful ================================================");

                    // Check for updates and display version information
                    if (PluginCheck.IsUpdateAvailable())
                        return;

                    // You can find all textures/images in OpenIV
                    string department = Settings.Department;
                    string icon = GetIconForDepartment(department);

                    // Check version type
                    string versionType = Settings.VersionType;
                    string updateType = (versionType == "Alpha" || versionType == "Beta" || versionType == "alpha" || versionType == "beta") ? "Unstable" : "Stable";

                    // Display the notification for the currently installed build type
                    if (updateType == "Unstable")
                    {
                        Game.DisplayNotification(icon ?? "", icon ?? "", "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                    }
                    else
                    {
                        Game.DisplayNotification(icon ?? "", icon ?? "", "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                    }

                    // Display help messages or set HelpMessages to false
                    DisplayHelpOrSetHelpMessages();
                });
            }
        }

        static void DisplayHelpOrSetHelpMessages()
        {
            GameFiber.Wait(300);
            if (Settings.HelpMessages)
            {
                Game.DisplayHelp("You can change all ~y~keys~w~ in the ~g~SWLCallouts.ini~w~. Press ~b~" + Settings.EndCall + "~w~ to end a callout.", 5000);
            }
            else
            {
                Settings.HelpMessages = false;
            }
        }

        // Helper method to get the icon for the department
        public static string GetIconForDepartment(string department)
        {
            switch (department)
            {
                case "police":
                    return "web_lossantospolicedept";
                case "lssheriff":
                    return "web_lossantossheriffdept";
                case "sheriff":
                    return ""; // Insert appropriate icon
                case "highway":
                    return ""; // Insert appropriate icon
                case "FIB":
                    return "web_fib";
                case "IAA":
                    return ""; // Insert appropriate icon
                case "lsfire":
                    return "web_lossantosfiredept";
                case "lsems":
                    return "web_lossantosmedicalcenter";
                default:
                    return "web_lossantospolicedept"; // Default icon
            }
        }

        private static void RegisterCallouts() //Register all your callouts here
        {
            Game.Console.Print("========================================== Start of callout loading for SWLCallouts ==========================================");
            if (Settings.HighSpeedChase) { Functions.RegisterCallout(typeof(SWLHighSpeedChase)); }
            if (Settings.WelfareCheck) { Functions.RegisterCallout(typeof(SWLWelfareCheck)); }
            Game.Console.Print("[LOG]: All callouts of the SWLCallouts.ini were loaded successfully.");
            Game.Console.Print("========================================== End of callout loading for SWLCallouts ==========================================");
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
}