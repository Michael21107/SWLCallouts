// Author: Scottywonderful
// Created: 4th Mar 2024
// Version: 0.4.9.0

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of a Stolen Emergency Vehicle (2)", CalloutProbability.Medium)]
class SWLStolenEmergencyVehicle2 : Callout
{
    private readonly string[] _emVehicles = new string[] { "AMBULANCE", "FIRETRUK", "lguard", "riot", "riot2" };
    private Vehicle _emergencyVehicle;
    private Ped _suspect;
    private Vector3 _spawnPoint;
    private Blip _blip;
    private LHandle _pursuit;

    public override bool OnBeforeCalloutDisplayed()
    {
        Normal("Choosing nearest location for callout...");
        _spawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(750f));
        Normal("Displaying callout blip...");
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 15f);
        Normal("Displaying callout message...");
        CalloutMessage = "[SWL]~w~ Reports of a Stolen Emergency Vehicle.";
        CalloutPosition = _spawnPoint;
        Normal("Playing callout audio..");
        Functions.PlayScannerAudioUsingPosition("CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 FOR CRIME_STOLEN_VEHICLE", _spawnPoint);
        Normal("StolenEmergencyVehicle2 callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("SWLCallouts Log: Stolen Emergency Vehicle2 callout accepted.");
        Normal("Playing respond code 3 audio...");
        Functions.PlayScannerAudio("CODE3");

        Normal("Spawning stolen vehicle...");
        _emergencyVehicle = new Vehicle(_emVehicles[new Random().Next((int)_emVehicles.Length)], _spawnPoint)
        {
            IsSirenOn = true
        };
        Normal("Loading vehicle record...");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", "Loading ~g~Information~w~ of the ~y~LSPD Database~w~...");
        Functions.DisplayVehicleRecord(_emergencyVehicle, true);
        Normal("Displaying vehicle record.");

        Normal("Spawning suspect into vehicle...");
        _suspect = new Ped(_spawnPoint);
        _suspect.WarpIntoVehicle(_emergencyVehicle, -1);
        _suspect.Inventory.GiveNewWeapon(new WeaponAsset(PistolWeapons[new Random().Next((int)PistolWeapons.Length)]), 500, true);
        _suspect.BlockPermanentEvents = true;

        Normal("Loading spawnpoint...");
        _blip = _suspect.AttachBlip();

        _pursuit = Functions.CreatePursuit();
        Functions.AddPedToPursuit(_pursuit, _suspect);
        Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
        //pursuitCreated = true;
        Normal("Spawnpoint loaded, pursuit activated.");

        Normal("Is AI Backup activated?");
        if (Settings.ActivateAIBackup)
        {
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            GameFiber.Sleep(2000);
            Normal("AI Backup enabled. Sending backup units!");
            Functions.PlayScannerAudioUsingPosition("CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 FOR CRIME_STOLEN_VEHICLE", _spawnPoint);
            Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.StateUnit);
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit);
        }
        else
        {
            Normal("AI Backup disabled. Sending current unit.");
            Settings.ActivateAIBackup = false;
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
        }
        GameFiber.Sleep(2000);
        Normal("Showing officer responding/on scene.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", SEV2DispatchArrive.PickRandom());
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        Normal("StolenEmergencyVehicle2 callout NOT accepted.");
        if (_suspect) _suspect.Delete();
        if (_emergencyVehicle) _emergencyVehicle.Delete();
        if (_blip) _blip.Delete();
        Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
        Normal("StolenEmergencyVehicle2 callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (Game.IsKeyDown(Settings.EndCall) || GPlayer.IsDead) End();
        if (_suspect.Exists() && (Functions.IsPedArrested(_suspect) || _suspect.IsDead)) End();
        base.Process();
    }

    public override void End()
    {
        Normal("Call ended, cleaning up call...");
        if (_blip) _blip.Delete();
        if (_emergencyVehicle) _emergencyVehicle.Dismiss();
        if (_suspect) _suspect.Dismiss();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~b~DISPATCH", "~w~[SWL] ~y~Stolen Emergency Vehicle", SEV2DispatchCode4.PickRandom());
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        base.End();
    }
}