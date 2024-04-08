/*
// Author: Scottywonderful
// Created: 5th Apr 2024
// Version: 0.5.0.5

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Request for a CALLOUTNAME", CalloutProbability.Medium)]
internal class CALLOUTNAME : Callout
{
    private Ped _suspect;
    private Vehicle _vehicle;
    private Vector3 _spawnPoint;
    private Vector3 _searcharea;
    private Blip _blip;

    public override bool OnBeforeCalloutDisplayed()
    {
        List<Vector3> list = new List<Vector3>
        {
            // City Locations //
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            // Blaine County Locations //
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            // Paleto Bay Locations //
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
            new(0f, 0f, 0f), // PLACENAME
        };

        // Find the nearest location that is not within the distance threshold
        foreach (Vector3 location in list.ToList())
        {
            if (GPlayer.Position.DistanceTo(location) < 80f)
            {
                Normal("Location too close, removing location from list...");
                list.Remove(location); // Remove locations within the distance threshold
                Normal("Choosing a different location..");
            }
        }

        Normal("Choosing nearest location for callout...");
        // Choose the nearest location from the updated list
        _spawnPoint = LocationChooser.ChooseNearestLocation(list);
        Normal("Displaying callout blip...");
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        Normal("Blip loaded.");

        Normal("Setting callout message and location...");
        CalloutMessage = "~w~Reports of CALLOUTNAME.";
        CalloutPosition = _spawnPoint;
        Normal("Message and Position set.");
        Normal("Choosing callout audio...");
        switch (new Random().Next(1, 3))
        {
            case 1:
                Normal("Audio 1 selected.");
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS IN_OR_ON_POSITION", _spawnPoint);
                break;
            case 2:
                Normal("Audio 2 selected.");
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CIV_ASSISTANCE IN_OR_ON_POSITION", _spawnPoint);
                break;
        }

        Normal("CALLOUTNAME callout offered.");
        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("CALLOUTNAME callout accepted.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Reports of CALLOUTNAME", "~b~Dispatch: ~w~Someone called the police because of shots fired. Respond with ~r~Code 3");

        Normal("Activating Blip...");
        _searcharea = _spawnPoint.Around2D(1f, 250f);
        _blip = new Blip(_searcharea, 80f)
        {
            Color = Color.Red,
            Alpha = 0.5f
        };
        _blip.EnableRoute(Color.Red);
        Normal("Blip Enabled");

        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        Normal("CALLOUTNAME callout NOT accepted.");
        if (_blip) _blip.Delete();
        if (_suspect) _suspect.Dismiss();
        Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
        Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
        Normal("CALLOUTNAME callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (_spawnPoint.DistanceTo(GPlayer) < 50f )
        {
            Normal("Showing officer on scene.");
            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~b~Dispatch", SFDispatchArrive.PickRandom());
        }
        if (Game.IsKeyDown(Settings.EndCall) || GPlayer.IsDead) End();
        //if (_suspect.Exists() && (Functions.IsPedArrested(_suspect) || _suspect.IsDead)) End();
        base.Process();
    }

    public override void End()
    {
        Normal("Call ended, cleaning up call...");
        if (_blip) _blip.Delete();
        if (_vehicle) _vehicle.Dismiss();
        if (_suspect) _suspect.Dismiss();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~b~DISPATCH", "~y~CALLOUTNAME", SFDispatchCode4.PickRandom());
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        base.End();
    }
}
*/