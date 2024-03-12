﻿// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.7.2

#region

using LSPD_First_Response.Engine.Scripting.Entities;
using Rage;

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Request for a Welfare Check", CalloutProbability.Medium)]
public class SWLWelfareCheck : Callout
{
    private Ped _suspect;
    private Vector3 _spawnPoint;
    private Vector3 _searcharea;
    private Blip _blip = null;
    private int _storyLine = 1;
    private int _callOutMessage = 0;
    private bool _scene1 = false;
    private bool _scene2 = false;
    private bool _scene3 = false;
    private bool _alreadySubtitleIntrod = false;
    private bool _notificationDisplayed = false;
    private bool _getAmbulance = false;
    private static Persona _suspectPersona;

    public override bool OnBeforeCalloutDisplayed()
    {
        //Random random = Rndm;
        List<Vector3> list = new List<Vector3>
        {
            // City Locations //
            new(917.1311f, -651.3591f, 57.86318f), // Mirror Park 1
            new(1329.527f, -609.7888f, 74.33716f), // Mirror Park 2
            new(-1905.715f, 365.4793f, 93.58082f), // Richman
            new(57.72354f, -1852.521f, 22.84686f), // Davis
            new(386.6045f, -1883.437f, 25.60606f), // Rancho
            new(-1043.986f, -1579.707f, 5.038178f), // La Puerta
            new(15.1229f, 522.7809f, 170.2276f), // Vinewood Hills 1
            new(-1071.873f, 575.5293f, 102.9082f), // Vinewood Hills 2
            new(-161.2484f, -5.143021f, 66.46629f), // West Vinewood
            new(-1884.859f, -600.6564f, 15.54568f), // Pacific Bluffs
            // Blaine County Locations //
            new(1661.571f, 4767.511f, 42.00745f), // Grapeseed
            new(1878.274f, 3922.46f, 33.06999f), // Sandy Shores
            new(-3204.997f, 1206.304f, 12.823f), // Chumash
            new(-2829.186f, 1419.623f, 100.9087f), // Banham Canyon
            new(372.15f, 2628.323f, 44.68521f), // Harmony
            new(15.56086f, 3687.897f, 39.57216f), // Stab City
            // Paleto Bay Locations //
            new(1442.942f, 6333.207f, 23.89725f), // Comm Camp, Mt Chiliad
            new(-4.716893f, 6664.873f, 31.15268f), // East Paleto
            new(-173.9554f, 6421.237f, 30.47764f), // Central Paleto
            new(-466.3274f, 6206.565f, 29.55285f), // West Paleto
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
        _spawnPoint = LocationChooser.ChooseNearestLocation(list);
        if (_suspect.Exists())
        {
            _suspect.Dismiss();
            //GameFiber.Sleep(1000);
            _suspect = new Ped(_spawnPoint, 0f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };
        }
        else {
            _suspect = new Ped(_spawnPoint, 0f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true
            };
        }
        _suspectPersona = Functions.GetPersonaForPed(_suspect);
        switch (Rndm.Next(1, 4))
        {
            case 1:
                _suspect.Kill();
                _scene1 = true;
                break;
            case 2:
                _scene3 = true;
                break;
            case 3:
                _suspect.Dismiss();
                _scene2 = true;
                break;
        }

        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        switch (Rndm.Next(1, 9))
        {
            case 1:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _callOutMessage = 1;
                break;
            case 2:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _callOutMessage = 2;
                break;
            case 3:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _callOutMessage = 3;
                break;
            case 4:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                _suspect.IsPersistent = true;
                _callOutMessage = 4;
                break;
            case 5:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                _suspect.IsPersistent = true;
                _callOutMessage = 5;
                break;
            case 6:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon("WEAPON_PISTOL", 500, true);
                _suspect.IsPersistent = true;
                _callOutMessage = 6;
                break;
            case 7:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                _suspect.IsPersistent = true;
                _callOutMessage = 7;
                break;
            case 8:
                CalloutMessage = "[SWL]~w~ Welfare Check";
                _suspect.Inventory.GiveNewWeapon(WCDispatchArrive.PickRandom(), 500, true);
                _suspect.IsPersistent = true;
                _callOutMessage = 8;
                break;
        }
        CalloutPosition = _spawnPoint;
        Functions.PlayScannerAudioUsingPosition("UNITS WE_HAVE CRIME_CIVILIAN_NEEDING_ASSISTANCE_02", _spawnPoint);
        Log("SWLCallouts - Welfare Check callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Log("SWLCallouts Log: Welfare Check callout accepted.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~Dispatch:~w~ Someone called the police for a welfare check. Search the ~y~yellow area~w~ for the person. Respond ~y~Code 2");
        Functions.PlayScannerAudio("UNITS_RESPOND_CODE_02_02");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~p~*In Car Computer/Tablet*", "Loading ~g~Information~w~ off the ~y~LSPD Database~w~...");
        Functions.DisplayPedId(_suspect, true);

        _searcharea = _spawnPoint.Around2D(1f, 2f);
        _blip = new(_searcharea, 40f)
        {
            Color = Color.Yellow,
            Alpha = 0.5f
        };
        _blip.EnableRoute(Color.Yellow);
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (_suspect) _suspect.Delete();
        if (_blip) _blip.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        GameFiber.StartNew(delegate
        {
            if (_spawnPoint.DistanceTo(GPlayer) < 50f)
            {
                if (_suspect.Exists())
                {
                    if (_scene1 == true && _suspect && _suspect.DistanceTo(GPlayer) < 40f && (!GPlayer.IsInAnyVehicle(false) || GPlayer.IsOnFoot) && !_notificationDisplayed && !_getAmbulance)
                    {
                        if (Settings.HelpMessages)
                        {
                            Game.DisplayHelp("Press the ~y~" + Settings.EndCall + "~w~ key to end the wellfare check callout.");
                        }
                        if (Settings.ActivateAIBackup)
                        {
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", WCDispatchArrive.PickRandom());
                            _notificationDisplayed = true;
                            GameFiber.Wait(1000);
                            Functions.RequestBackup(GPlayer.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.Ambulance);
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE CRIME_AMBULANCE_REQUESTED_03");
                            _getAmbulance = true;
                        }
                        else
                        {
                            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", WCDispatchArrive.PickRandom());
                            _notificationDisplayed = true;
                            GameFiber.Wait(1000);
                            Settings.ActivateAIBackup = false;
                            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                        }
                    }
                    if (_scene2 == true && _spawnPoint.DistanceTo(GPlayer) < 20f && (!GPlayer.IsInAnyVehicle(false) || GPlayer.IsOnFoot) && !_notificationDisplayed)
                    {
                        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", "Investigate the area. If you don't find anyone, you may ~g~End~w~ the call and return to patrol.");
                        _notificationDisplayed = true;
                        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                    }
                    if (_scene3 == true && _suspect && _suspect.DistanceTo(GPlayer) < 40f && (!GPlayer.IsInAnyVehicle(false) || GPlayer.IsOnFoot) && _alreadySubtitleIntrod == false)
                    {
                        Speech("Press ~y~" + Settings.Dialog + "~w~to speak with the civilian.", 5000);
                        if (Settings.HelpMessages)
                        {
                            Game.DisplayHelp("Press the ~y~" + Settings.EndCall + "~w~ key to end the wellfare check callout.", 5000);
                        }
                        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICERS_ARRIVED_ON_SCENE");
                        if (_callOutMessage == 6)
                        {
                            Speech("~y~Civilian: ~w~I'm armed and I'm not afraid to use it!!", 10000);
                            GameFiber.Sleep(1000);
                            Speech("~b~You: ~w~Police! Put your weapon down NOW!", 5000);
                            if (Settings.ActivateAIBackup)
                            {
                                Print("~b~You: ~w~Dispatch, requesting code 3 backup immediately!");
                                GameFiber.Wait(1000);
                                Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH OFFICER_REQUESTING_BACKUP CODE3");
                                Functions.RequestBackup(GPlayer.Position, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                            }
                            else { Settings.ActivateAIBackup = false; }
                            GameFiber.Sleep(2000);
                            Speech("~y~Civilian: ~w~Oh shit, Sorry officer! Here you go!", 5000);
                            // Thanks Astro for the below suggested code //
                            if (_suspect.Inventory.EquippedWeapon is not null && _suspect.Inventory.EquippedWeapon != "WEAPON_UNARMED")
                            {
                                NativeFunction.Natives.x6B7513D9966FBEC0(_suspect); // SET_PED_DROPS_WEAPON
                            }
                            _suspect.Tasks.PutHandsUp(4000, GPlayer);
                            Speech("~y~Civilian: ~w~Can we chat without cuffs being involved?", 5000);
                            GameFiber.Wait(1000);
                        }
                        if (_callOutMessage == 7)
                        {
                            Speech("~y~Civilian: ~w~Fuck you bitch, you're gonna die for trespassing!", 10000);
                            GameFiber.Wait(2000);
                            _suspect.KeepTasks = true;
                            _suspect.Tasks.FightAgainst(GPlayer);
                            Speech("~b~You: ~y~*YELLS* ~w~POLICE, Drop the weapon!", 5000);
                            GameFiber.Wait(2000);
                            Speech("~b~You: ~y~*YELLS* ~w~STOP! POLICE!! GET DOWN ON THE GROUND!", 5000);
                            GameFiber.Sleep(10000);
                            Speech("~y~Civilian: ~w~Oh shit, Sorry officer! I thought you were an intruder!", 5000);
                            _suspect.Tasks.Pause(1000);
                            // Thanks Astro for the below suggested code //
                            if (_suspect.Inventory.EquippedWeapon is not null && _suspect.Inventory.EquippedWeapon != "WEAPON_UNARMED")
                            {
                                NativeFunction.Natives.x6B7513D9966FBEC0(_suspect); // SET_PED_DROPS_WEAPON
                            }
                            _suspect.Tasks.PutHandsUp(8000, GPlayer);
                            GameFiber.Wait(5000);
                            Speech("~b~Civilian: Please officer, don't arrest me. I wasn't thinking and didn't know it was you", 10000);
                            GameFiber.Sleep(2000);
                        }

                        if (_callOutMessage == 8)
                        {
                            Speech("~y~Suspect: ~w~Fuck!! It's the cops!", 10000);
                            GameFiber.Wait(2000);
                            _suspect.KeepTasks = true;
                            _suspect.Tasks.FightAgainst(GPlayer);
                        }

                        _alreadySubtitleIntrod = true;
                    }
                    if (_scene3 == true && _scene1 == false && _scene2 == false && _suspect.DistanceTo(GPlayer) < 5f && Game.IsKeyDown(Settings.Dialog))
                    {
                        _suspect.Face(GPlayer);
                        switch (_storyLine)
                        {
                            case 1:
                                if (_callOutMessage == 1)
                                    Speech("~y~Civilian: ~w~Hello Officer, how can I help you? Is everything alright?", 10000);
                                if (_callOutMessage == 2)
                                    Speech("~y~Civilian: ~w~Hey Officer, can I help you?", 10000);
                                if (_callOutMessage == 3)
                                    Speech("~y~Civilian: ~w~Oh Officer, how can I help you today?", 10000);
                                if (_callOutMessage == 4)
                                    Speech("~y~Civilian: ~w~Yo Officer! Can I help you?!", 10000);
                                if (_callOutMessage == 5)
                                    Speech("~y~Civilian: ~w~Can I help you Officer?", 10000);
                                if (_callOutMessage == 6)
                                    Speech("~y~Civilian: ~w~I'm sorry again officer, let's start again, how can I help you?", 10000);
                                _storyLine++;
                                break;
                            case 2:
                                if (_callOutMessage == 1)
                                    Speech("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you weren't answering your phone and someone is concerned. Is everything okay here?", 10000);
                                if (_callOutMessage == 2)
                                    Speech("~b~You: ~w~Hi. I'm just checking in on this address as we've had a request for a welfare check come through. Apparently you were on the phone with someone and they heard a noise before the line dropped. Is everything all good here?", 10000);
                                if (_callOutMessage == 3)
                                    Speech("~b~You: ~w~Hi. I'm just checking in on this address as we had a welfare check request come through. Apparently no one has heard from you for a while and someone is worried. Is everything fine here?", 10000);
                                if (_callOutMessage == 4)
                                    Speech("~b~You: ~w~Hey there, can we put the knife down please. I'm just here to do a welfare check.", 10000);
                                if (_callOutMessage == 5)
                                    Speech("~b~You: ~w~Hey there, could you please do me a favour and put the knife down, I'm just here to do a welfare check.", 10000);
                                if (_callOutMessage == 5)
                                    Speech("~b~You: ~w~Hey there, could you please do me a favour and put the knife down, I'm just here to do a welfare check.", 10000);
                                if (_callOutMessage == 6)
                                    Speech("~b~You: ~w~We got a request to do a welfare check at this address, being you had a gun, is everything alright?", 10000);
                                _storyLine++;
                                break;
                            case 3:
                                if (_callOutMessage == 1)
                                    Speech("~y~Civilian: ~w~Oh dear! I didn't want to worry anyone.", 10000);
                                if (_callOutMessage == 2)
                                    Speech("~y~Civilian: ~w~Oh, Yes everything is all good.", 10000);
                                if (_callOutMessage == 3)
                                    Speech("~y~Civilian: ~w~Uhh.. Everything is fine officer. I don't understand why they wouldn't call me.", 10000);
                                if (_callOutMessage == 4)
                                {
                                    Speech("~y~Civilian: ~w~Yeah sure, we were just getting food prepared when I thought I heard a noise so I was just checking that out.", 10000);
                                    _suspect.Inventory.GiveNewWeapon("WEAPON_UNARMED", -1, true);
                                }
                                if (_callOutMessage == 5)
                                {
                                    Speech("~b~Civilian: ~w~What did you say? You a little scared are ya?!", 5000);
                                }
                                if (_callOutMessage == 6)
                                {
                                    Speech("~y~Civilian: ~w~Yes, I had the gun because I thought I heard a noise, which it seems like it was just you.", 10000);
                                    GameFiber.Sleep(10000);
                                    Speech("~b~You: ~w~Great, can I get your details and check it with dispatch.", 5000);
                                    GameFiber.Sleep(5000);
                                    Speech("~y~Civilian: ~w~Yes, Here you go.", 10000);
                                    Functions.DisplayPedId(_suspect, false);
                                    GameFiber.Sleep(5000);
                                    Speech("~b~You: ~w~Ok, Can I let dispatch know that you are fine and well?", 5000);
                                    GameFiber.Wait(4000);
                                    // Inside your Process method or wherever you want to display the information
                                    Persona pedPersona = Functions.GetPersonaForPed(_suspect);
                                    string message = String.Format("Dispatch requesting an ID check on {0}, born on {1}.", pedPersona.FullName, pedPersona.Birthday.ToShortDateString());
                                    Speech("~b~You: ~w~" + message, 5000);
                                }
                                _storyLine++;
                                break;
                            case 4:
                                if (_callOutMessage == 1)
                                    Speech("~y~Civilian: ~w~I lost my phone on the bus today, I was actually just about to head to a payphone to ring the bus depot.", 10000);
                                if (_callOutMessage == 2)
                                    Speech("~y~Civilian: ~w~My phone battery died because I forgot to charge it earlier! I put it on charge and then got distracted and forgot to tell them my phone died.", 10000);
                                if (_callOutMessage == 3)
                                    Speech("~y~Civilian: ~w~Let me check my phone... Oops, I had my phone on silent! I'll call them back now. Sorry to cause such trouble!", 10000);
                                if (_callOutMessage == 4)
                                    Speech("~y~Civilian: ~w~So, Why are you here? I don't understand?", 5000);
                                if (_callOutMessage == 5)
                                {
                                    Speech("~y~Civilian: ~w~Fuck off ya pig!", 5000);
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
                                if (_callOutMessage == 6)
                                {
                                    Speech("~y~Civilian: ~w~Yes you can, Sorry again about before officer, you have a wonderful day! Stay safe.", 5000);
                                    GameFiber.Wait(5000);
                                    Speech("~b~You: ~w~No worries, you have yourself a wonderful day too!", 5000);
                                    GameFiber.Wait(5000);
                                    End();
                                }
                                _storyLine++;
                                break;
                            case 5:
                                if (_callOutMessage == 1)
                                    Speech("~b~You: ~w~That's no good. I'll let dispatch know everything is okay. Good luck finding your phone!", 10000);
                                if (_callOutMessage == 2)
                                    Speech("~b~You: ~w~Alright, well as long as everything here is okay, I can leave. You should return that phone call though, the caller was really worried.", 10000);
                                if (_callOutMessage == 3)
                                    Speech("~b~You: ~w~No problem, I'm just glad you're okay. I'll let dispatch know everything is fine here.", 10000);
                                if (_callOutMessage == 4)
                                    Speech("~b~You: ~w~We got a call saying you weren't able to be contacted and someone was worried about you. Upon seeing you here, we can tell you have been busy. So we will leave you to it, have a great day!", 20000);
                                _storyLine++;
                                break;
                            case 6:
                                if (_callOutMessage == 1)
                                    End();
                                if (_callOutMessage == 2)
                                    End();
                                if (_callOutMessage == 3)
                                    End();
                                if (_callOutMessage == 4)
                                    Speech("~y~Civilian: ~w~Yes, I have been busy and didn't take note of the time. We are fine, thanks for the concern.", 10000);
                                End();
                                _storyLine++;
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
        if (_blip.Exists()) _blip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~DISPATCH", "[SWL] ~y~Welfare Check", WCDispatchCode4.PickRandom());
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Log("SWLCallouts - Welfare Check cleanup.");
        base.End();
    }
}