// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.6.0

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Request for a Welfare Check", CalloutProbability.Medium)]
public class SWLWelfareCheck : Callout
{
    private Ped _suspect;
    private Vector3 SpawnPoint;
    private Vector3 searcharea;
    private Blip Blip = null;
    private int storyLine = 1;
    private int callOutMessage = 0;
    private bool Scene1 = false;
    private bool Scene2 = false;
    private bool Scene3 = false;
    private bool alreadySubtitleIntrod = false;
    private bool notificationDisplayed = false;
    private bool getAmbulance = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        Random random = new();
        List<Vector3> list = new()
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
        foreach (Vector3 location in list.ToList())
        {
            if (GPlayer.Position.DistanceTo(location) < 80f)
            {
                list.Remove(location); // Remove locations within the distance threshold
            }
        }

        // Choose the nearest location from the updated list
        SpawnPoint = LocationChooser.ChooseNearestLocation(list);
        if (_suspect.Exists())
        {
            _suspect.Dismiss();
            //GameFiber.Sleep(1000);
            _suspect = new Ped(SpawnPoint, 0f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };
        }
        else {
            _suspect = new Ped(SpawnPoint, 0f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };
        }
        LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(_suspect);
        switch (random.Next(1, 4))
        {
            case 1:
                _suspect.Kill();
                Scene1 = true;
                break;
            case 2:
                Scene3 = true;
                break;
            case 3:
                _suspect.Dismiss();
                Scene2 = true;
                break;
        }
        ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
        switch (random.Next(1, 6))
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
            case 4:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                _suspect.IsPersistent = true;
                callOutMessage = 4;
                break;
            case 5:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                _suspect.IsPersistent = true;
                callOutMessage = 5;
                break;
        }
        CalloutPosition = SpawnPoint;
        Functions.PlayScannerAudioUsingPosition("UNITS WE_HAVE CRIME_CIVILIAN_NEEDING_ASSISTANCE_02", SpawnPoint);
        Game.LogTrivial("SWLCallouts - Welfare Check callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Game.LogTrivial("SWLCallouts Log: Welfare Check callout accepted.");
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~Dispatch:~w~ Someone called the police for a welfare check. Search the ~y~yellow area~w~ for the person. Respond ~y~Code 2");
        Functions.PlayScannerAudio("UNITS_RESPOND_CODE_02_02");
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "", "Loading ~g~Information~w~ off the ~y~LSPD Database~w~...");
        Functions.DisplayPedId(_suspect, true);

        searcharea = SpawnPoint.Around2D(1f, 2f);
        Blip = new Blip(searcharea, 40f);
        Blip.EnableRoute(Color.Yellow);
        Blip.Color = Color.Yellow;
        Blip.Alpha = 0.5f;

        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (_suspect) _suspect.Delete();
        if (Blip) Blip.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        GameFiber.StartNew(delegate
        {
            if (SpawnPoint.DistanceTo(GPlayer) < 50f)
            {
                if (_suspect.Exists())
                {
                    if (Scene1 == true && _suspect && _suspect.DistanceTo(GPlayer) < 25f && (!GPlayer.IsInAnyVehicle(false) || GPlayer.IsOnFoot) && !notificationDisplayed && !getAmbulance)

                    {
                        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Dispatch", "We are going to call an ~y~ambulance~w~ to your current location, officer. Press the ~y~END~w~ key to end the welfare check callout.");
                        notificationDisplayed = true;
                        GameFiber.Wait(1000);
                        if (Settings.HelpMessages)
                        {
                            Game.DisplayHelp("Press the ~y~" + Settings.EndCall + "~w~ key to end the wellfare check callout.");
                        }
                        if (Settings.ActivateAIBackup)
                        {
                            Functions.RequestBackup(GPlayer.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.Ambulance);
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE CRIME_AMBULANCE_REQUESTED_03");
                        }
                        else {
                            Settings.ActivateAIBackup = false;
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                        }
                        getAmbulance = true;
                    }
                    if (Scene2 == true && SpawnPoint.DistanceTo(GPlayer) < 20f && (!GPlayer.IsInAnyVehicle(false) || GPlayer.IsOnFoot) && !notificationDisplayed)
                    {
                        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Dispatch", "Investigate the area. If you don't find anyone here, then ~g~End~w~ the callout.");
                        notificationDisplayed = true;
                        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                    }
                    if (Scene3 == true && _suspect && _suspect.DistanceTo(GPlayer) < 25f && (!GPlayer.IsInAnyVehicle(false) || GPlayer.IsOnFoot) && alreadySubtitleIntrod == false)
                    {
                        Game.DisplaySubtitle("Press ~y~Y ~w~to speak with the civilian.", 5000);
                        Game.DisplayHelp("Press the ~y~END~w~ key to end the ~o~welfare check~w~ callout.", 5000);
                        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                        alreadySubtitleIntrod = true;
                    }
                    if (Scene3 == true && Scene1 == false && Scene2 == false && _suspect.DistanceTo(GPlayer) < 5f && Game.IsKeyDown(Settings.Dialog))
                    {
                        _suspect.Face(GPlayer);
                        switch (storyLine)
                        {
                            case 1:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Hello Officer, how can I help you? Is everything alright?", 10000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Hey Officer, can I help you?", 10000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Oh Officer, how can I help you today?", 10000);
                                if (callOutMessage == 4)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Yo Officer! Can I help you?!", 10000);
                                if (callOutMessage == 5)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Can I help you Officer?", 10000);
                                storyLine++;
                                break;
                            case 2:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you weren't answering your phone and someone is concerned. Is everything okay here?", 10000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you were on the phone with someone and they heard a noise before the line dropped. Is everything all good here?", 10000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~b~You: ~w~Hi. I'm just checking in on this address as we had a welfare check request come through. Apparently no one has heard from you for a while and someone is worried. Is everything fine here?", 10000);
                                if (callOutMessage == 4)
                                    Game.DisplaySubtitle("~b~You: ~w~Hey there, can we put the knife down please. I'm just here to do a welfare check.", 10000);
                                if (callOutMessage == 5)
                                    Game.DisplaySubtitle("~b~You: ~w~Hey there, could you please do me a favour and put the knife down, I'm just here to do a welfare check.", 10000);
                                storyLine++;
                                break;
                            case 3:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Oh dear! I didn't want to worry anyone.", 10000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Oh, Yes everything is all good.", 10000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Uhh.. Everything is fine officer. I don't understand why they wouldn't call me.", 10000);
                                if (callOutMessage == 4)
                                {
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Yeah sure, we were just getting food prepared when I thought I heard a noise so I was just checking that out.", 10000);
                                    _suspect.Inventory.GiveNewWeapon("WEAPON_UNARMED", -1, true);
                                }
                                if (callOutMessage == 5)
                                {
                                    Game.DisplaySubtitle("~b~Civilian: ~w~What did you say? You a little scared are ya?!", 5000);
                                }
                                storyLine++;
                                break;
                            case 4:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~I lost my phone on the bus today, I was actually just about to head to a payphone to ring the bus depot.", 10000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~My phone battery died because I forgot to charge it earlier! I put it on charge and then got distracted and forgot to tell them my phone died.", 10000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Let me check my phone... Oops, I had my phone on silent! I'll call them back now. Sorry to cause such trouble!", 10000);
                                if (callOutMessage == 4)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~So, Why are you here? I don't understand?", 5000);
                                if (callOutMessage == 5)
                                {
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Fuck off ya pig!", 5000);
                                    if (Settings.ActivateAIBackup)
                                    {
                                        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICER_REQUESTING_BACKUP CODE3");
                                        Functions.RequestBackup(GPlayer.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                                    }
                                    else { Settings.ActivateAIBackup = false; }
                                    GameFiber.Wait(2000);
                                    _suspect.KeepTasks = true;
                                    _suspect.Tasks.FightAgainst(GPlayer);
                                }
                                storyLine++;
                                break;
                            case 5:
                                if (callOutMessage == 1)
                                    Game.DisplaySubtitle("~b~You: ~w~That's no good. I'll let dispatch know everything is okay. Good luck finding your phone!", 10000);
                                if (callOutMessage == 2)
                                    Game.DisplaySubtitle("~b~You: ~w~Alright, well as long as everything here is okay, I can leave. You should return that phone call though, the caller was really worried.", 10000);
                                if (callOutMessage == 3)
                                    Game.DisplaySubtitle("~b~You: ~w~No problem, I'm just glad you're okay. I'll let dispatch know everything is fine here.", 10000);
                                if (callOutMessage == 4)
                                    Game.DisplaySubtitle("~b~You: ~w~We got a call saying you weren't able to be contacted and someone was worried about you. Upon seeing you here, we can tell you have been busy. So we will leave you to it, have a great day!", 20000);
                                storyLine++;
                                break;
                            case 6:
                                if (callOutMessage == 1)
                                    End();
                                if (callOutMessage == 2)
                                    End();
                                if (callOutMessage == 3)
                                    End();
                                if (callOutMessage == 4)
                                    Game.DisplaySubtitle("~y~Civilian: ~w~Yes, I have been busy and didn't take note of the time. We are fine, thanks for the concern.", 10000);
                                End();
                                storyLine++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            if (Game.IsKeyDown(Settings.EndCall)) End();
            if (GPlayer.IsDead) End();
        }, "Welfare Check [SWLCallouts]");
        base.Process();
    }

    public override void End()
    {
        if (_suspect.Exists()) _suspect.Dismiss();
        if (Blip.Exists()) Blip.Delete();
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Game.LogTrivial("SWLCallouts - Welfare Check cleanup.");
        base.End();
    }
}