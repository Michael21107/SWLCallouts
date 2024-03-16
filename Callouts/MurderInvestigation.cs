// Author: Scottywonderful
// Created: 11th Mar 2024
// Version: 0.4.8.4

#region
using LSPD_First_Response.Engine.Scripting.Entities;
#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Murder Investigation", CalloutProbability.Medium)]
internal class SWLMurderInvestigation : Callout
{
    private readonly string[] _copCars = new string[] { "FBI", "FBI2" };
    private Ped _deadPerson;
    private Ped _deadPerson2;
    private Ped _murderer;
    private Ped _cop1;
    private Ped _cop2;
    private Ped _coroner1;
    private Ped _coroner2;
    private Vehicle _coronerVeh;
    private Vehicle _copVeh;
    private Vector3 _searcharea;
    private Vector3 _deadPersonSpawn;
    private Vector3 _cop1Spawn;
    private Vector3 _cop2Spawn;
    private Vector3 _coroner1Spawn;
    private Vector3 _coroner2Spawn;
    private Vector3 _coronerVehSpawn;
    private Vector3 _copVehSpawn;
    private Vector3 _murdererLocation;
    private Blip MurderLocationBlip;
    private Blip SpawnLocationBlip;
    private readonly LHandle pursuit;
    private int storyLine = 1;
    private int _callOutMessage = 0;
    private int scenario = 0;
    private bool _Scene1 = false;
    private bool _Scene2 = false;
    private readonly bool wasClose = false;
    private bool Noticed = false;
    private readonly bool notificationDisplayed = false;
    private readonly bool hasPursuitBegun = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        _deadPersonSpawn = new Vector3(1162.029f, 2371.788f, 57.66312f);
        _cop1Spawn = new Vector3(1174.725f, 2369.399f, 57.59957f);
        _cop2Spawn = new Vector3(1167.733f, 2382.189f, 57.61982f);
        _coroner1Spawn = new Vector3(1161.486f, 2369.885f, 57.76299f);
        _coroner2Spawn = new Vector3(1165.3f, 2374.227f, 57.63049f);
        _copVehSpawn = new Vector3(1174.684f, 2375.117f, 57.6276f);
        _coronerVehSpawn = new Vector3(1165.686f, 2360.025f, 57.62796f);

        List<Vector3> list = new List<Vector3>
            {
                new(-10.93565f, -1434.329f, 31.11683f),
                new(-1.838376f, 523.2645f, 174.6274f),
                new(-801.5516f, 178.7447f, 72.83471f),
                new(-801.5516f, 178.7447f, 72.83471f),
                new(-812.7239f, 178.7438f, 76.74079f),
                new(3.542758f, 526.8926f, 170.6218f),
                new(-1155.698f, -1519.297f, 10.63272f),
                new(1392.589f, 3613.899f, 38.94194f),
                new(2435.457f, 4966.514f, 46.8106f),

        };

        // Find the nearest location that is not within the distance threshold
        foreach (Vector3 location in list.ToList())
        {
            if (GPlayer.Position.DistanceTo(location) < 80f)
            {
                list.Remove(location); // Remove locations within the distance threshold
            }
        }

        _murdererLocation = LocationChooser.ChooseNearestLocation(list);
        scenario = new Random().Next(0, 100);
        _copVeh = new Vehicle(_copCars[new Random().Next((int)_copCars.Length)], _copVehSpawn, 76.214f)
        {
            IsEngineOn = true,
            IsInteriorLightOn = true,
            IsSirenOn = true,
            IsSirenSilent = true
        };
        _coronerVeh = new Vehicle("Speedo", _coronerVehSpawn, 22.32638f);
        _coronerVeh.IsEngineOn = true;
        _coronerVeh.IsInteriorLightOn = true;
        _coronerVeh.IndicatorLightsStatus = VehicleIndicatorLightsStatus.Both;

