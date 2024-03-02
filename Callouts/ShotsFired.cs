// Author: Scottywonderful
// Created: 2nd Mar 2024
// Version: 0.4.5.1

using System;
using Rage;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;
using System.Collections.Generic;
using SWLCallouts.Stuff;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("[SWL] Reports of Shots Fired", CalloutProbability.Medium)]
    public class SWLShotsFired : Callout
    {
        private string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_ASSAULTRIFLE", "WEAPON_SAWNOFFSHOTGUN", "WEAPON_PISTOL50" };
        private Ped Suspect;
        private Ped V1;
        private Ped V2;
        private Ped V3;
        private Vector3 SpawnPoint;
        private Vector3 searcharea;
        private Blip Blip;
        private int scenario = 0;
        private bool hasBegunAttacking = false;
        private bool isArmed = false;
        private bool hasPursuitBegun = false;

        public override bool OnBeforeCalloutDisplayed()
        {
            Random random = new Random();
            List<Vector3> list = new List<Vector3>
            {
                new Vector3(-1622.711f, 214.8514f, 60.22071f),
                new Vector3(295.0424f, -578.2471f, 43.18422f),
                new Vector3(-1573.039f, -1169.825f, 2.402837f),
                new Vector3(-1323.908f, 50.76834f, 53.53567f),
                new Vector3(1155.258f, -741.4567f, 57.30391f),
                new Vector3(291.6201f, 179.956f, 104.297f),
        };
            SpawnPoint = LocationChooser.chooseNearestLocation(list);
            scenario = new Random().Next(0, 100);
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
            CalloutMessage = "[SWL]~w~ Reports of Shots Fired.";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION", SpawnPoint);
            // Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("SWLCallouts Log: Reports of Shots Fired callout accepted.");
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~Dispatch: ~w~Someone called the police because of shots fired. Respond with ~r~Code 3");

            Suspect = new Ped(SpawnPoint);
            Suspect.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
            Suspect.BlockPermanentEvents = true;
            Suspect.IsPersistent = true;
            Suspect.Tasks.Wander();

            V1 = new Ped(SpawnPoint);
            V2 = new Ped(SpawnPoint);
            V3 = new Ped(SpawnPoint);
            V1.IsPersistent = true;
            V2.IsPersistent = true;
            V3.IsPersistent = true;
            V1.Tasks.Wander();
            V2.Tasks.Wander();
            V3.Tasks.Wander();

            searcharea = SpawnPoint.Around2D(1f, 2f);
            Blip = new Blip(searcharea, 80f);
            Blip.Color = Color.Red;
            Blip.EnableRoute(Color.Red);
            Blip.Alpha = 0.5f;

            if (Settings.ActivateAIBackup)
            {
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.SwatTeam);
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
            }
            else { return false; }
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (Blip) Blip.Delete();
            if (Suspect) Suspect.Delete();
            if (V1) V1.Delete();
            if (V2) V2.Delete();
            if (V3) V3.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (Suspect.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
                {
                    if (Blip) Blip.Delete();
                }
                if (Suspect.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !isArmed)
                {
                    Suspect.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
                    isArmed = true;
                }
                if (Suspect && Suspect.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 40f && !hasBegunAttacking)
                {
                    if (scenario > 40)
                    {
                        new RelationshipGroup("AG");
                        new RelationshipGroup("VI");
                        Suspect.RelationshipGroup = "AG";
                        V1.RelationshipGroup = "VI";
                        V2.RelationshipGroup = "VI";
                        V3.RelationshipGroup = "VI";
                        Suspect.KeepTasks = true;
                        Game.SetRelationshipBetweenRelationshipGroups("AG", "VI", Relationship.Hate);
                        Suspect.Tasks.FightAgainstClosestHatedTarget(1000f);
                        GameFiber.Wait(2000);
                        Suspect.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        hasBegunAttacking = true;
                        GameFiber.Wait(600);
                    }
                    else
                    {
                        if (!hasPursuitBegun)
                        {
                            Suspect.Face(Game.LocalPlayer.Character);
                            Suspect.Tasks.PutHandsUp(-1, Game.LocalPlayer.Character);
                            Game.DisplayNotification("~b~Dispatch:~w~ The suspect is surrendering. Try to ~o~arrest him~w~.");
                            hasPursuitBegun = true;
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (Suspect && Suspect.IsDead) End();
                if (Suspect && Functions.IsPedArrested(Suspect)) End();
            }, "Reports of Shots Fired [SWLCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (Suspect) Suspect.Dismiss();
            if (V1) V1.Dismiss();
            if (V2) V2.Dismiss();
            if (V3) V3.Dismiss();
            if (Blip) Blip.Delete();
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}