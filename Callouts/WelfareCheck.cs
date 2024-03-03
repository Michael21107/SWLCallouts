// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.5.4

using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using SWLCallouts.Stuff;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("[SWL] Request for a Welfare Check", CalloutProbability.Medium)]
    public class SWLWelfareCheck : Callout
    {
        private Ped Suspect;
        private Vector3 SpawnPoint;
        private Vector3 searcharea;
        private Blip Blip = null;
        private int storyLine = 1;
        private int callOutMessage = 0;
        private bool Scene1 = false;
        private bool Scene2 = false;
        private bool Scene3 = false;
#pragma warning disable CS0414
        private bool hasBegunAttacking = false;
        private bool isArmed = false;
#pragma warning restore CS0414
        private bool alreadySubtitleIntrod = false;
        private bool notificationDisplayed = false;
        private bool getAmbulance = false;
        string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

        public override bool OnBeforeCalloutDisplayed()
        {
            Game.LogTrivial("SWLCallouts-WC Action 100");
            Random random = new Random();
            Game.LogTrivial("SWLCallouts-WC Action 101");
            List<Vector3> list = new List<Vector3>
            {
                // City Locations //
                new Vector3(917.1311f, -651.3591f, 57.86318f), // Mirror Park 1
                new Vector3(1329.527f, -609.7888f, 74.33716f), // Mirror Park 2
                new Vector3(-1905.715f, 365.4793f, 93.58082f), // Richman
                new Vector3(57.72354f, -1852.521f, 22.84686f), // Davis
                new Vector3(386.6045f, -1883.437f, 25.60606f), // Rancho
                new Vector3(-1043.986f, -1579.707f, 5.038178f), // La Puerta
                new Vector3(15.1229f, 522.7809f, 170.2276f), // Vinewood Hills 1
                new Vector3(-1071.873f, 575.5293f, 102.9082f), // Vinewood Hills 2
                new Vector3(-161.2484f, -5.143021f, 66.46629f), // West Vinewood
                new Vector3(-1884.859f, -600.6564f, 15.54568f), // Pacific Bluffs
                // Blaine County Locations //
                new Vector3(1661.571f, 4767.511f, 42.00745f), // Grapeseed
                new Vector3(1878.274f, 3922.46f, 33.06999f), // Sandy Shores
                new Vector3(-3204.997f, 1206.304f, 12.823f), // Chumash
                new Vector3(-2829.186f, 1419.623f, 100.9087f), // Banham Canyon
                new Vector3(372.15f, 2628.323f, 44.68521f), // Harmony
                new Vector3(15.56086f, 3687.897f, 39.57216f), // Stab City
                // Paleto Bay Locations //
                new Vector3(1442.942f, 6333.207f, 23.89725f), // Comm Camp, Mt Chiliad
                new Vector3(-4.716893f, 6664.873f, 31.15268f), // East Paleto
                new Vector3(-173.9554f, 6421.237f, 30.47764f), // Central Paleto
                new Vector3(-466.3274f, 6206.565f, 29.55285f), // West Paleto
            };

            // Find the nearest location that is not within the distance threshold
            Game.LogTrivial("SWLCallouts-WC Action 102");
            foreach (Vector3 location in list.ToList())
            {
                Game.LogTrivial("SWLCallouts-WC Action 103");
                if (Game.LocalPlayer.Character.Position.DistanceTo(location) < 80f)
                {
                    Game.LogTrivial("SWLCallouts-WC Action 104");
                    list.Remove(location); // Remove locations within the distance threshold
                    Game.LogTrivial("SWLCallouts-WC Action 105");
                }
                Game.LogTrivial("SWLCallouts-WC Action 106");
            }

            // Choose the nearest location from the updated list
            Game.LogTrivial("SWLCallouts-WC Action 107");
            SpawnPoint = LocationChooser.chooseNearestLocation(list);
            Game.LogTrivial("SWLCallouts-WC Action 108");
            Suspect = new Ped(SpawnPoint, 0f);
            Game.LogTrivial("SWLCallouts-WC Action 109");
            Suspect.IsPersistent = true;
            Game.LogTrivial("SWLCallouts-WC Action 110");
            Suspect.BlockPermanentEvents = true;
            Game.LogTrivial("SWLCallouts-WC Action 111");
            LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(Suspect);
            Game.LogTrivial("SWLCallouts-WC Action 112");
            switch (random.Next(1, 4))
            {
                case 1:
                    Game.LogTrivial("SWLCallouts-WC Action 113");
                    Suspect.Kill();
                    Scene1 = true;
                    break;
                case 2:
                    Game.LogTrivial("SWLCallouts-WC Action 114");
                    Scene3 = true;
                    break;
                case 3:
                    Game.LogTrivial("SWLCallouts-WC Action 115");
                    Suspect.Dismiss();
                    Scene2 = true;
                    break;
            }
            Game.LogTrivial("SWLCallouts-WC Action 116");
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
            Game.LogTrivial("SWLCallouts-WC Action 117");
            switch (random.Next(1, 6))
            {
                case 1:
                    Game.LogTrivial("SWLCallouts-WC Action 118");
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    Game.LogTrivial("SWLCallouts-WC Action 119");
                    callOutMessage = 1;
                    break;
                case 2:
                    Game.LogTrivial("SWLCallouts-WC Action 120");
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    Game.LogTrivial("SWLCallouts-WC Action 121");
                    callOutMessage = 2;
                    break;
                case 3:
                    Game.LogTrivial("SWLCallouts-WC Action 122");
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    Game.LogTrivial("SWLCallouts-WC Action 123");
                    callOutMessage = 3;
                    break;
                case 4:
                    Game.LogTrivial("SWLCallouts-WC Action 124");
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    Game.LogTrivial("SWLCallouts-WC Action 125");
                    Suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                    Game.LogTrivial("SWLCallouts-WC Action 126");
                    isArmed = true;
                    Game.LogTrivial("SWLCallouts-WC Action 127");
                    Suspect.IsPersistent = true;
                    Game.LogTrivial("SWLCallouts-WC Action 128");
                    callOutMessage = 4;
                    break;
                case 5:
                    Game.LogTrivial("SWLCallouts-WC Action 129");
                    CalloutMessage = "[SWL]~w~ Welfare Check";
                    Game.LogTrivial("SWLCallouts-WC Action 130");
                    Suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                    Game.LogTrivial("SWLCallouts-WC Action 131");
                    isArmed = true;
                    Game.LogTrivial("SWLCallouts-WC Action 132");
                    Suspect.IsPersistent = true;
                    Game.LogTrivial("SWLCallouts-WC Action 133");
                    callOutMessage = 5;
                    break;
            }
            Game.LogTrivial("SWLCallouts-WC Action 134");
            CalloutPosition = SpawnPoint;
            Game.LogTrivial("SWLCallouts-WC Action 135");
            Functions.PlayScannerAudioUsingPosition("UNITS WE_HAVE CRIME_CIVILIAN_NEEDING_ASSISTANCE_02", SpawnPoint);
            Game.LogTrivial("SWLCallouts-WC Action 136");
            Game.LogTrivial("SWLCallouts - Welfare Check Offered.");
            Game.LogTrivial("SWLCallouts-WC Action 137");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("SWLCallouts-WC Action 200");
            Game.LogTrivial("SWLCallouts Log: Welfare Check callout accepted.");
            Game.LogTrivial("SWLCallouts-WC Action 201");
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~Dispatch:~w~ Someone called the police for a welfare check. Search the ~y~yellow area~w~ for the person. Respond ~y~Code 2");
            Game.LogTrivial("SWLCallouts-WC Action 202");
            Functions.PlayScannerAudio("UNITS_RESPOND_CODE_02_02");
            Game.LogTrivial("SWLCallouts-WC Action 203");
            //Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_HAVE_01 CITIZENS_REPORT_01 A_01 CRIME_CIVILIAN_NEEDING_ASSISTANCE_02 UNITS_RESPOND_CODE_02_02");
            Game.LogTrivial("SWLCallouts-WC Action 204");
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "", "Loading ~g~Information~w~ off the ~y~LSPD Database~w~...");
            Game.LogTrivial("SWLCallouts-WC Action 205");
            Functions.DisplayPedId(Suspect, true);
            Game.LogTrivial("SWLCallouts-WC Action 206");

            searcharea = SpawnPoint.Around2D(1f, 2f);
            Game.LogTrivial("SWLCallouts-WC Action 207");
            Blip = new Blip(searcharea, 40f);
            Game.LogTrivial("SWLCallouts-WC Action 208");
            Blip.EnableRoute(Color.Yellow);
            Game.LogTrivial("SWLCallouts-WC Action 209");
            Blip.Color = Color.Yellow;
            Game.LogTrivial("SWLCallouts-WC Action 210");
            Blip.Alpha = 0.5f;
            Game.LogTrivial("SWLCallouts-WC Action 211");

            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            Game.LogTrivial("SWLCallouts-WC Action 300");
            if (Suspect) Suspect.Delete();
            Game.LogTrivial("SWLCallouts-WC Action 301");
            if (Blip) Blip.Delete();
            Game.LogTrivial("SWLCallouts-WC Action 302");
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            Game.LogTrivial("SWLCallouts-WC Action 400");
            GameFiber.StartNew(delegate
            {
                Game.LogTrivial("SWLCallouts-WC Action 401");
                if (SpawnPoint.DistanceTo(Game.LocalPlayer.Character) < 50f)
                {
                    Game.LogTrivial("SWLCallouts-WC Action 402");
                    if (Suspect != null && Suspect.Exists())
                    {
                        Game.LogTrivial("SWLCallouts-WC Action 403");
                        if (Scene1 == true && Suspect && Suspect.DistanceTo(Game.LocalPlayer.Character) < 20f && Game.LocalPlayer.Character.IsOnFoot && !notificationDisplayed && !getAmbulance)
                        {
                            Game.LogTrivial("SWLCallouts-WC Action 404");
                            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Dispatch", "We are going to call an ~y~ambulance~w~ to your current location, officer. Press the ~y~END~w~ key to end the welfare check callout.");
                            Game.LogTrivial("SWLCallouts-WC Action 405");
                            notificationDisplayed = true;
                            Game.LogTrivial("SWLCallouts-WC Action 406");
                            GameFiber.Wait(1000);
                            Game.LogTrivial("SWLCallouts-WC Action 407");
                            if (Settings.HelpMessages)
                            {
                                Game.LogTrivial("SWLCallouts-WC Action 408");
                                Game.DisplayHelp("Press the ~y~" + Settings.EndCall + "~w~ key to end the wellfare check callout.");
                                Game.LogTrivial("SWLCallouts-WC Action 409");
                            }
                            Game.LogTrivial("SWLCallouts-WC Action 410");
                            Functions.RequestBackup(Game.LocalPlayer.Character.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.Ambulance);
                            Game.LogTrivial("SWLCallouts-WC Action 411");
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE CRIME_AMBULANCE_REQUESTED_03");
                            Game.LogTrivial("SWLCallouts-WC Action 412");
                            getAmbulance = true;
                        }
                        Game.LogTrivial("SWLCallouts-WC Action 413");
                        if (Scene2 == true && SpawnPoint.DistanceTo(Game.LocalPlayer.Character) < 10f && Game.LocalPlayer.Character.IsOnFoot && !notificationDisplayed)
                        {
                            Game.LogTrivial("SWLCallouts-WC Action 414");
                            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Dispatch", "Investigate the area. If you don't find anyone here, then ~g~End~w~ the callout.");
                            Game.LogTrivial("SWLCallouts-WC Action 415");
                            notificationDisplayed = true;
                            Game.LogTrivial("SWLCallouts-WC Action 416");
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                            Game.LogTrivial("SWLCallouts-WC Action 417");
                        }
                        Game.LogTrivial("SWLCallouts-WC Action 418");
                        if (Scene3 == true && Suspect && Suspect.DistanceTo(Game.LocalPlayer.Character) < 25f && Game.LocalPlayer.Character.IsOnFoot && alreadySubtitleIntrod == false)
                        {
                            Game.LogTrivial("SWLCallouts-WC Action 419");
                            Game.DisplaySubtitle("Press ~y~Y ~w~to speak with the civilian.", 5000);
                            Game.LogTrivial("SWLCallouts-WC Action 420");
                            Game.DisplayHelp("Press the ~y~END~w~ key to end the ~o~welfare check~w~ callout.", 5000);
                            Game.LogTrivial("SWLCallouts-WC Action 421");
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                            Game.LogTrivial("SWLCallouts-WC Action 422");
                            alreadySubtitleIntrod = true;
                        }
                        Game.LogTrivial("SWLCallouts-WC Action 423");
                        if (Scene3 == true && Scene1 == false && Scene2 == false && Suspect.DistanceTo(Game.LocalPlayer.Character) < 5f && Game.IsKeyDown(Settings.Dialog))
                        {
                            Game.LogTrivial("SWLCallouts-WC Action 424");
                            Suspect.Face(Game.LocalPlayer.Character);
                            Game.LogTrivial("SWLCallouts-WC Action 425");
                            switch (storyLine)
                            {
                                case 1:
                                    Game.LogTrivial("SWLCallouts-WC Action 426");
                                    if (callOutMessage == 1)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Hello Officer, how can I help you? Is everything alright?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 427");
                                    if (callOutMessage == 2)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Hey Officer, can I help you?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 428");
                                    if (callOutMessage == 3)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Oh Officer, how can I help you today?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 429");
                                    if (callOutMessage == 4)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Yo Officer! Can I help you?!", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 430");
                                    if (callOutMessage == 5)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Can I help you Officer?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 431");
                                    storyLine++;
                                    break;
                                case 2:
                                    Game.LogTrivial("SWLCallouts-WC Action 432");
                                    if (callOutMessage == 1)
                                        Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you weren't answering your phone and someone is concerned. Is everything okay here?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 433");
                                    if (callOutMessage == 2)
                                        Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you were on the phone with someone and they heard a noise before the line dropped. Is everything all good here?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 434");
                                    if (callOutMessage == 3)
                                        Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we had a welfare check request come through. Apparently no one has heard from you for a while and someone is worried. Is everything fine here?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 435");
                                    if (callOutMessage == 4)
                                        Game.DisplaySubtitle("~b~You: ~w~Hey there, can we put the knife down please. I'm just here to do a welfare check.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 436");
                                    if (callOutMessage == 5)
                                        Game.DisplaySubtitle("~b~You: ~w~Hey there, could you please do me a favour and put the knife down, I'm just here to do a welfare check.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 437");
                                    storyLine++;
                                    break;
                                case 3:
                                    Game.LogTrivial("SWLCallouts-WC Action 438");
                                    if (callOutMessage == 1)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Oh dear! I didn't want to worry anyone.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 439");
                                    if (callOutMessage == 2)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Oh, Yes everything is all good.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 440");
                                    if (callOutMessage == 3)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Uhh.. Everything is fine officer. I don't understand why they wouldn't call me.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 441");
                                    if (callOutMessage == 4)
                                    {
                                        Game.LogTrivial("SWLCallouts-WC Action 442");
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Yeah sure, we were just getting food prepared when I thought I heard a noise so I was just checking that out.", 10000);
                                        Game.LogTrivial("SWLCallouts-WC Action 443");
                                        Suspect.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                                        Game.LogTrivial("SWLCallouts-WC Action 444");
                                        isArmed = false;
                                        Game.LogTrivial("SWLCallouts-WC Action 445");
                                    }
                                    Game.LogTrivial("SWLCallouts-WC Action 446");
                                    if (callOutMessage == 5)
                                    {
                                        Game.LogTrivial("SWLCallouts-WC Action 447");
                                        if (Settings.ActivateAIBackup)
                                        {
                                            Game.LogTrivial("SWLCallouts-WC Action 448");
                                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICER_REQUESTING_BACKUP CODE3");
                                            Game.LogTrivial("SWLCallouts-WC Action 449");
                                            Functions.RequestBackup(Game.LocalPlayer.Character.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                                            Game.LogTrivial("SWLCallouts-WC Action 450");
                                        }
                                        else
                                        {
                                            Game.LogTrivial("SWLCallouts-WC Action 451");
                                            Settings.ActivateAIBackup = false;
                                        }
                                        Game.LogTrivial("SWLCallouts-WC Action 452");
                                        Game.DisplaySubtitle("~b~Civilian: ~w~What did you say? You a little scared are ya?!", 10000);
                                        Game.LogTrivial("SWLCallouts-WC Action 453");
                                        Suspect.Tasks.Wander();
                                        Game.LogTrivial("SWLCallouts-WC Action 454");
                                    }
                                    storyLine++;
                                    break;
                                case 4:
                                    Game.LogTrivial("SWLCallouts-WC Action 455");
                                    if (callOutMessage == 1)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~I lost my phone on the bus today, I was actually just about to head to a payphone to ring the bus depot.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 456");
                                    if (callOutMessage == 2)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~My phone battery died because I forgot to charge it earlier! I put it on charge and then got distracted and forgot to tell them my phone died.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 457");
                                    if (callOutMessage == 3)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Let me check my phone... Oops, I had my phone on silent! I'll call them back now. Sorry to cause such trouble!", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 458");
                                    if (callOutMessage == 4)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~So, Why are you here? I don't understand?", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 459");
                                    if (callOutMessage == 5)
                                    {
                                        Game.LogTrivial("SWLCallouts-WC Action 460");
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Fuck off ya pig!", 5000);
                                        Game.LogTrivial("SWLCallouts-WC Action 461");
                                        isArmed = true;
                                        Game.LogTrivial("SWLCallouts-WC Action 462");
                                        Suspect.KeepTasks = true;
                                        Game.LogTrivial("SWLCallouts-WC Action 463");
                                        Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                                        Game.LogTrivial("SWLCallouts-WC Action 464");
                                        hasBegunAttacking = true;
                                        Game.LogTrivial("SWLCallouts-WC Action 465");
                                    }
                                    storyLine++;
                                    break;
                                case 5:
                                    Game.LogTrivial("SWLCallouts-WC Action 466");
                                    if (callOutMessage == 1)
                                        Game.DisplaySubtitle("~b~You: ~w~That's no good. I'll let dispatch know everything is okay. Good luck finding your phone!", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 467");
                                    if (callOutMessage == 2)
                                        Game.DisplaySubtitle("~b~You: ~w~Alright, well as long as everything here is okay, I can leave. You should return that phone call though, the caller was really worried.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 468");
                                    if (callOutMessage == 3)
                                        Game.DisplaySubtitle("~b~You: ~w~No problem, I'm just glad you're okay. I'll let dispatch know everything is fine here.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 469");
                                    if (callOutMessage == 4)
                                        Game.DisplaySubtitle("~b~You: ~w~We got a call saying you weren't able to be contacted and someone was worried about you. Upon seeing you here, we can tell you have been busy. So we will leave you to it, have a great day!", 20000);
                                    Game.LogTrivial("SWLCallouts-WC Action 470");
                                    storyLine++;
                                    break;
                                case 6:
                                    Game.LogTrivial("SWLCallouts-WC Action 471");
                                    if (callOutMessage == 1)
                                        End();
                                    Game.LogTrivial("SWLCallouts-WC Action 472");
                                    if (callOutMessage == 2)
                                        End();
                                    Game.LogTrivial("SWLCallouts-WC Action 473");
                                    if (callOutMessage == 3)
                                        End();
                                    Game.LogTrivial("SWLCallouts-WC Action 474");
                                    if (callOutMessage == 4)
                                        Game.DisplaySubtitle("~y~Civilian: ~w~Yes, I have been busy and didn't take note of the time. We are fine, thanks for the concern.", 10000);
                                    Game.LogTrivial("SWLCallouts-WC Action 475");
                                    End();
                                    Game.LogTrivial("SWLCallouts-WC Action 476");
                                    storyLine++;
                                    break;
                                default:
                                    Game.LogTrivial("SWLCallouts-WC Action 477");
                                    break;
                            }
                            Game.LogTrivial("SWLCallouts-WC Action 478");
                        }
                    }
                }
                if (Game.IsKeyDown(Settings.EndCall)) End();
                Game.LogTrivial("SWLCallouts-WC Action 479");
                if (Game.LocalPlayer.Character.IsDead) End();
                Game.LogTrivial("SWLCallouts-WC Action 480");
            }, "Welfare Check [SWLCallouts]");
            base.Process();
        }

        public override void End()
        {
            Game.LogTrivial("SWLCallouts-WC Action 500");
            if (Suspect.Exists()) Suspect.Dismiss();
            Game.LogTrivial("SWLCallouts-WC Action 501");
            if (Blip.Exists()) Blip.Delete();
            Game.LogTrivial("SWLCallouts-WC Action 502");
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Game.LogTrivial("SWLCallouts-WC Action 503");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
            Game.LogTrivial("SWLCallouts-WC Action 504");

            Game.LogTrivial("SWLCallouts - Welfare Check Cleanup.");
            Game.LogTrivial("SWLCallouts-WC Action 505");
            base.End();
        }
    }
}