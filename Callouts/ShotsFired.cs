// Author: Scottywonderful
// Created: 2nd Mar 2024
// Version: 0.4.8.8

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of Shots Fired", CalloutProbability.Medium)]
public class SWLShotsFired : Callout
{
    private Ped _suspect1;
    private Ped _suspect2;
    private Ped _ped1;
    private Ped _ped2;
    private Ped _ped3;
    private Vector3 _spawnPoint;
    private Vector3 _searcharea;
    private Blip _blip;
    private int _callOutScene = 0;
    private int _scenario = 0;
    private bool _hasBegunAttacking = false;
    private bool _isArmed = false;
    private bool _hasPursuitBegun = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        _ = new Random();
        List<Vector3> list = new List<Vector3>
        {
            // City Locations //
            new(-1622.711f, 214.8514f, 60.22071f), // Richman Uni
            new(295.0424f, -578.2471f, 43.18422f), // Pillbox Hill Med
            new(-1573.039f, -1169.825f, 2.402837f), // Del Pero Pier Beach
            new(-1323.908f, 50.76834f, 53.53567f), // Golfing Society
            new(1155.258f, -741.4567f, 57.30391f), // Mirror Park
            new(291.6201f, 179.956f, 104.297f), // Downtown Vinewood
            new(39.61766f, -1743.935f, 29.30354f), // Davis
            new(-2196245f, -1998.847f, 27.75542f), // Maze Bank Arena
            new(619.7628f, -2762.83f, 6.058973f), // Elysian Island
            new(-1035.871f, -2734.664f, 13.75664f), // LSIA
            // Blaine County Locations //
            new(2757.787f, 3468.468f, 55.72614f), // YouTool Sandy Shores
            new(25.71366f, 3640.249f, 39.7965f), // Stab City
            new(1703.841f, 4801.849f, 41.77493f), // Wonderama Grapeseed
            new(3516.073f, 3764.618f, 29.91412f), // Humane Labs
            new(591.8223f, 2735.149f, 42.06024f), // Dollar Pills Harmony
            new(-1907.137f, 2048.193f, 140.7384f), // Tongva Hills Vines
            // Paleto Bay Locations //
            new(-2203.282f, 4288.408f, 48.46412f), // North Chumash GOH
            new(-577.6245f, 5326.903f, 70.2608f), // Sawmill
            new(696.0341f, 5818.545f, 17.25508f), // Wooden Lodge
            new(62.00109f, 6351.052f, 31.22805f), // Chick Factory
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
        _scenario = new Random().Next(0, 100);
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        Normal("Setting callout message and location...");
        CalloutMessage = "[SWL]~w~ Reports of Shots Fired.";
        CalloutPosition = _spawnPoint;
        Normal("Message and Position set.");
        Normal("Choosing callout audio...");
        switch (new Random().Next(1, 3))
        { 
            case 1:
                Normal("Audio 1 selected.");
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION", _spawnPoint);
                break;
            case 2:
                Normal("Audio 2 selected.");
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", _spawnPoint);
                break;
        }
        Normal("Shots Fired callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("Shots Fired callout accepted.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~Dispatch: ~w~Someone called the police because of shots fired. Respond with ~r~Code 3");
        bool result = false; // This is to store the result of the switch statement below //

        switch (new Random().Next(1, 3))
        {
            case 1:
                Normal("Loaded callout scene 1");
                _callOutScene = 1;
                Normal("Spawning 1 suspect...");
                _suspect1 = new Ped(_spawnPoint);
                if (_suspect1.Exists())
                {
                    Normal("Spawned suspect 1 successfully.");
                    _suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                    _suspect1.BlockPermanentEvents = true;
                    _suspect1.IsPersistent = true;
                    Normal("Suspect Unarmed.");
                    try
                    {
                        Normal("Suspect wandering...");
                        _suspect1.Tasks.Wander();
                    }
                    catch (Exception ex)
                    {
                        Log("Failed to make Suspect 1 wander. Continuing with call.");
                        Error(ex, nameof(OnCalloutAccepted));
                    }
                    result = true; // Set result to true after successfully spawning suspect 1
                }
                else
                {
                    Log("Failed to detect suspect 1, ending call...");
                    End();
                    return false; // Return false if failed to spawn suspect 1
                }
                break;
            case 2:
                Normal("Loaded callout scene 2");
                _callOutScene = 2;
                Normal("Spawning 2 suspects...");
                _suspect1 = new Ped(_spawnPoint);
                _suspect2 = new Ped(_spawnPoint);
                if (_suspect1.Exists() && _suspect2.Exists())
                {
                    Normal("Spawned suspects successfully.");
                    _suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                    _suspect2.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                    _suspect1.BlockPermanentEvents = true;
                    _suspect2.BlockPermanentEvents = true;
                    _suspect1.IsPersistent = true;
                    _suspect2.IsPersistent = true;
                    Normal("Suspects Unarmed.");
                    try
                    {
                        Normal("Suspects wandering...");
                        _suspect1.Tasks.Wander();
                        _suspect2.Tasks.Wander();
                    }
                    catch (Exception ex)
                    {
                        Log("Failed to make Suspects wander. Continuing with call.");
                        Error(ex, nameof(OnCalloutAccepted));
                    }
                    result = true; // Set result to true after successfully spawning both suspects
                }
                else
                {
                    Log("Failed to detect both suspect 1 and suspect 2, ending call...");
                    End();
                    return false; // Return false if failed to spawn both suspects
                }
                break;
            default:
                Log("Unknown issue, ending call...");
                End();
                return false; // Return false by default if neither case is executed
        }

        // Code after the switch statement
        if (result)
        {
            // This block should only be executed if result is true
            Normal("Spawning civilian peds...");
            _ped1 = new Ped(_spawnPoint);
            _ped2 = new Ped(_spawnPoint);
            _ped3 = new Ped(_spawnPoint);
            _ped1.IsPersistent = true;
            _ped2.IsPersistent = true;
            _ped3.IsPersistent = true;
            Normal("Spawned civilians");
            if (_ped1.Exists()) _ped1.Tasks.Wander();
            if (_ped2.Exists()) _ped2.Tasks.Wander();
            if (_ped3.Exists()) _ped3.Tasks.Wander();

            Normal("Activating Blip...");
            _searcharea = _spawnPoint.Around2D(1f, 250f);
            _blip = new Blip(_searcharea, 80f)
            {
                Color = Color.Red,
                Alpha = 0.5f
            };
            _blip.EnableRoute(Color.Red);
            Normal("Blip Enabled");

            if (Settings.ActivateAIBackup)
            {
                Normal("Dispatch requesting for AI Cops to respond..");
                Functions.PlayScannerAudio("ATTENTION_ALL_UNITS_05 WE_HAVE_02 CITIZENS_REPORT_04 CRIME_SHOTS_FIRED_AT_AN_OFFICER_03 CODE3");
                GameFiber.Wait(5000);
                Normal("AI responding to dispatch and spawn enroute..");
                Functions.PlayScannerAudio("UNIT_RESPONDING_DISPATCH");
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                GameFiber.Wait(6000);
                Normal("SWAT Team are now enroute..");
                Functions.PlayScannerAudio("AI_BOBCAT4_RESPONDING");
                Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.SwatTeam);
            }
            else
            {
                Settings.ActivateAIBackup = false;
            }
        }
        else
        {
            Log("No results found. Ending call...");
            End();
        }

        return base.OnCalloutAccepted();
    }


