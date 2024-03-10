// Author: Scottywonderful
// Created: 4th Mar 2024
// Version: 0.4.6.4

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of a Stolen Emergency Vehicle (1)", CalloutProbability.Medium)]
class SWLStolenEmergencyVehicle : Callout
{
    private readonly string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_REVOLVER", "WEAPON_DOUBLEACTION", "WEAPON_CERAMICPISTOL" };
    private readonly string[] copVehicles = new string[] { "POLICE", "POLICE2", "POLICE3", "POLICE4", "FBI", "FBI2", "POLICEB", "SHERIFF", "SHERIFF2", "pbus", "pranger", "policet" };
    private Vehicle EmergencyVehicle;
    private Ped _suspect;
    private Vector3 SpawnPoint;
    private Blip Blip;
    private LHandle pursuit;

    public override bool OnBeforeCalloutDisplayed()
    {
        SpawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(1000f));
        ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 15f);
        CalloutMessage = "[SWL]~w~ Reports of a Stolen Emergency Vehicle.";
        CalloutPosition = SpawnPoint;
        Functions.PlayScannerAudioUsingPosition("CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 FOR CRIME_STOLEN_VEHICLE CODE3", SpawnPoint);
        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Game.LogTrivial("SWLCallouts Log: Stolen Emergency Vehicle callout accepted.");

        EmergencyVehicle = new Vehicle(copVehicles[new Random().Next((int)copVehicles.Length)], SpawnPoint)
        {
            IsSirenOn = true
        };
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Dispatch", "Loading ~g~Information~w~ of the ~y~LSPD Database~w~...");
        Functions.DisplayVehicleRecord(EmergencyVehicle, true);

        _suspect = new Ped(SpawnPoint);
        _suspect.WarpIntoVehicle(EmergencyVehicle, -1);
        _suspect.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
        _suspect.BlockPermanentEvents = true;

        Blip = _suspect.AttachBlip();

        pursuit = Functions.CreatePursuit();
        Functions.AddPedToPursuit(pursuit, _suspect);
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
        if (_suspect) _suspect.Delete();
        if (EmergencyVehicle) EmergencyVehicle.Delete();
        if (Blip) Blip.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        GameFiber.StartNew(delegate
        {
            if (GPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
            if (_suspect && _suspect.IsDead) End();
            if (_suspect && Functions.IsPedArrested(_suspect)) End();
        }, "Stolen Emergency Vehicle [SWLCallouts]");
        base.Process();
    }

    public override void End()
    {
        if (Blip) Blip.Delete();
        if (EmergencyVehicle) EmergencyVehicle.Dismiss();
        if (_suspect) _suspect.Dismiss();
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Stolen Emergency Vehicle", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        base.End();
    }
}