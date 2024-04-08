/*// Author: Scottywonderful
// Created: 5th Apr 2024
// Version: 0.5.0.5

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Request for a CALLOUTNAME", CalloutProbability.Medium)]
internal class SWLEmergencyCallHangup : Callout
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
            new(917.1311f, -651.3591f, 57.86318f), // Mirror Park 1
            new(1329.527f, -609.7888f, 74.33716f), // Mirror Park 2
            new(-1905.715f, 365.4793f, 93.58082f), // Richman
            new(57.72354f, -1852.521f, 22.84686f), // Davis
            new(386.6045f, -1883.437f, 25.60606f), // Rancho
            new(-1043.986f, -1579.707f, 5.038178f), // La Puerta
            new(15.1229f, 522.7809f, 170.2276f), // Vinewood Hills 1
            new(-1071.873f, 575.5293f, 102.9082f), // Vinewood Hills 2
            new(-121.3116f, -21.69181f, 58.30245f), // West Vinewood/Burton
            new(-1849.062f, -633.7643f, 11.16098f), // Pacific Bluffs
            // Blaine County Locations //
            new(1661.571f, 4767.511f, 42.00745f), // Grapeseed
            new(1878.274f, 3922.46f, 33.06999f), // Sandy Shores
            new(-3204.997f, 1206.304f, 12.823f), // Chumash
            new(-2829.186f, 1419.623f, 100.9087f), // Banham Canyon
            new(372.15f, 2628.323f, 44.68521f), // Harmony
            new(15.56086f, 3687.897f, 39.57216f), // Stab City
            // Paleto Bay Locations //
            new(1442.942f, 6333.207f, 23.89725f), // Comm Camp, Mt Chiliad
            new(-4.716893f, 6664.873f, 31.15268f), // East Paleto
            new(-173.9554f, 6421.237f, 30.47764f), // Central Paleto
            new(-466.3274f, 6206.565f, 29.55285f), // West Paleto
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
        if (_spawnPoint.DistanceTo(GPlayer) < 50f)
        {
            Normal("Showing officer on scene.");
            NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~b~Dispatch", SFDispatchArrive.PickRandom());
        }
        if (Game.IsKeyDown(Settings.EndCall) || GPlayer.IsDead) End();
        if (_suspect.Exists() && (Functions.IsPedArrested(_suspect) || _suspect.IsDead)) End();
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