        _deadPerson = new Ped(_deadPersonSpawn);
        _murderer = new Ped(_murdererLocation);
        _deadPerson2 = new Ped(_murderer.GetOffsetPosition(new Vector3(0, 1.8f, 0)));
        _deadPerson2.IsPersistent = true;
        _deadPerson2.BlockPermanentEvents = true;
        _cop1 = new Ped("s_m_y_sheriff_01", _cop1Spawn, 0f);
        _cop2 = new Ped("s_m_y_sheriff_01", _cop2Spawn, 0f);
        _coroner1 = new Ped("S_M_M_Doctor_01", _coroner1Spawn, 0f);
        _coroner2 = new Ped("S_M_M_Doctor_01", _coroner2Spawn, 0f);
        _deadPerson.Kill();
        _deadPerson.IsPersistent = true;
        _deadPerson.BlockPermanentEvents = true;

        _murderer.IsPersistent = true;
        _murderer.Inventory.GiveNewWeapon(new WeaponAsset(WeaponList[new Random().Next((int)WeaponList.Length)]), 500, true);
        _murderer.BlockPermanentEvents = true;
        _murderer.Health = 200;
        _murderer.Armor = 300;
        NativeFunction.CallByName<uint>("TASK_AIM_GUN_AT_ENTITY", _murderer, _deadPerson2, -1, true);
        _deadPerson.Tasks.PlayAnimation("random@arrests@busted", "idle_a", 8.0F, AnimationFlags.Loop);


        Functions.IsPedACop(_cop1);
        Functions.IsPedACop(_cop2);
        _cop1.IsInvincible = true;
        _cop1.IsPersistent = true;
        _cop1.BlockPermanentEvents = true;
        _cop1.IsInvincible = true;
        _cop1.IsPersistent = true;
        _cop1.BlockPermanentEvents = true;

        _coroner1.IsPersistent = true;
        _coroner1.IsInvincible = true;
        _coroner1.Face(_deadPerson);
        _coroner1.BlockPermanentEvents = true;
        _coroner2.IsInvincible = true;
        _coroner2.Face(_deadPerson);
        _coroner2.IsPersistent = true;
        _coroner2.BlockPermanentEvents = true;
        _coroner1.KeepTasks = false;
        _coroner2.KeepTasks = false;

        Rage.Object camera = new Rage.Object("prop_ing_camera_01", _coroner1.GetOffsetPosition(Vector3.RelativeTop * 30));
        Rage.Object camera2 = new Rage.Object("prop_ing_camera_01", _coroner2.GetOffsetPosition(Vector3.RelativeTop * 30));
        _coroner1.Tasks.PlayAnimation("amb@world_human_paparazzi@male@idle_a", "idle_a", 8.0F, AnimationFlags.Loop);
        _coroner2.Tasks.PlayAnimation("amb@medic@standing@kneel@base", "base", 8.0F, AnimationFlags.Loop);
        _cop2.Tasks.PlayAnimation("amb@world_human_cop_idles@male@idle_a", "idle_a", 1.5f, AnimationFlags.Loop);

        switch (new Random().Next(1, 2))
        {
            case 1:
                _Scene1 = true;
                break;
            case 2:
                _Scene2 = true;
                break;
        }

        ShowCalloutAreaBlipBeforeAccepting(_deadPersonSpawn, 100f);
        AddMinimumDistanceCheck(10f, _deadPerson.Position);
        switch (new Random().Next(1, 2))
        {
            case 1:
                CalloutMessage = "[SWL]~w~ Dead Body Located, ~y~detective~w~ required.";
                _callOutMessage = 1;
                break;
            case 2:
                CalloutMessage = "[SWL]~w~ Dead Body Located, ~y~detective~w~ required.";
                _callOutMessage = 2;
                break;
        }
        CalloutPosition = _deadPersonSpawn;
        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        SpawnLocationBlip = new Blip(_cop1)
        {
            Color = Color.LightGreen,
            Sprite = BlipSprite.PointOfInterest
        };
        SpawnLocationBlip.EnableRoute(Color.LightBlue);

