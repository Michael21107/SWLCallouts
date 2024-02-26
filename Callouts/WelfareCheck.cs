// Author: Scottywonderful
// Date: 16th Feb 2024  ||  Last Modified: 21st Feb 2024
// Version: 0.4.0-Alpha

using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using SWLCallouts.Stuff;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("Welfare Check", CalloutProbability.Medium)]
    public class WelfareCheck : Callout
    {
        private Ped subject;
        private string[] Suspects = new string[] { "ig_andreas", "g_m_m_armlieut_01", "a_m_m_bevhills_01", "a_m_y_business_02", "s_m_m_gaffer_01",
                                                   "a_f_y_golfer_01", "a_f_y_bevhills_01", "a_f_y_bevhills_04", "a_f_y_fitness_02"};
        private Vector3 SpawnPoint;
        private Vector3 searcharea;
        private Blip Blip = null;
        private int storyLine = 1;
        private int callOutMessage = 0;
        private bool Scene1 = false;
        private bool Scene2 = false;
        private bool Scene3 = false;
#pragma warning disable CS0414 // Ignores the warning on we get with the next line.
        private bool wasClose = false;
#pragma warning restore CS0414 // Looks for other CS0414 errors outide of here.
        private bool alreadySubtitleIntrod = false;
        private bool notificationDisplayed = false;
        private bool getAmbulance = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            Random random = new Random();
            List<Vector3> list = new List<Vector3>
            {
                new Vector3(917.1311f, -651.3591f, 57.86318f),
                new Vector3(-1905.715f, 365.4793f, 93.58082f),
                new Vector3(1661.571f, 4767.511f, 42.00745f),
                new Vector3(1878.274f, 3922.46f, 33.06999f),
            };
            SpawnPoint = LocationChooser.chooseNearestLocation(list);
            subject = new Ped(Suspects[new Random().Next((int)Suspects.Length)], SpawnPoint, 0f);
            LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(subject);
            switch (new Random().Next(1, 3))
            {
                case 1:
                    subject.Kill();
                    Scene1 = true;
                    break;
                case 2:
                    Scene3 = true;
                    break;
                case 3:
                    subject.Dismiss();
                    Scene2 = true;
                    break;
            }
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
            switch (new Random().Next(1, 3))
            {
                case 1:
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    callOutMessage = 1;
                    break;
                case 2:
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    callOutMessage = 2;
                    break;
                case 3:
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    callOutMessage = 3;
                    break;
            }
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("UNITS WE_HAVE CRIME_CIVILIAN_NEEDING_ASSISTANCE_02", SpawnPoint);

            Game.LogTrivial("SWLCallouts - Welfare Check Offered.");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("SWLCallouts Log: WelfareCheck callout accepted.");
            string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Welfare Check", "~b~Dispatch:~w~ Someone called the police for a welfare check. Search the ~y~yellow area~w~ for the person. Respond ~y~Code 2");
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Loading ~g~Information~w~ of the ~y~LSPD Database~w~...");
            Functions.DisplayPedId(subject, true);

            searcharea = SpawnPoint.Around2D(1f, 2f);
            Blip = new Blip(searcharea, 40f);
            Blip.EnableRoute(Color.Yellow);
            Blip.Color = Color.Yellow;
            Blip.Alpha = 0.5f;

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (subject != null) subject.Delete();
            if (Blip != null) Blip.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs
            GameFiber.StartNew(delegate
            {
                if (SpawnPoint.DistanceTo(Game.LocalPlayer.Character) < 25f)
                {
                    if (Scene1 == true && subject && subject.DistanceTo(Game.LocalPlayer.Character) < 10f && Game.LocalPlayer.Character.IsOnFoot && !notificationDisplayed && !getAmbulance)
                    {
                        Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Dispatch", "We are going to call an ~y~ambulance~w~ to your current location, officer. Press the ~y~END~w~ key to end the welfare check callout.");
                        notificationDisplayed = true;
                        GameFiber.Wait(1000);
                        if (Settings.HelpMessages)
                        {
                            Game.DisplayHelp("Press the ~y~" + Settings.EndCall + "~w~ key to end the wellfare check callout.");
                        }
                        Functions.RequestBackup(Game.LocalPlayer.Character.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.Ambulance);
                        getAmbulance = true;
                    }
                    if (Scene2 == true && SpawnPoint.DistanceTo(Game.LocalPlayer.Character) < 8f && Game.LocalPlayer.Character.IsOnFoot && !notificationDisplayed)
                    {
                        Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Dispatch", "Investigate the area. If you don't find anyone here, then ~g~End~w~ the callout.");
                        notificationDisplayed = true;
                    }
                    if (Scene3 == true && subject && subject.DistanceTo(Game.LocalPlayer.Character) < 25f && Game.LocalPlayer.Character.IsOnFoot && alreadySubtitleIntrod == false)
                    {
                        Game.DisplaySubtitle("Press ~y~Y ~w~to speak with the suspect.", 5000);
                        Game.DisplayHelp("Press the ~y~END~w~ key to end the ~o~welfare check~w~ callout.", 5000);
                        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                        alreadySubtitleIntrod = true;
                        wasClose = true;
                    }
                    if (Scene3 == true && Scene1 == false && Scene2 == false && subject.DistanceTo(Game.LocalPlayer.Character) < 2f && Game.IsKeyDown(Settings.Dialog))
                    {
                        subject.Face(Game.LocalPlayer.Character);
                        switch (storyLine)
                        {
                            case 1:
                                Game.DisplaySubtitle("~y~Suspect: ~w~Hello Officer, how can I help you? Is everything alright? (1/5)", 5000);
                                storyLine++;
                                break;
                            case 2:
                                Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you weren't answering your phone and someone is concerned. Is everything okay here? (2/5)", 5000);
                                storyLine++;
                                break;
                            case 3:
                                Game.DisplaySubtitle("~y~Suspect: ~w~Oh dear! I didn't want to worry anyone. (3/5)", 5000);
                                storyLine++;
                                break;
                            case 4:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~y~Suspect: ~w~I lost my phone on the bus today, I was actually just about to head to a payphone to ring the bus depot. (4/5)", 5000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~y~Suspect: ~w~My phone battery died because I forgot to charge it earlier! I did see a missed call but didn't think anything of it. (4/5)", 5000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~y~Suspect: ~w~Let me check... Oops, I had my phone on silent! I'll call them back now. Sorry to cause such trouble! (4/5)", 5000);
                                storyLine++;
                                break;
                            case 5:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~b~You: ~w~Ouch. I'll let dispatch know everything is okay. Good luck finding your phone! (5/5)", 5000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~b~You: ~w~Alright, well as long as everything here is okay, I can leave. You should return that phone call though, the caller was really worried. (5/5)", 5000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~b~You: ~w~No problem, I'm just glad you're okay. I'll let dispatch know everything is fine here. (5/5)", 5000);
                                storyLine++;
                                break;
                            case 6:
                                if (callOutMessage == 1)
                                    End();
                                if (callOutMessage == 2)
                                    End();
                                if (callOutMessage == 3)
                                    End();
                                storyLine++;
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (Game.LocalPlayer.Character.IsDead) End();
            }, "Welfare Check [SWLCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (subject.Exists()) subject.Dismiss();
            if (Blip != null && Blip.Exists()) Blip.Delete();
            string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Welfare Check", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("SWLCallouts - Welfare Check Cleanup.");
            base.End();
        }
    }
}