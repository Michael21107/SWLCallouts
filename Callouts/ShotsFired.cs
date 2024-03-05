// Author: Scottywonderful
// Created: 2nd Mar 2024
// Version: 0.4.5.6

using System;
using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using System.Collections.Generic;
using SWLCallouts.Stuff;
using System.Linq;
using System.Diagnostics.Eventing.Reader;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("[SWL] Reports of Shots Fired", CalloutProbability.Medium)]
    public class SWLShotsFired : Callout
    {
        private readonly string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_REVOLVER", "WEAPON_DOUBLEACTION", "WEAPON_CERAMICPISTOL",/*/ <<Pistols || Rifles>> /*/ "WEAPON_MIRCOSMG", "WEAPON_SMG", "WEAPON_TECPISTOL", "WEAPON_ASSAULTRIFLE", "WEAPON_BULLPUPRIFLE", "WEAPON_COMPACTRIFLE", "WEAPON_SAWNOFFSHOTGUN"};
        private Ped Suspect1;
        private Ped Suspect2;
        private Ped Ped1;
        private Ped Ped2;
        private Ped Ped3;
        private Vector3 SpawnPoint;
        private Vector3 searcharea;
        private Blip Blip;
        private int callOutScene = 0;
        private int scenario = 0;
        private bool hasBegunAttacking = false;
        private bool isArmed = false;
        private bool hasPursuitBegun = false;
        readonly string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

        public override bool OnBeforeCalloutDisplayed()
        {
            #pragma warning disable IDE0059 // Ignores the warning on we get with the next line.
            Random random = new Random();
            #pragma warning restore IDE0059 // Looks for other CS0414 errors outide of here.
            List<Vector3> list = new List<Vector3>
            {
                // City Locations //
                new Vector3(-1622.711f, 214.8514f, 60.22071f), // Richman Uni
                new Vector3(295.0424f, -578.2471f, 43.18422f), // Pillbox Hill Med
                new Vector3(-1573.039f, -1169.825f, 2.402837f), // Del Pero Pier Beach
                new Vector3(-1323.908f, 50.76834f, 53.53567f), // Golfing Society
                new Vector3(1155.258f, -741.4567f, 57.30391f), // Mirror Park
                new Vector3(291.6201f, 179.956f, 104.297f), // Downtown Vinewood
                new Vector3(39.61766f, -1743.935f, 29.30354f), // Davis
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
                // Blaine County Locations //
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
                // Paleto Bay Locations //
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
                //new Vector3(), // 
            };

            // Find the nearest location that is not within the distance threshold
            foreach (Vector3 location in list.ToList())
            {
                if (Game.LocalPlayer.Character.Position.DistanceTo(location) < 80f)
                {
                    list.Remove(location); // Remove locations within the distance threshold
                }
            }

            SpawnPoint = LocationChooser.chooseNearestLocation(list);
            scenario = new Random().Next(0, 100);
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
            CalloutMessage = "[SWL]~w~ Reports of Shots Fired.";
            CalloutPosition = SpawnPoint;
            switch (new Random().Next(1, 3))
            { 
                case 1:
                    Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION", SpawnPoint);
                    break;
                case 2:
                    Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", SpawnPoint);
                    break;
            }
            Game.LogTrivial("SWLCallouts - Shots Fired callout offered.");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("SWLCallouts Log: Shots Fired callout accepted.");
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~Dispatch: ~w~Someone called the police because of shots fired. Respond with ~r~Code 3");

            switch (new Random().Next(1, 3))
            {
                case 1:
                    Suspect1 = new Ped(SpawnPoint);
                    Suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                    Suspect1.BlockPermanentEvents = true;
                    Suspect1.IsPersistent = true;
                    Suspect1.Tasks.Wander();
                    callOutScene = 1;
                    break;
                case 2:
                    Suspect1 = new Ped(SpawnPoint);
                    Suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                    Suspect1.BlockPermanentEvents = true;
                    Suspect1.IsPersistent = true;
                    Suspect1.Tasks.Wander();

                    Suspect2 = new Ped(SpawnPoint);
                    Suspect2.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                    Suspect2.BlockPermanentEvents = true;
                    Suspect2.IsPersistent = true;
                    Suspect2.Tasks.Wander();
                    callOutScene = 2;
                    break;
            }

            Ped1 = new Ped(SpawnPoint);
            Ped2 = new Ped(SpawnPoint);
            Ped3 = new Ped(SpawnPoint);
            Ped1.IsPersistent = true;
            Ped2.IsPersistent = true;
            Ped3.IsPersistent = true;
            Ped1.Tasks.Wander();
            Ped2.Tasks.Wander();
            Ped3.Tasks.Wander();

            searcharea = SpawnPoint.Around2D(1f, 2f);
            Blip = new Blip(searcharea, 80f)
            {
                Color = Color.Red,
                Alpha = 0.5f
            }; 
            Blip.EnableRoute(Color.Red);

            if (Settings.ActivateAIBackup)
            {
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.SwatTeam);
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
            }
            else { Settings.ActivateAIBackup = false; }
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (Blip) Blip.Delete();
            if (Suspect1) Suspect1.Delete();
            if (Suspect2.Exists()) Suspect2.Delete();
            if (Ped1) Ped1.Delete();
            if (Ped2) Ped2.Delete();
            if (Ped3) Ped3.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (Suspect1.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
                {
                    if (Blip) Blip.Delete();
                }
                if (Suspect2.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
                {
                    if (Blip) Blip.Delete();
                }
                if (Suspect1.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !isArmed)
                {
                    Suspect1.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
                    isArmed = true;
                }
                if (Suspect2.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !isArmed)
                {
                    Suspect2.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
                    isArmed = true;
                }
                if ((Suspect1 && Suspect1.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 40f) || (Suspect2 && Suspect2.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 40f) && !hasBegunAttacking)
                {
                    if (scenario > 40)
                    {
                        if (callOutScene == 1)
                        {
                            new RelationshipGroup("SI");
                            new RelationshipGroup("PI");
                            Suspect1.RelationshipGroup = "SI";
                            Ped1.RelationshipGroup = "PI";
                            Ped2.RelationshipGroup = "PI";
                            Ped3.RelationshipGroup = "PI";
                            Suspect1.KeepTasks = true;
                            Suspect2.KeepTasks = true;
                            Game.SetRelationshipBetweenRelationshipGroups("SI", "PI", Relationship.Hate);
                            Suspect1.Tasks.FightAgainstClosestHatedTarget(1000f);
                            GameFiber.Wait(2000);
                            Suspect1.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            hasBegunAttacking = true;
                            GameFiber.Wait(600);
                        }
                        else if (callOutScene == 2)
                        {
                            new RelationshipGroup("SI");
                            new RelationshipGroup("SII");
                            new RelationshipGroup("PI");
                            Suspect1.RelationshipGroup = "SI";
                            Suspect2.RelationshipGroup = "SII";
                            Ped1.RelationshipGroup = "PI";
                            Ped2.RelationshipGroup = "PI";
                            Ped3.RelationshipGroup = "PI";
                            Suspect1.KeepTasks = true;
                            Suspect2.KeepTasks = true;
                            Game.SetRelationshipBetweenRelationshipGroups("SI", "PI", Relationship.Hate);
                            Game.SetRelationshipBetweenRelationshipGroups("SII", "PI", Relationship.Hate);
                            Game.SetRelationshipBetweenRelationshipGroups("SI", "SII", Relationship.Hate);
                            Game.SetRelationshipBetweenRelationshipGroups("SII", "SI", Relationship.Hate);
                            Suspect1.Tasks.FightAgainstClosestHatedTarget(1000f);
                            Suspect2.Tasks.FightAgainstClosestHatedTarget(1000f);
                            GameFiber.Wait(2000);
                            Suspect1.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            Suspect2.Tasks.FightAgainst(Game.LocalPlayer.Character);
                            hasBegunAttacking = true;
                            GameFiber.Wait(600);
                        }
                    }
                    else
                    {
                        if (!hasPursuitBegun)
                        {
                            if (callOutScene == 1)
                            {
                                Suspect1.Face(Game.LocalPlayer.Character);
                                Suspect1.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                                Game.DisplayNotification("~b~Dispatch:~w~ The suspect is surrendering. Try to ~o~arrest them~w~.");
                                hasPursuitBegun = true;
                            }
                            else if (callOutScene == 2)
                            {
                                Suspect1.Face(Game.LocalPlayer.Character);
                                Suspect2.Face(Game.LocalPlayer.Character);
                                Suspect1.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                                Suspect2.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                                Game.DisplayNotification("~b~Dispatch:~w~ The suspects are surrendering. Try to ~o~arrest them both~w~.");
                                hasPursuitBegun = true;
                            }
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (Suspect1 && Suspect1.IsDead) End();
                if (Suspect1 && Functions.IsPedArrested(Suspect1)) End();
                if (Suspect2.Exists() && Suspect2.IsDead) End();
                if (Suspect2.Exists() && Functions.IsPedArrested(Suspect2)) End();
            }, "Reports of Shots Fired [SWLCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (Suspect1) Suspect1.Dismiss();
            if (Suspect2.Exists()) Suspect2.Dismiss();
            if (Ped1) Ped1.Dismiss();
            if (Ped2) Ped2.Dismiss();
            if (Ped3) Ped3.Dismiss();
            if (Blip) Blip.Delete();
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("SWLCallouts - Shots Fired cleanup.");
            base.End();
        }
    }
}