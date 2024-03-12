// Author: Scottywonderful
// Created: 2nd Mar 2024
// Version: 0.4.7.2

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of Shots Fired", CalloutProbability.Medium)]
public class SWLShotsFired : Callout
{
    private readonly string[] _wepList = new string[] { "WEAPON_PISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_REVOLVER", "WEAPON_DOUBLEACTION", "WEAPON_CERAMICPISTOL",/*/ <<Pistols || Rifles>> /*/ "WEAPON_MIRCOSMG", "WEAPON_SMG", "WEAPON_TECPISTOL", "WEAPON_ASSAULTRIFLE", "WEAPON_BULLPUPRIFLE", "WEAPON_COMPACTRIFLE", "WEAPON_SAWNOFFSHOTGUN"};
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
                list.Remove(location); // Remove locations within the distance threshold
            }
        }

        _spawnPoint = LocationChooser.ChooseNearestLocation(list);
        _scenario = new Random().Next(0, 100);
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        CalloutMessage = "[SWL]~w~ Reports of Shots Fired.";
        CalloutPosition = _spawnPoint;
        switch (new Random().Next(1, 3))
        { 
            case 1:
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION", _spawnPoint);
                break;
            case 2:
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", _spawnPoint);
                break;
        }
        Log("SWLCallouts - Shots Fired callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Log("SWLCallouts Log: Shots Fired callout accepted.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~Dispatch: ~w~Someone called the police because of shots fired. Respond with ~r~Code 3");

        switch (new Random().Next(1, 3))
        {
            case 1:
                _suspect1 = new Ped(_spawnPoint);
                _suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                _suspect1.BlockPermanentEvents = true;
                _suspect1.IsPersistent = true;
                _suspect1.Tasks.Wander();
                _callOutScene = 1;
                break;
            case 2:
                _suspect1 = new Ped(_spawnPoint);
                _suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                _suspect1.BlockPermanentEvents = true;
                _suspect1.IsPersistent = true;
                _suspect1.Tasks.Wander();

                _suspect2 = new Ped(_spawnPoint);
                _suspect2.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                _suspect2.BlockPermanentEvents = true;
                _suspect2.IsPersistent = true;
                _suspect2.Tasks.Wander();
                _callOutScene = 2;
                break;
        }

        _ped1 = new Ped(_spawnPoint);
        _ped2 = new Ped(_spawnPoint);
        _ped3 = new Ped(_spawnPoint);
        _ped1.IsPersistent = true;
        _ped2.IsPersistent = true;
        _ped3.IsPersistent = true;
        _ped1.Tasks.Wander();
        _ped2.Tasks.Wander();
        _ped3.Tasks.Wander();

        _searcharea = _spawnPoint.Around2D(1f, 2f);
        _blip = new Blip(_searcharea, 80f)
        {
            Color = Color.Red,
            Alpha = 0.5f
        }; 
        _blip.EnableRoute(Color.Red);

        if (Settings.ActivateAIBackup)
        {
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.SwatTeam);
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
            Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
        }
        else { Settings.ActivateAIBackup = false; }
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (_blip) _blip.Delete();
        if (_suspect1) _suspect1.Delete();
        if (_suspect2.Exists()) _suspect2.Delete();
        if (_ped1) _ped1.Delete();
        if (_ped2) _ped2.Delete();
        if (_ped3) _ped3.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        GameFiber.StartNew(delegate
        {
            if (_suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
            {
                if (_blip) _blip.Delete();
            }
            if (_suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
            {
                if (_blip) _blip.Delete();
            }
            if (_suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !_isArmed)
            {
                _suspect1.Inventory.GiveNewWeapon(new WeaponAsset(_wepList[new Random().Next((int)_wepList.Length)]), 500, true);
                _isArmed = true;
            }
            if (_suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !_isArmed)
            {
                _suspect2.Inventory.GiveNewWeapon(new WeaponAsset(_wepList[new Random().Next((int)_wepList.Length)]), 500, true);
                _isArmed = true;
            }
            if ((_suspect1 && _suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f) || (_suspect2 && _suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f) && !_hasBegunAttacking)
            {
                if (_scenario > 40)
                {
                    if (_callOutScene == 1)
                    {
                        new RelationshipGroup("SI");
                        new RelationshipGroup("PI");
                        _suspect1.RelationshipGroup = "SI";
                        _ped1.RelationshipGroup = "PI";
                        _ped2.RelationshipGroup = "PI";
                        _ped3.RelationshipGroup = "PI";
                        _suspect1.KeepTasks = true;
                        _suspect2.KeepTasks = true;
                        Game.SetRelationshipBetweenRelationshipGroups("SI", "PI", Relationship.Hate);
                        _suspect1.Tasks.FightAgainstClosestHatedTarget(1000f);
                        GameFiber.Wait(2000);
                        _suspect1.Tasks.FightAgainst(GPlayer);
                        _hasBegunAttacking = true;
                        GameFiber.Wait(600);
                    }
                    else if (_callOutScene == 2)
                    {
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
                        GameFiber.Wait(2000);
                        _suspect1.Tasks.FightAgainst(GPlayer);
                        _suspect2.Tasks.FightAgainst(GPlayer);
                        _hasBegunAttacking = true;
                        GameFiber.Wait(600);
                    }
                }
                else
                {
                    if (!_hasPursuitBegun)
                    {
                        if (_callOutScene == 1)
                        {
                            _suspect1.Face(GPlayer);
                            _suspect1.Tasks.PutHandsUp(-1, GPlayer);
                            HelpMsg("~b~Dispatch:~w~ The _suspect is surrendering. Try to ~o~arrest them~w~.");
                            _hasPursuitBegun = true;
                        }
                        else if (_callOutScene == 2)
                        {
                            _suspect1.Face(GPlayer);
                            _suspect2.Face(GPlayer);
                            _suspect1.Tasks.PutHandsUp(-1, GPlayer);
                            _suspect2.Tasks.PutHandsUp(-1, GPlayer);
                            HelpMsg("~b~Dispatch:~w~ The _suspects are surrendering. Try to ~o~arrest them both~w~.");
                            _hasPursuitBegun = true;
                        }
                    }
                }
            }
            if (GPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
            if (_suspect1 && _suspect1.IsDead) End();
            if (_suspect1 && Functions.IsPedArrested(_suspect1)) End();
            if (_suspect2.Exists() && _suspect2.IsDead) End();
            if (_suspect2.Exists() && Functions.IsPedArrested(_suspect2)) End();
        }, "Reports of Shots Fired [SWLCallouts]");
        base.Process();
    }

    public override void End()
    {
        if (_suspect1) _suspect1.Dismiss();
        if (_suspect2.Exists()) _suspect2.Dismiss();
        if (_ped1) _ped1.Dismiss();
        if (_ped2) _ped2.Dismiss();
        if (_ped3) _ped3.Dismiss();
        if (_blip) _blip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Log("SWLCallouts - Shots Fired cleanup.");
        base.End();
    }
}