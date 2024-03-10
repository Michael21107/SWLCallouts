// Author: Scottywonderful
// Created: 16th Feb 2024
// Version: 0.4.6.4

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Officers Report of a High Speed Chase", CalloutProbability.Medium)]
public class SWLHighSpeedChase : Callout
{
    private Vehicle _suspectVehicle;
    private readonly string[] VehicleList = new string[] { "ADDER", "AKUMA", "BANSHEE", "BATI", "BULLET", "CARBONRS", "CHEETAH", "COMET", "COQUETTE", "DOUBLE", "ENTITYXF", "HAKUCHOU", "INFERNUS", "JESTER", "MASSACRO", "NEMESIS", "NINEF", "OSIRIS", "PANTO", "PCJ", "SURANO", "T20", "VACCA", "VOLTIC", "ZENTORNO" };
    private Ped _suspect;
    private Vector3 SpawnPoint;
    private Blip _suspectBlip;
    private LHandle Pursuit;
    private bool PursuitCreated = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        SpawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(1000f));
        ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 50f);
        CalloutMessage = "[SWL]~w~ High Speed Chase in progress";
        CalloutPosition = SpawnPoint;
        Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", SpawnPoint);
        Game.LogTrivial("SWLCallouts - High Speed Chase callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Game.LogTrivial("SWLCallouts - High Speed Chase callout accepted.");
        _suspectVehicle = new Vehicle(VehicleList[new Random().Next((int)VehicleList.Length)], SpawnPoint)
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

    public override void Process()
    {
        if (!PursuitCreated && GPlayer.DistanceTo(_suspectVehicle) <= 30f)
        {
            Pursuit = Functions.CreatePursuit();
            Functions.AddPedToPursuit(Pursuit, _suspect);
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
        if (GPlayer.IsDead) End();
        if (Game.IsKeyDown(Settings.EndCall)) End();
        //if (_suspect != null && _suspect.IsDead) End();
        //if (_suspect != null && Functions.IsPedArrested(_suspect)) End();
        base.Process();
    }

    public override void End()
    {
        //if (_suspect != null && _suspect.Exists()) _suspect.Dismiss();
        //if (_suspectVehicle != null && _suspectVehicle.Exists()) _suspectVehicle.Dismiss();
        if (_suspectBlip != null && _suspectBlip.Exists()) _suspectBlip.Delete();
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "[SWL] ~y~High Speed Chase", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Game.LogTrivial("SWLCallouts - High Speed Chase cleanup.");
        base.End();
    }
}
