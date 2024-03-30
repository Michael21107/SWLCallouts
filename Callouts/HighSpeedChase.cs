// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.9.0

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

        Normal("Spawning suspect & suspect vehicle...");
        _suspect = _suspectVehicle.CreateRandomDriver();
        _suspect.IsPersistent = true;
        _suspect.BlockPermanentEvents = true;
        _suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);
        Normal("Suspect and Vehicle spawned.");

        Normal("Creating suspect blip...");
        _suspectBlip = _suspect.AttachBlip();
        _suspectBlip.IsFriendly = false;
        Normal("Blip created.");

        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        Normal("HighSpeedChase callout NOT accepted.");
        if (_suspect) _suspect.Delete();
        if (_suspectVehicle) _suspectVehicle.Delete();
        if (_suspectBlip) _suspectBlip.Delete();
        Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
        Normal("HighSpeedChase callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (!_pursuitCreated && GPlayer.DistanceTo(_suspectVehicle) <= 30f)
        {
            Normal("Showing officer in pursuit.");
            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch", HSCDispatchArrive.PickRandom());
            Normal("Creating pursuit...");
            _pursuit = Functions.CreatePursuit();
            Functions.AddPedToPursuit(_pursuit, _suspect);
            Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
            Normal("Pursuit created.");

            Normal("Is AI Backup activated?");
            if (Settings.ActivateAIBackup)
            {
                Normal("AI Backup enabled. Sending backup units!");
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.AirUnit); // Unsure if I should have the air unit respond automatically or not //
            }
            else { Settings.ActivateAIBackup = false; }
            _pursuitCreated = true;
        }
        if (Game.IsKeyDown(Settings.EndCall) || GPlayer.IsDead) End();
        if (_suspect.Exists() && (Functions.IsPedArrested(_suspect) || _suspect.IsDead)) End();
        base.Process();
    }

    public override void End()
    {
        Normal("Called ended, cleaning up call...");
        if (_suspectBlip && _suspectBlip.Exists()) _suspectBlip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~b~DISPATCH", "~w~[SWL] ~y~High Speed Chase", HSCDispatchCode4.PickRandom());
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Normal("HighSpeedChase cleanup.");
        base.End();
    }
}
