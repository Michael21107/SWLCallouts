// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.4.5

using Rage;
using System;
using System.Drawing;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using SWLCallouts.Stuff;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("[SWL] High Speed Chase", CalloutProbability.Medium)]
    public class SWLHighSpeedChase : Callout
    {
        private Vehicle SuspectVehicle;
        private string[] VehicleList = new string[] { "ADDER", "AKUMA", "BANSHEE", "BATI", "BULLET", "CARBONRS", "CHEETAH", "COMET", "COQUETTE", "DOUBLE", "ENTITYXF", "HAKUCHOU", "INFERNUS", "JESTER", "MASSACRO", "NEMESIS", "NINEF", "OSIRIS", "PANTO", "PCJ", "SURANO", "T20", "VACCA", "VOLTIC", "ZENTORNO" };
        private Ped Suspect;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip;
        private LHandle Pursuit;
        private bool PursuitCreated = false;
#pragma warning disable CS0414 // Ignores the warning on we get with the next line.
        private bool notificationDisplayed = false;
#pragma warning restore CS0414 // Looks for other CS0414 errors outide of here.

        public override bool OnBeforeCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 50f);
            CalloutMessage = "[SWL]~w~ High Speed Chase in progress";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", SpawnPoint);

            Game.LogTrivial("SWLCallouts - High Speed Chase Offered.");

            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.LogTrivial("SWLCallouts - High Speed Chase Accepted.");
            SuspectVehicle = new Vehicle(VehicleList[new Random().Next((int)VehicleList.Length)], SpawnPoint);
            SuspectVehicle.IsPersistent = true;

            Suspect = SuspectVehicle.CreateRandomDriver();
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;
            Suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.IsFriendly = false;

            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            if (!PursuitCreated && Game.LocalPlayer.Character.DistanceTo(SuspectVehicle) <= 30f)
            {
                Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(Pursuit, Suspect);
                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);

                if (Settings.ActivateAIBackup)
                {
                    Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                    Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                    Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit); // Unsure if I should have the air unit respond automatically or not //
                }
                else { Settings.ActivateAIBackup = false; }
                PursuitCreated = true;
            }
            if (Game.LocalPlayer.Character.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
            //if (Suspect != null && Suspect.IsDead) End();
            //if (Suspect != null && Functions.IsPedArrested(Suspect)) End();
            base.Process();
        }

        public override void End()
        {
            //if (Suspect != null && Suspect.Exists()) Suspect.Dismiss();
            //if (SuspectVehicle != null && SuspectVehicle.Exists()) SuspectVehicle.Dismiss();
            if (SuspectBlip != null && SuspectBlip.Exists()) SuspectBlip.Delete();
            string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "[SWL] ~y~High Speed Chase", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

            Game.LogTrivial("SWLCallouts - High Speed Chase Cleanup.");
            base.End();
        }
    }
}
