// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.4.6.4

// Thanks for Astro for this idea

namespace SWLCallouts.Stuff;

internal static class Helper
{
    internal static Ped GPlayer => Game.LocalPlayer.Character; // Local Player's Character
    internal static Random Rndm = new(DateTime.Now.Millisecond); // Random Time/Date

    internal static string SWLicon => Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs
}

// Thanks for Astro for this idea