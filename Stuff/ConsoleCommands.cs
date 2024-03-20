// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.4.8.7

#region

using Rage.Attributes;

#endregion

namespace SWLCallouts.Stuff;

internal static class ConsoleCommands
{
    [ConsoleCommand("Refresh loaded SWLCallouts settings")]
    internal static void Command_SWLRefreshSettings() => Settings.LoadSettings();

    [ConsoleCommand("Change a SWLCallouts callout to be enabled/disabled (true/false)")]
    internal static void Command_SWLChangeCalloutSettings(string calloutName, bool isEnabled)
    {
        var ini = new InitializationFile(@"Plugins/LSPDFR/SWLCallouts.ini");

        // Set the value of the callout in the INI file
        ini.Write("Callouts", calloutName, isEnabled.ToString().ToLower());

        // Refresh settings by reloading them
        Settings.LoadSettings();
    }
}
