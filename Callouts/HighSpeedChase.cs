// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.8.5

#region

#endregion

using Rage;

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Officers Report of a High Speed Chase", CalloutProbability.Medium)]
public class SWLHighSpeedChase : Callout
{
    private Vehicle _suspectVehicle;
    private readonly string[] _vehicleList = new string[] { "ADDER", "AKUMA", "BANSHEE", "BATI", "BULLET", "CARBONRS", "CHEETAH", "COMET", "COQUETTE", "DOUBLE", "ENTITYXF", "HAKUCHOU", "INFERNUS", "JESTER", "MASSACRO", "NEMESIS", "NINEF", "OSIRIS", "PANTO", "PCJ", "SURANO", "T20", "VACCA", "VOLTIC", "ZENTORNO" };
    private Ped _suspect;
    private Vector3 _spawnPoint;
    private Blip _suspectBlip;
    private LHandle _pursuit;
    private bool _pursuitCreated = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        _spawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(1000f));
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 50f);
        CalloutMessage = "[SWL]~w~ High Speed Chase in progress";
        CalloutPosition = _spawnPoint;
        Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", _spawnPoint);
        Normal("HighSpeedChase callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("HighSpeedChase callout accepted.");
        _suspectVehicle = new Vehicle(_vehicleList[new Random().Next((int)_vehicleList.Length)], _spawnPoint)
        {
            IsPersistent = true
        };

        _suspect = _suspectVehicle.CreateRandomDriver();
        _suspect.IsPersistent = true;
        _suspect.BlockPermanentEvents = true;
        _suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);

        _suspectBlip = _suspect.AttachBlip();
        _suspectBlip.IsFriendly = false;

        return base.OnCalloutAccepted();
    }
    public override void OnCalloutNotAccepted()
    {
        Normal("HighSpeedChase callout NOT accepted.");
        if (_suspect.Exists()) _suspect.Delete();
        if (_suspectVehicle.Exists()) _suspectVehicle.Delete();
        if (_suspectBlip.Exists()) _suspectBlip.Delete();
        Functions.PlayScannerAudio(CalloutNoAnswer.PickRandom());
        Normal("HighSpeedChase callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (!_pursuitCreated && GPlayer.DistanceTo(_suspectVehicle) <= 30f)
        {
            _pursuit = Functions.CreatePursuit();
            Functions.AddPedToPursuit(_pursuit, _suspect);
            Functions.SetPursuitIsActiveForPlayer(_pursuit, true);

            if (Settings.ActivateAIBackup)
            {
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit); // Unsure if I should have the air unit respond automatically or not //
            }
            else { Settings.ActivateAIBackup = false; }
            _pursuitCreated = true;
        }
        if (GPlayer.IsDead) End();
        if (Game.IsKeyDown(Settings.EndCall)) End();
        base.Process();
    }

    public override void End()
    {
        if (_suspectBlip && _suspectBlip.Exists()) _suspectBlip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "[SWL] ~y~High Speed Chase", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Normal("HighSpeedChase cleanup.");
        base.End();
    }
}
