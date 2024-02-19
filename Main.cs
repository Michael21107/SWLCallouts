using Rage;
using LSPD_First_Response.Mod.API;
using SWLCallouts.Callouts;
using SWLCallouts.VersionChecker;
using System.Reflection;

namespace SWLCallouts
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
            Settings.LoadSettings();
        }

        public override void Finally()
        {
            //Game.Console.Print("SWLCallouts has been cleaned up");
            Game.LogTrivial("SWLCallouts has been cleaned up");
        }

        static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
                GameFiber.StartNew(delegate
                {
                    RegisterCallouts();
                    Game.Console.Print();
                    Game.Console.Print("=============================================== SWLCallouts by Scottywonderful ================================================");
                    Game.Console.Print();
                    Game.Console.Print("[LOG]: Callouts and settings were loaded successfully.");
                    Game.Console.Print("[LOG]: The config file was loaded successfully.");
                    Game.Console.Print("[VERSION]: Detected Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    Game.Console.Print("[LOG]: Checking for a new SWLCallouts version...");
                    Game.Console.Print();
                    Game.Console.Print("=============================================== SWLCallouts by Scottywonderful ================================================");
                    Game.Console.Print();

                    // Check for updates and display version information
                    if (PluginCheck.isUpdateAvailable())
                        return;

                    // You can find all textures/images in OpenIV
                    string department = Settings.Department;
                    string icon = GetIconForDepartment(department);

                    // Display the notification for the unstable build
                    if (PluginCheck.IsAlphaBeta(Assembly.GetExecutingAssembly().GetName().Version.ToString()))
                    {
                        Game.DisplayNotification(icon, icon, "SWLCallouts", "~y~Unstable Build", "This is the latest ~r~unstable build~w~ of SWLCallouts. You may notice bugs while playing this unstable build.");
                    }
                    else
                    {
                        Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Detected the ~g~latest~w~ build of ~y~SWLCallouts~w~!");
                    }

                    // Display help messages or set HelpMessages to false
                    DisplayHelpOrSetHelpMessages();
                });
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
            Game.Console.Print();
            Game.Console.Print("================================================== SWLCallouts ===================================================");
            Game.Console.Print();
            if (Settings.HighSpeedChase) { Functions.RegisterCallout(typeof(HighSpeedChase)); }
            if (Settings.WelfareCheck) { Functions.RegisterCallout(typeof(WelfareCheck)); }
            Game.Console.Print("[LOG]: All callouts of the SWLCallouts.ini were loaded successfully.");
            Game.Console.Print();
            Game.Console.Print("================================================== SWLCallouts ===================================================");
            Game.Console.Print();
        }
        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
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
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}