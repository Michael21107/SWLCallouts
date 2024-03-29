// Author: Scottywonderful
// Created: 11th Mar 2024
// Version: 0.4.8.8

#region

#endregion

using LSPD_First_Response.Engine;

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Cyclist on the Motorway", CalloutProbability.Medium)]
public class SWLCyclistOnTheMotorway : Callout
{
    private readonly string[] _pedList = new string[] { "A_F_Y_Hippie_01", "A_M_Y_Skater_01", "A_M_M_FatLatin_01", "A_M_M_EastSA_01", "A_M_Y_Latino_01", "G_M_Y_FamDNF_01", "G_M_Y_FamCA_01", "G_M_Y_BallaSout_01", "G_M_Y_BallaOrig_01", "G_M_Y_BallaEast_01", "G_M_Y_StrPunk_02", "S_M_Y_Dealer_01", "A_M_M_RurMeth_01", "A_M_Y_MethHead_01", "A_M_M_Skidrow_01", "S_M_Y_Dealer_01", "a_m_y_mexthug_01", "G_M_Y_MexGoon_03", "G_M_Y_MexGoon_02", "G_M_Y_MexGoon_01", "G_M_Y_SalvaGoon_01", "G_M_Y_SalvaGoon_02", "G_M_Y_SalvaGoon_03", "G_M_Y_Korean_01", "G_M_Y_Korean_02", "G_M_Y_StrPunk_01" };
    private readonly string[] _bikes = new string[] { "bmx", "Cruiser", "Fixter", "Scorcher", "tribike3", "tribike2", "tribike" };
    private Ped _suspect;
    private Vehicle _bike;
    private Vector3 _spawnPoint;
    private Blip _blip;
    private LHandle _pursuit;
    private bool _isStolen = false;
    private bool _startedPursuit = false;
    private bool _alreadySubtitleIntrod = false;

    public override bool OnBeforeCalloutDisplayed()
    {

        _ = new Random();
        List<Vector3> list = new List<Vector3>
        {
            // City Locations //
            new(1720.068f, 1535.201f, 84.72424f), // 
            new(2563.921f, 5393.056f, 44.55834f), // 
            new(-1826.79f, 4697.899f, 56.58701f), // 
            new(-1344.75f, -757.6135f, 11.10569f), // 
            new(1163.919f, 449.0514f, 82.59987f), // 
            //new(), // 
            //new(), // 
            //new(), // 
            //new(), // 
            //new(), // 
            // Blaine County Locations //
            //new(), // 
            //new(), // 
            //new(), // 
            //new(), // 
            //new(), // 
            //new(), // 
            // Paleto Bay Locations //
            //new(), // 
            //new(), // 
            //new(), // 
            //new(), // 
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

        _spawnPoint = LocationChooser.ChooseNearestLocation(list);
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        switch (new Random().Next(1, 2))
        {
            case 1:
                _isStolen = true;
                break;
            case 2:
                break;
        }
        CalloutMessage = "[SWL]~w~ Reports of a Cyclist on the Motorway";
        CalloutPosition = _spawnPoint;
        Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS SUSPICIOUS_PERSON IN_OR_ON_POSITION", _spawnPoint);
        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("CyclistOntheMotorway callout accepted.");
        Functions.PlayScannerAudio("CODE2");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Cyclist on the Motorway", "~b~Dispatch:~w~ Someone called the police because there is someone with a ~g~bicycle~w~ on the ~o~motorway~w~. Respond with ~y~Code 2");

        _suspect = new Ped(_pedList[new Random().Next((int)_pedList.Length)], _spawnPoint, 0f);
        _bike = new Vehicle(_bikes[new Random().Next((int)_bikes.Length)], _spawnPoint, 0f);
        _suspect.WarpIntoVehicle(_bike, -1);

        _blip = _bike.AttachBlip();
        _blip.Color = Color.LightBlue;
        _blip.EnableRoute(Color.Yellow);

        _suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.FollowTraffic);
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        Normal("CyclistOnTheMotorway callout NOT accepted.");
        if (_suspect) _suspect.Delete();
        if (_bike) _bike.Delete();
        if (_blip) _blip.Delete();
        Functions.PlayScannerAudio(CalloutNoAnswer.PickRandom());
        Normal("CyclistOnTheMotorway callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (_suspect.DistanceTo(Game.LocalPlayer.Character) < 20f)
        {
            if (_isStolen == true && _startedPursuit == false)
            {
                if (_blip) _blip.Delete();
                _pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(_pursuit, _suspect);
                Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                _startedPursuit = true;
                _bike.IsStolen = true;
                NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Dispatch Information", "The ~o~bicycle~w~ from the suspect is a ~o~" + _bike.Model.Name + "~w~. The ~g~bicycle~w~ was ~r~stolen~w~.");
                GameFiber.Wait(2000);
            }
            if (_suspect.DistanceTo(Game.LocalPlayer.Character) < 25f && Game.LocalPlayer.Character.IsOnFoot && _alreadySubtitleIntrod == false && _pursuit == null)
            {
                Print("Perform a normal traffic stop with the ~o~suspect~w~.");
                Print("~b~Dispatch:~w~ Checking the serial number of the bike...");
                GameFiber.Wait(2000);
                Print("~b~Dispatch~w~ We checked the serial number of the bike.<br>Model: ~o~" + _bike.Model.Name + "<br>~w~Serial number: ~o~" + _bike.LicensePlate);
                _alreadySubtitleIntrod = true;
                return;
            }
        }
        if (_suspect && Functions.IsPedArrested(_suspect) && _isStolen && _suspect.DistanceTo(Game.LocalPlayer.Character) < 15f)
        {
            Game.DisplaySubtitle("~y~Suspect: ~w~Please let me go! I'll bring the bike back.", 4000);
        }
        base.Process();
    }

    public override void End()
    {
        if (_suspect) _suspect.Dismiss();
        if (_bike) _bike.Dismiss();
        if (_blip) _blip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Bicycle on the Freeway", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        base.End();
    }
}