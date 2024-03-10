// Author: Scottywonderful
// Created: 2nd Mar 2024
// Version: 0.4.6.4

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of Shots Fired", CalloutProbability.Medium)]
public class SWLShotsFired : Callout
{
    private readonly string[] wepList = new string[] { "WEAPON_PISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_REVOLVER", "WEAPON_DOUBLEACTION", "WEAPON_CERAMICPISTOL",/*/ <<Pistols || Rifles>> /*/ "WEAPON_MIRCOSMG", "WEAPON_SMG", "WEAPON_TECPISTOL", "WEAPON_ASSAULTRIFLE", "WEAPON_BULLPUPRIFLE", "WEAPON_COMPACTRIFLE", "WEAPON_SAWNOFFSHOTGUN"};
    private Ped _suspect1;
    private Ped _suspect2;
    private Ped Ped1;
    private Ped Ped2;
    private Ped Ped3;
    private Vector3 SpawnPoint;
    private Vector3 searcharea;
    private Blip Blip;
    private int callOutScene = 0;
    private int scenario = 0;
    private bool hasBegunAttacking = false;
    private bool isArmed = false;
    private bool hasPursuitBegun = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        #pragma warning disable IDE0059 // Ignores the warning on we get with the next line.
        Random random = new();
        #pragma warning restore IDE0059 // Looks for other CS0414 errors outide of here.
        List<Vector3> list = new()
        {
            // City Locations //
            new Vector3(-1622.711f, 214.8514f, 60.22071f), // Richman Uni
            new Vector3(295.0424f, -578.2471f, 43.18422f), // Pillbox Hill Med
            new Vector3(-1573.039f, -1169.825f, 2.402837f), // Del Pero Pier Beach
            new Vector3(-1323.908f, 50.76834f, 53.53567f), // Golfing Society
            new Vector3(1155.258f, -741.4567f, 57.30391f), // Mirror Park
            new Vector3(291.6201f, 179.956f, 104.297f), // Downtown Vinewood
            new Vector3(39.61766f, -1743.935f, 29.30354f), // Davis
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
            // Blaine County Locations //
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
            // Paleto Bay Locations //
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
            //new Vector3(), // 
        };

        // Find the nearest location that is not within the distance threshold
        foreach (Vector3 location in list.ToList())
        {
            if (GPlayer.Position.DistanceTo(location) < 80f)
            {
                list.Remove(location); // Remove locations within the distance threshold
            }
        }

