// Author: Scottywonderful
// Created: 28th Feb 2024
// Version: 0.4.5.0

using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;
using System;
using System.Drawing;
using SWLCallouts.Stuff;
using System.Linq;

namespace SWLCallouts.Callouts
{
    [CalloutInfo("[SWL] Person With a Knife", CalloutProbability.Medium)]
    public class SWLPersonWithAKnife : Callout
    {
        private string[] pedList = new string[] { "A_F_M_SouCent_01", "A_F_M_SouCent_02", "A_M_Y_Skater_01", "A_M_M_FatLatin_01", "A_M_M_EastSA_01", "A_M_Y_Latino_01", "G_M_Y_FamDNF_01",
                                                  "G_M_Y_FamCA_01", "G_M_Y_BallaSout_01", "G_M_Y_BallaOrig_01", "G_M_Y_BallaEast_01", "G_M_Y_StrPunk_02", "S_M_Y_Dealer_01", "A_M_M_RurMeth_01",
                                                  "A_M_M_Skidrow_01", "A_M_Y_MexThug_01", "G_M_Y_MexGoon_03", "G_M_Y_MexGoon_02", "G_M_Y_MexGoon_01", "G_M_Y_SalvaGoon_01", "G_M_Y_SalvaGoon_02",
                                                  "G_M_Y_SalvaGoon_03", "G_M_Y_Korean_01", "G_M_Y_Korean_02", "G_M_Y_StrPunk_01" };
        private Ped subject;
        private Vector3 SpawnPoint;
        private Vector3 searcharea;
        private Blip Blip;
        private LHandle pursuit;
        private int scenario = 0;
        private bool hasBegunAttacking = false;
        private bool isArmed = false;
        private bool hasPursuitBegun = false;
#pragma warning disable CS0414
        private bool hasSpoke = false;
        private bool pursuitCreated = false;
#pragma warning restore CS0414
        string icon = Main.GetIconForDepartment(Settings.Department); // Get icons from Main.cs and Settings.cs

        public override bool OnBeforeCalloutDisplayed()
        {
            scenario = new Random().Next(0, 100);
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(1000f));
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
            CalloutMessage = "[SWL]~w~ Reports of a Person With a Knife.";
            CalloutPosition = SpawnPoint;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", SpawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "~y~Person With a Knife", "~b~Dispatch: ~w~Try to arrest the suspect. Respond with ~r~Code 3");
            Functions.PlayScannerAudio("UNITS_RESPOND_CODE_03_01");

            subject = new Ped(pedList[new Random().Next((int)pedList.Length)], SpawnPoint, 0f);
            subject.BlockPermanentEvents = true;
            subject.IsPersistent = true;
            subject.Tasks.Wander();

            searcharea = SpawnPoint.Around2D(1f, 2f);
            Blip = new Blip(searcharea, 80f);
            Blip.Color = Color.Orange;
            Blip.EnableRoute(Color.Orange);
            Blip.Alpha = 0.5f;
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (Blip) Blip.Delete();
            if (subject) subject.Delete();
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (subject.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !isArmed)
                {
                    subject.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                    isArmed = true;
                }
                if (subject && subject.DistanceTo(Game.LocalPlayer.Character.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !hasBegunAttacking)
                {
                    if (scenario > 40)
                    {
                        subject.KeepTasks = true;
                        subject.Tasks.FightAgainst(Game.LocalPlayer.Character);
                        hasBegunAttacking = true;
                        switch (new Random().Next(1, 3))
                        {
                            case 1:
                                Game.DisplaySubtitle("~r~Suspect: ~w~I do not want to live anymore!", 4000);
                                hasSpoke = true;
                                break;
                            case 2:
                                Game.DisplaySubtitle("~r~Suspect: ~w~Go away! - I'm not going back to the psychiatric hospital!", 4000);
                                hasSpoke = true;
                                break;
                            case 3:
                                Game.DisplaySubtitle("~r~Suspect: ~w~I'm not going back to the psychiatric hospital!", 4000);
                                hasSpoke = true;
                                break;
                            default: break;
                        }
                        GameFiber.Wait(2000);
                    }
                    else
                    {
                        if (!hasPursuitBegun)
                        {
                            pursuit = Functions.CreatePursuit();
                            Functions.AddPedToPursuit(pursuit, subject);
                            Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                            hasPursuitBegun = true;
                        }
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (subject && subject.IsDead) End();
                if (subject && Functions.IsPedArrested(subject)) End();
            }, "Person With a Knife [SWLCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (subject) subject.Dismiss();
            if (Blip) Blip.Delete();
            Game.DisplayNotification(icon, icon, "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}