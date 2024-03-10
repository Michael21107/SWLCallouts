// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.4.6.4

#region

using Rage.Attributes;

#endregion

namespace SWLCallouts.Stuff;

internal static class ConsoleCommands
{
    [ConsoleCommand]
    public static void Command_SWLRefreshSettings()
    {
        Settings.LoadSettings();
    }

    [ConsoleCommand]
    internal static void Command_SWLChangeCalloutSettings(string calloutName, bool isEnabled)
    {
        var path = "Plugins/LSPDFR/SWLCallouts.ini";
        var ini = new InitializationFile(path);

        // Set the value of the callout in the INI file
        ini.Write(calloutName, isEnabled.ToString().ToLower(), "Callouts");

        // Refresh settings by reloading them
        Settings.LoadSettings();
    }
}
