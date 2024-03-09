// Author: Scottywonderful
// Created: 4th Mar 2024
// Version: 0.4.5.6

using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using System;
using Rage;
using System.Drawing;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("[SWL] Reports of a Stolen Emergency Vehicle (1)", CalloutProbability.Medium)]
    class SWLStolenEmergencyVehicle : Callout
    {
        private readonly string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_REVOLVER", "WEAPON_DOUBLEACTION", "WEAPON_CERAMICPISTOL" };
        private readonly string[] copVehicles = new string[] { "POLICE", "POLICE2", "POLICE3", "POLICE4", "FBI", "FBI2", "POLICEB", "SHERIFF", "SHERIFF2", "pbus", "pranger", "policet" };
        private Vehicle EmergencyVehicle;
        private Ped Suspect;
        private Vector3 SpawnPoint;
        private Blip Blip;
        private LHandle pursuit;
        //#pragma warning disable CS0414 // Ignores the warning on we get with the next line.
        //private bool pursuitCreated = false;
        //#pragma warning restore CS0414 // Looks for other CS0414 errors outide of here.
        readonly string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 15f);
            CalloutMessage = "[SWL]~w~ Reports of a Stolen Emergency Vehicle.";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 FOR CRIME_STOLEN_VEHICLE CODE3", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("SWLCallouts Log: Stolen Emergency Vehicle callout accepted.");

            EmergencyVehicle = new Vehicle(copVehicles[new Random().Next((int)copVehicles.Length)], SpawnPoint);
            EmergencyVehicle.IsSirenOn = true;
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Dispatch", "Loading ~g~Information~w~ of the ~y~LSPD Database~w~...");
            Functions.DisplayVehicleRecord(EmergencyVehicle, true);

            Suspect = new Ped(SpawnPoint);
            Suspect.WarpIntoVehicle(EmergencyVehicle, -1);
            Suspect.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
            Suspect.BlockPermanentEvents = true;

            Blip = Suspect.AttachBlip();

            pursuit = Functions.CreatePursuit();
            Functions.AddPedToPursuit(pursuit, Suspect);
            Functions.SetPursuitIsActiveForPlayer(pursuit, true);
            //pursuitCreated = true;

            if (Settings.ActivateAIBackup)
            {
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.StateUnit);
                Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit);
            }
            else { Settings.ActivateAIBackup = false; }
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (Suspect) Suspect.Delete();
            if (EmergencyVehicle) EmergencyVehicle.Delete();
            if (Blip) Blip.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (Suspect && Suspect.IsDead) End();
                if (Suspect && Functions.IsPedArrested(Suspect)) End();
            }, "Stolen Emergency Vehicle [SWLCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (Blip) Blip.Delete();
            if (EmergencyVehicle) EmergencyVehicle.Dismiss();
            if (Suspect) Suspect.Dismiss();
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Stolen Emergency Vehicle", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}