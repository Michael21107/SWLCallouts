// Author: Scottywonderful
// Created: 4th Mar 2024
// Version: 0.4.8.1

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of a Stolen Emergency Vehicle (2)", CalloutProbability.Medium)]
class SWLStolenEmergencyVehicle2 : Callout
{
    private readonly string[] _wepList = new string[] { "WEAPON_PISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_REVOLVER", "WEAPON_DOUBLEACTION", "WEAPON_CERAMICPISTOL" };
    private readonly string[] _emVehicles = new string[] { "AMBULANCE", "FIRETRUK", "lguard", "riot", "riot2" };
    private Vehicle _emergencyVehicle;
    private Ped _suspect;
    private Vector3 _spawnPoint;
    private Blip _blip;
    private LHandle _pursuit;

    public override bool OnBeforeCalloutDisplayed()
    {
        _spawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(1000f));
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 15f);
        CalloutMessage = "[SWL]~w~ Reports of a Stolen Emergency Vehicle.";
        CalloutPosition = _spawnPoint;
        Functions.PlayScannerAudioUsingPosition("CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01 FOR CRIME_STOLEN_VEHICLE CODE3", _spawnPoint);
        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Log("SWLCallouts Log: Stolen Emergency Vehicle2 callout accepted.");

        _emergencyVehicle = new Vehicle(_emVehicles[new Random().Next((int)_emVehicles.Length)], _spawnPoint)
        {
            IsSirenOn = true
        };
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", "Loading ~g~Information~w~ of the ~y~LSPD Database~w~...");
        Functions.DisplayVehicleRecord(_emergencyVehicle, true);

        _suspect = new Ped(_spawnPoint);
        _suspect.WarpIntoVehicle(_emergencyVehicle, -1);
        _suspect.Inventory.GiveNewWeapon(new WeaponAsset(_wepList[new Random().Next((int)_wepList.Length)]), 500, true);
        _suspect.BlockPermanentEvents = true;

        _blip = _suspect.AttachBlip();

        _pursuit = Functions.CreatePursuit();
        Functions.AddPedToPursuit(_pursuit, _suspect);
        Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
        //pursuitCreated = true;

        if (Settings.ActivateAIBackup)
        {
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.StateUnit);
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit);
        }
        else
        {
            Settings.ActivateAIBackup = false;
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
        }
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (_suspect) _suspect.Delete();
        if (_emergencyVehicle) _emergencyVehicle.Delete();
        if (_blip) _blip.Delete();
        Functions.PlayScannerAudio(CalloutNoAnswer.PickRandom());
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
        if (_blip) _blip.Delete();
        if (_emergencyVehicle) _emergencyVehicle.Dismiss();
        if (_suspect) _suspect.Dismiss();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Stolen Emergency Vehicle", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        base.End();
    }
}