// Author: Scottywonderful
// Created: 28th Feb 2024
// Version: 0.4.9.0

#region

#endregion

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of a Person With A Knife", CalloutProbability.Medium)]
public class SWLPersonWithAKnife : Callout
{
    private readonly string[] _pedList = new string[] { "A_F_M_SouCent_01", "A_F_M_SouCent_02", "A_M_Y_Skater_01", "A_M_M_FatLatin_01", "A_M_M_EastSA_01", "A_M_Y_Latino_01", "G_M_Y_FamDNF_01",
                                              "G_M_Y_FamCA_01", "G_M_Y_BallaSout_01", "G_M_Y_BallaOrig_01", "G_M_Y_BallaEast_01", "G_M_Y_StrPunk_02", "S_M_Y_Dealer_01", "A_M_M_RurMeth_01",
                                              "A_M_M_Skidrow_01", "A_M_Y_MexThug_01", "G_M_Y_MexGoon_03", "G_M_Y_MexGoon_02", "G_M_Y_MexGoon_01", "G_M_Y_SalvaGoon_01", "G_M_Y_SalvaGoon_02",
                                              "G_M_Y_SalvaGoon_03", "G_M_Y_Korean_01", "G_M_Y_Korean_02", "G_M_Y_StrPunk_01" };
    private Ped _suspect;
    private Vector3 _spawnPoint;
    private Vector3 _searcharea;
    private Blip _blip;
    private LHandle _pursuit;
    private int _scenario = 0;
    private bool _hasBegunAttacking = false;
    private bool _isArmed = false;
    private bool _hasPursuitBegun = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        Normal("Choosing nearest location for callout...");
        _scenario = new Random().Next(0, 100);
        _spawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(750f));
        Normal("Displaying callout blip...");
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        Normal("Blip loaded.");
        Normal("Displaying callout message...");
        CalloutMessage = "[SWL]~w~ Reports of a Person With a Knife.";
        CalloutPosition = _spawnPoint;
        Normal("Play scanner audio...");
        Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", _spawnPoint);
        Normal("PersonWithAKnife callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("PersonWithAKnife callout accepted.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Person With a Knife", "~b~Dispatch: ~w~Try to arrest the Suspect. Respond with ~r~Code 3");
        Normal("Play respond code 3 audio...");
        Functions.PlayScannerAudio("CODE3");

        Normal("Spawning suspect...");
        _suspect = new Ped(_pedList[new Random().Next((int)_pedList.Length)], _spawnPoint, 0f)
        {
            BlockPermanentEvents = true,
            IsPersistent = true
        };
        _suspect.Tasks.Wander();
        Normal("Spawned suspect.");

        Normal("Loading spawnpoint...");
        _searcharea = _spawnPoint.Around2D(1f, 2f);
        _blip = new Blip(_searcharea, 80f)
        {
            Color = Color.Orange,
            Alpha = 0.5f
        };
        _blip.EnableRoute(Color.Orange);
        Normal("Spawnpoint loaded, waypoint added.");
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        Normal("PersonWithAKnife callout NOT accepted.");
        if (_blip) _blip.Delete();
        if (_suspect) _suspect.Delete();
        Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
        Normal("PersonWithAKnife callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (_suspect.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !_isArmed)
        {
            Normal("Arming suspect...");
            _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
            _isArmed = true;
            Normal("Suspect armed.");
        }
        if (_suspect && _suspect.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !_hasBegunAttacking)
        {
            if (_scenario > 50)
            {
                Normal("Setting suspect to attack officer...");
                _suspect.KeepTasks = true;
                _suspect.Tasks.FightAgainst(GPlayer);
                _hasBegunAttacking = true;
                Normal("Attack assigned.");
                Normal("Suspect yells at cop...");
                Speech(PWAKSuspectSpeech.PickRandom(), 4000);
                GameFiber.Sleep(4000);
                Normal("Officer response...");
                Speech("~b~You: ~y~*YELLS* ~w~Put the knife down now!", 2000);

                Normal("Is AI Backup activated?");
                if (Settings.ActivateAIBackup)
                {
                    Normal("AI Backup enabled. Requesting for assistance...");
                    GameFiber.Sleep(2000);
                    Speech("~b~You: ~w~Dispatch, person is confirmed armed with a knife, requesting an additional unit.", 4000);
                    GameFiber.Sleep(4000);
                    Normal("Playing dispatch radio response...");
                    Functions.PlayScannerAudioUsingPosition("OFFICER_REQUESTING_BACKUP IN_OR_ON_POSITION", _spawnPoint);
                    Normal("Sending backup unit...");
                    Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
                    Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Code3, LSPD_First_Response.EBackupUnitType.LocalUnit);
                }
                else
                {
                    Settings.ActivateAIBackup = false;
                    Normal("AI Backup disabled. NO backup responding.");
                }
                GameFiber.Sleep(2000);
            }
            else
            {
                if (!_hasPursuitBegun)
                {
                    Normal("Setting suspect to run from officer...");
                    _pursuit = Functions.CreatePursuit();
                    Functions.AddPedToPursuit(_pursuit, _suspect);
                    Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                    _hasPursuitBegun = true;
                    Normal("Fleeing assigned.");

                    Normal("Is AI Backup activated?");
                    if (Settings.ActivateAIBackup)
                    {
                        Normal("AI Backup enabled. Requesting for assistance...");
                        GameFiber.Sleep(2000);
                        Speech("~b~You: ~w~Dispatch, person is confirmed armed with a knife and fleeing at this time.", 4000);
                        GameFiber.Sleep(4000);
                        Normal("Playing dispatch radio response...");
                        Functions.PlayScannerAudioUsingPosition("OFFICER_REQUESTING_BACKUP IN_OR_ON_POSITION", _spawnPoint);
                        Normal("Sending backup unit...");
                        Functions.PlayScannerAudio(AIOfficerEnroute.PickRandom());
                        Functions.RequestBackup(_spawnPoint, LSPD_First_Response.EBackupResponseType.Pursuit, LSPD_First_Response.EBackupUnitType.LocalUnit);
                    }
                    else
                    {
                        Settings.ActivateAIBackup = false;
                        Normal("AI Backup disabled. NO backup responding.");
                    }
                    GameFiber.Sleep(2000);
                }
            }
        }
        if (Game.IsKeyDown(Settings.EndCall) || GPlayer.IsDead) End();
        if (_suspect.Exists() && (Functions.IsPedArrested(_suspect) || _suspect.IsDead)) End();
        base.Process();
    }

    public override void End()
    {
        Normal("Call ended, cleaning up call...");
        if (_suspect) _suspect.Dismiss();
        if (_blip) _blip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~b~DISPATCH", "~w~[SWL] ~y~Person With A Knife", PWAKDispatchCode4.PickRandom());
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Normal("PersonWithAKnife cleanup.");
        base.End();
    }
}