        Functions.PlayScannerAudioUsingPosition("ATTENTION_GENERIC_01 UNITS WE_HAVE A_01 CRIME_DEAD_BODY_01 CODE3", _deadPersonSpawn);
        Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Murder Investigation", "~b~Dispatch: ~w~The police department needs a ~b~detective~w~ on scene to find and arrest the murder. Respond with ~r~Code 3");
        GameFiber.Wait(2000);
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (_cop1) _cop1.Delete();
        if (_cop2) _cop2.Delete();
        if (_coroner1) _coroner1.Delete();
        if (_murderer) _murderer.Delete();
        if (_deadPerson) _deadPerson.Delete();
        if (_deadPerson2) _deadPerson2.Delete();
        if (_coroner2) _coroner2.Delete();
        if (_copVeh) _copVeh.Delete();
        if (_coronerVeh) _coronerVeh.Delete();
        if (SpawnLocationBlip) SpawnLocationBlip.Delete();
        if (MurderLocationBlip) MurderLocationBlip.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (_cop1.DistanceTo(GPlayer) < 25f && GPlayer.IsOnFoot && !Noticed)
        {
            Game.DisplaySubtitle("Press ~y~Y~w~ to speak with the officer.", 5000);
            Functions.PlayScannerAudio("ATTENTION_GENERIC_01 OFFICERS_ARRIVED_ON_SCENE");
            _cop1.Face(GPlayer);
            if (SpawnLocationBlip) SpawnLocationBlip.Delete();
            Noticed = true;
        }
        if (_murderer.DistanceTo(GPlayer) < 25f & !Noticed)
        {
            if (MurderLocationBlip) MurderLocationBlip.Delete();
            Noticed = true;

            if (_Scene1 == true && _Scene2 == false)
            {
                _deadPerson2.Kill();
                _murderer.Tasks.FightAgainst(GPlayer);
            }
            if (_Scene2 == true && _Scene1 == false)
            {
                new RelationshipGroup("AG");
                new RelationshipGroup("VI");
                _murderer.RelationshipGroup = "AG";
                _deadPerson2.RelationshipGroup = "VI";
                Game.SetRelationshipBetweenRelationshipGroups("AG", "VI", Relationship.Hate);
                _murderer.Tasks.FightAgainstClosestHatedTarget(1000f);
                GameFiber.Wait(300);
                _murderer.Tasks.FightAgainst(GPlayer);
            }
        }
        if (_cop1.DistanceTo(GPlayer) < 2f && Game.IsKeyDown(Settings.Dialog))
        {
            _cop1.Face(GPlayer);
            Normal("Storyline started.");
            switch (storyLine)
            {
                case 1:
                    Normal("line 1");
                    Game.DisplaySubtitle("~y~Officer: ~w~Hello Detective, we called you because we have a dead body here.", 5000);
                    storyLine++;
                    break;
                case 2:
                    Normal("Line 2");
                    Game.DisplaySubtitle("~b~You: ~w~Do we have anything about the murder?", 5000);
                    storyLine++;
                    break;
                case 3:
                    Normal("Line 3");
                    Game.DisplaySubtitle("~y~Officer: ~w~Yes, we have found something of interest.", 5000);
                    storyLine++;
                    break;
                case 4:
                    Normal("Line 4");
                    if (_callOutMessage == 1)
                        Game.DisplaySubtitle("~y~Officer: ~w~We checked the cameras around here and there was a man without a mask, so our office checked the identity. Here's the results", 5000);
                    if (_callOutMessage == 2)
                        Game.DisplaySubtitle("~y~Officer: ~w~As the coroner searched the killed person, they found an ID next to the person. Here are those results.", 5000);
                    storyLine++;
                    break;
                case 5:
                    Normal("Line 5");
                    if (_callOutMessage == 1)
                    {
                        Normal("Checking possible suspect ID...");
                        Speech("~b~*You look at the ID*", 4000);
                        GameFiber.Wait(1000);
                        Functions.DisplayPedId(_murderer, true);
                        Speech("~b~*You see the results paper*", 4000);
                        GPlayer.Tasks.PlayAnimation("amb@world_human_clipboard@male@idle_a", "idle_a", 8.0F, AnimationFlags.Loop);
                        Persona pedPersona = Functions.GetPersonaForPed(_murderer);
                        string message = String.Format("Name: {0}<br>DOB: {1}<br>Gender:{2}<br>Wanted: {3}<br>{4}", pedPersona.FullName, pedPersona.Birthday.ToShortDateString(), pedPersona.Gender, pedPersona.Wanted, pedPersona.WantedInformation);
                        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~Suspect Identification", "~b~Department Records", message);
                        GameFiber.Sleep(2000);
                        GPlayer.Tasks.Clear();
                        Normal("Murder Suspect Identified");
                    }
                    if (_callOutMessage == 2)
                    {
                        Normal("Checking possible suspect ID...");
                        Speech("~b~*You look at the ID*", 4000);
                        GameFiber.Wait(1000);
                        Functions.DisplayPedId(_murderer, true);
                        Speech("~b~*You see the results paper*", 4000);
                        GPlayer.Tasks.PlayAnimation("amb@world_human_clipboard@male@idle_a", "idle_a", 8.0F, AnimationFlags.Loop);
                        Persona pedPersona = Functions.GetPersonaForPed(_murderer);
                        string message = String.Format("Name: {0}<br>DOB: {1}<br>Gender:{2}<br>Wanted: {3}<br>{4}", pedPersona.FullName, pedPersona.Birthday.ToShortDateString(), pedPersona.Gender, pedPersona.Wanted, pedPersona.WantedInformation);
                        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~Suspect Identification", "~b~Department Records", message);
                        GameFiber.Sleep(2000);
                        GPlayer.Tasks.Clear();
                        Normal("Murder Suspect Identified");
                    }
                    storyLine++;
                    break;
                case 6:
                    Normal("Line 6");
                    if (_callOutMessage == 1)
                        Game.DisplaySubtitle("~b~You: ~w~Alright, I'll check the house of the murderer. Thank you for your time, officer!", 5000);
                    if (_callOutMessage == 2)
                        Game.DisplaySubtitle("~b~You: ~w~Okay, thank you for letting me know! I'll find the murder!", 5000);
                    storyLine++;
                    Game.DisplayHelp("The ~y~Police Department~w~ is setting up the location on your GPS...", 5000);
                    GameFiber.Wait(3000);
                    Normal("Location of murder suspect revealed.");
                    Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Police Department",
                                             "~b~Detective~w~, we ~o~marked the apartment~w~ for you on the map. Search the ~y~yellow circle area~w~ on your map and try to ~y~find~w~ and ~b~arrest~w~ the ~g~murderer~w~.");
                    _searcharea = _murdererLocation.Around2D(1f, 2f);
                    MurderLocationBlip = new Blip(_searcharea, 40f);
                    MurderLocationBlip.EnableRoute(Color.Yellow);
                    MurderLocationBlip.Color = Color.Yellow;
                    MurderLocationBlip.Alpha = 0.5f;
                    break;
                default:
                    break;
            }
            Normal("Storyline Completed");
        }
        if (Game.IsKeyDown(Settings.EndCall)) End();
        if (GPlayer.IsDead) End();
        if (_murderer && _murderer.IsDead) End();
        if (_murderer && Functions.IsPedArrested(_murderer)) End();

        base.Process();
    }

    public override void End()
    {
        Normal("Cleaning up call...");
        if (_cop1) _cop1.Dismiss();
        if (_cop2) _cop2.Dismiss();
        if (_coroner1) _coroner1.Dismiss();
        if (_murderer) _murderer.Dismiss();
        if (_deadPerson) _deadPerson.Dismiss();
        if (_deadPerson2) _deadPerson2.Dismiss();
        if (_coroner2) _coroner2.Dismiss();
        if (_copVeh) _copVeh.Dismiss();
        if (_coronerVeh) _coronerVeh.Dismiss();
        if (SpawnLocationBlip) SpawnLocationBlip.Delete();
        if (MurderLocationBlip) MurderLocationBlip.Delete();
        Game.DisplayNotification("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Murder Investigation", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");
        Normal("Call cleared.");
        base.End();
    }
}