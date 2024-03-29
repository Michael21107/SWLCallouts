// Author: Scottywonderful
// Created: 10th Mar 2024
// Version: 0.4.8.8

// Thanks for Astro for this idea

#region

#endregion

namespace SWLCallouts.Stuff;

internal static class Helper
{
    internal static Ped GPlayer => Game.LocalPlayer.Character; // Local Player's Character
    internal static Random Rndm = new(DateTime.Now.Millisecond); // Random Time/Date

    // Pick a random selection from the Arrays.cs file
    internal static T PickRandom<T>(this IEnumerable<T> source) => source.Any() ? source.PickRandom(1).Single() : default;
    internal static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);
    internal static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.OrderBy(_ => Guid.NewGuid());

    // Change Normal("message") to a Normal event log entry. //
    internal static void Settings(string msg) => Game.LogTrivial($"SWLCallouts [SETTINGS]: {msg}");
    // Change Normal("message") to a Normal event log entry. //
    internal static void Normal(string msg) => Game.LogTrivial($"[NORMAL] SWLCallouts: {msg}");
    // Change Log("message") to a Log entry. //
    internal static void Log(string msg) => Game.LogTrivial($"[LOG] SWLCallouts: {msg}");

    // Change Error("message") to a log an error entry. //
#pragma warning disable IDE0060 // Remove unused parameter
    internal static void Error(Exception ex, string location) => Game.LogTrivial($"[ERROR] SWLCallouts: {ex}");
#pragma warning restore IDE0060 // Remove unused parameter

    // Change Subtitle("message", time) to a timed subtitle. //
    internal static void Speech(string msg, int time) => Game.DisplaySubtitle(msg, time);

    // Change Print("message") to a console entry. //
    internal static void Print(string msg)
    {
        Game.Console.Print(msg);
    }

    // Change HelpMsg("message") to a Notification. //
    internal static void HelpMsg(string msg)
    {
        Game.DisplayNotification(msg);
    }

    // Change NotifyP(icon, icon, "title", "subtitle", "message") to a Notification. //
    internal static void NotifyP(string iconLocation, string iconName, string title, string subTitle, string msg)
    {
        Game.DisplayNotification(iconLocation, iconName, title, subTitle, msg);
    }

    /* // DISABLED TILL I FIGURE IT OUT//
    // Change DEPicon to the selected department icon. //
    internal static string DEPicon => Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

    // Change SWLicon to the SWLCO image from github. //
    internal static string SWLicon => "Images/SWLCO.png";
    */ // DISABLED TILL I FIGURE IT OUT //
}

// Thanks for Astro for this idea