    public override void OnCalloutNotAccepted()
    {
        Normal("ShotsFired callout NOT accepted.");
        if (_blip.Exists()) _blip.Delete();
        if (_suspect1.Exists()) _suspect1.Dismiss();
        if (_suspect2.Exists()) _suspect2.Dismiss();
        if (_ped1.Exists()) _ped1.Dismiss();
        if (_ped2.Exists()) _ped2.Dismiss();
        if (_ped3.Exists()) _ped3.Dismiss();
        Functions.PlayScannerAudio(CalloutNoAnswer.PickRandom());
        Normal("ShotFired callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (_suspect1 != null && (_suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 70f) && !_isArmed)
        {
            Normal("Given suspect1 a weapon");
            _suspect1.Inventory.GiveNewWeapon(WeaponList.PickRandom(), 500, true);
            _isArmed = true;
        }
        if (_suspect2 != null && (_suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 70f) && !_isArmed)
        {
            Normal("Given suspect2 a weapon");
            _suspect2.Inventory.GiveNewWeapon(WeaponList.PickRandom(), 500, true);
            _isArmed = true;
        }
        if ((_suspect1 != null && _suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f) || (_suspect2 != null && _suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f) && !_hasBegunAttacking)
        {
            if (_scenario > 40)
            {
                Normal("Arriving on scene..");
                if (_callOutScene == 1)
                {
                    Normal("Arrived at callout scene 1");
                    new RelationshipGroup("SI");
                    new RelationshipGroup("PI");
                    _suspect1.RelationshipGroup = "SI";
                    _ped1.RelationshipGroup = "PI";
                    _ped2.RelationshipGroup = "PI";
                    _ped3.RelationshipGroup = "PI";
                    _suspect1.KeepTasks = true;
                    Game.SetRelationshipBetweenRelationshipGroups("SI", "PI", Relationship.Hate);
                    _suspect1.Tasks.FightAgainstClosestHatedTarget(1000f);
                    Normal("Suspect attacking random civilians...");
                    GameFiber.Wait(2000);
                    _suspect1.Tasks.FightAgainst(GPlayer);
                    _hasBegunAttacking = true;
                    Normal("Suspect attacking player...");
                    GameFiber.Wait(600);

                }
                else if (_callOutScene == 2)
                {
                    Normal("Arrived at callout scene 2");
                    new RelationshipGroup("SI");
                    new RelationshipGroup("SII");
                    new RelationshipGroup("PI");
                    _suspect1.RelationshipGroup = "SI";
                    _suspect2.RelationshipGroup = "SII";
                    _ped1.RelationshipGroup = "PI";
                    _ped2.RelationshipGroup = "PI";
                    _ped3.RelationshipGroup = "PI";
                    _suspect1.KeepTasks = true;
                    _suspect2.KeepTasks = true;
                    Game.SetRelationshipBetweenRelationshipGroups("SI", "PI", Relationship.Hate);
                    Game.SetRelationshipBetweenRelationshipGroups("SII", "PI", Relationship.Hate);
                    Game.SetRelationshipBetweenRelationshipGroups("SI", "SII", Relationship.Hate);
                    Game.SetRelationshipBetweenRelationshipGroups("SII", "SI", Relationship.Hate);
                    _suspect1.Tasks.FightAgainstClosestHatedTarget(1000f);
                    _suspect2.Tasks.FightAgainstClosestHatedTarget(1000f);
                    Normal("Suspects attacking random civilians...");
                    GameFiber.Wait(2000);
                    _suspect1.Tasks.FightAgainst(GPlayer);
                    _suspect2.Tasks.FightAgainst(GPlayer);
                    _hasBegunAttacking = true;
                    Normal("Suspects attacking player...");
                    GameFiber.Wait(600);
                }
            }
            else
            {
                if (!_hasPursuitBegun)
                {
                    if (_callOutScene == 1)
                    {
                        Normal("Arrived at callout pursuit scene 1");
                        _suspect1.Face(GPlayer);
                        _suspect1.Tasks.PutHandsUp(-1, GPlayer);
                        HelpMsg("~b~Dispatch:~w~ The suspect is surrendering. Try to ~o~arrest them~w~.");
                        Normal("Suspect gives up.");
                        _hasPursuitBegun = true;
                    }
                    else if (_callOutScene == 2)
                    {
                        Normal("Arrived at callout pursuit scene 2");
                        _suspect1.Face(GPlayer);
                        _suspect2.Face(GPlayer);
                        _suspect1.Tasks.PutHandsUp(-1, GPlayer);
                        _suspect2.Tasks.PutHandsUp(-1, GPlayer);
                        HelpMsg("~b~Dispatch:~w~ The suspects are surrendering. Try to ~o~arrest them both~w~.");
                        Normal("Suspects give up.");
                        _hasPursuitBegun = true;
                    }
                }
            }
            
        }

        if (Game.IsKeyDown(Settings.EndCall) || GPlayer.IsDead) End();
        if (_suspect1.Exists() && (_suspect1.IsDead) || Functions.IsPedArrested(_suspect1)) End();
        if (_suspect2.Exists() && (_suspect2.IsDead) || Functions.IsPedArrested(_suspect2)) End();

        base.Process();
    }

    public override void End()
    {
        Normal("Call ended, cleaning up call...");
        if (_suspect1) _suspect1.Dismiss();
        if (_suspect2.Exists()) _suspect2.Dismiss();
        if (_ped1) _ped1.Dismiss();
        if (_ped2) _ped2.Dismiss();
        if (_ped3) _ped3.Dismiss();
        if (_blip) _blip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Reports of Shots Fired", SFDispatchCode4.PickRandom());
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Normal("ShotsFired call cleaned up.");
        base.End();
    }
}