        SpawnPoint = LocationChooser.ChooseNearestLocation(list);
        scenario = new Random().Next(0, 100);
        ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
        CalloutMessage = "[SWL]~w~ Reports of Shots Fired.";
        CalloutPosition = SpawnPoint;
        switch (new Random().Next(1, 3))
        { 
            case 1:
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION", SpawnPoint);
                break;
            case 2:
                Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", SpawnPoint);
                break;
        }
        Game.LogTrivial("SWLCallouts - Shots Fired callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Game.LogTrivial("SWLCallouts Log: Shots Fired callout accepted.");
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~Dispatch: ~w~Someone called the police because of shots fired. Respond with ~r~Code 3");

        switch (new Random().Next(1, 3))
        {
            case 1:
                _suspect1 = new Ped(SpawnPoint);
                _suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                _suspect1.BlockPermanentEvents = true;
                _suspect1.IsPersistent = true;
                _suspect1.Tasks.Wander();
                callOutScene = 1;
                break;
            case 2:
                _suspect1 = new Ped(SpawnPoint);
                _suspect1.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                _suspect1.BlockPermanentEvents = true;
                _suspect1.IsPersistent = true;
                _suspect1.Tasks.Wander();

                _suspect2 = new Ped(SpawnPoint);
                _suspect2.Inventory.GiveNewWeapon("WEAPON_UNARMED", 500, true);
                _suspect2.BlockPermanentEvents = true;
                _suspect2.IsPersistent = true;
                _suspect2.Tasks.Wander();
                callOutScene = 2;
                break;
        }

        Ped1 = new Ped(SpawnPoint);
        Ped2 = new Ped(SpawnPoint);
        Ped3 = new Ped(SpawnPoint);
        Ped1.IsPersistent = true;
        Ped2.IsPersistent = true;
        Ped3.IsPersistent = true;
        Ped1.Tasks.Wander();
        Ped2.Tasks.Wander();
        Ped3.Tasks.Wander();

        searcharea = SpawnPoint.Around2D(1f, 2f);
        Blip = new Blip(searcharea, 80f)
        {
            Color = Color.Red,
            Alpha = 0.5f
        }; 
        Blip.EnableRoute(Color.Red);

        if (Settings.ActivateAIBackup)
        {
            Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.SwatTeam);
            Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
            Functions.RequestBackup(SpawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
        }
        else { Settings.ActivateAIBackup = false; }
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (Blip) Blip.Delete();
        if (_suspect1) _suspect1.Delete();
        if (_suspect2.Exists()) _suspect2.Delete();
        if (Ped1) Ped1.Delete();
        if (Ped2) Ped2.Delete();
        if (Ped3) Ped3.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        GameFiber.StartNew(delegate
        {
            if (_suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
            {
                if (Blip) Blip.Delete();
            }
            if (_suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f)
            {
                if (Blip) Blip.Delete();
            }
            if (_suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !isArmed)
            {
                _suspect1.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
                isArmed = true;
            }
            if (_suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 70f && !isArmed)
            {
                _suspect2.Inventory.GiveNewWeapon(new WeaponAsset(wepList[new Random().Next((int)wepList.Length)]), 500, true);
                isArmed = true;
            }
            if ((_suspect1 && _suspect1.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f) || (_suspect2 && _suspect2.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 40f) && !hasBegunAttacking)
            {
                if (scenario > 40)
                {
                    if (callOutScene == 1)
                    {
                        new RelationshipGroup("SI");
                        new RelationshipGroup("PI");
                        _suspect1.RelationshipGroup = "SI";
                        Ped1.RelationshipGroup = "PI";
                        Ped2.RelationshipGroup = "PI";
                        Ped3.RelationshipGroup = "PI";
                        _suspect1.KeepTasks = true;
                        _suspect2.KeepTasks = true;
                        Game.SetRelationshipBetweenRelationshipGroups("SI", "PI", Relationship.Hate);
                        _suspect1.Tasks.FightAgainstClosestHatedTarget(1000f);
                        GameFiber.Wait(2000);
                        _suspect1.Tasks.FightAgainst(GPlayer);
                        hasBegunAttacking = true;
                        GameFiber.Wait(600);
                    }
                    else if (callOutScene == 2)
                    {
                        new RelationshipGroup("SI");
                        new RelationshipGroup("SII");
                        new RelationshipGroup("PI");
                        _suspect1.RelationshipGroup = "SI";
                        _suspect2.RelationshipGroup = "SII";
                        Ped1.RelationshipGroup = "PI";
                        Ped2.RelationshipGroup = "PI";
                        Ped3.RelationshipGroup = "PI";
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
                        hasBegunAttacking = true;
                        GameFiber.Wait(600);
                    }
                }
                else
                {
                    if (!hasPursuitBegun)
                    {
                        if (callOutScene == 1)
                        {
                            _suspect1.Face(GPlayer);
                            _suspect1.Tasks.PutHandsUp(-1, GPlayer);
                            Game.DisplayNotification("~b~Dispatch:~w~ The _suspect is surrendering. Try to ~o~arrest them~w~.");
                            hasPursuitBegun = true;
                        }
                        else if (callOutScene == 2)
                        {
                            _suspect1.Face(GPlayer);
                            _suspect2.Face(GPlayer);
                            _suspect1.Tasks.PutHandsUp(-1, GPlayer);
                            _suspect2.Tasks.PutHandsUp(-1, GPlayer);
                            Game.DisplayNotification("~b~Dispatch:~w~ The _suspects are surrendering. Try to ~o~arrest them both~w~.");
                            hasPursuitBegun = true;
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
        if (Ped1) Ped1.Dismiss();
        if (Ped2) Ped2.Dismiss();
        if (Ped3) Ped3.Dismiss();
        if (Blip) Blip.Delete();
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Reports of Shots Fired", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Game.LogTrivial("SWLCallouts - Shots Fired cleanup.");
        base.End();
    }
}