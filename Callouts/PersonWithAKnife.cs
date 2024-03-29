// Author: Scottywonderful
// Created: 28th Feb 2024
// Version: 0.4.8.9

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
        _scenario = new Random().Next(0, 100);
        _spawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(1000f));
        ShowCalloutAreaBlipBeforeAccepting(_spawnPoint, 100f);
        CalloutMessage = "[SWL]~w~ Reports of a Person With a Knife.";
        CalloutPosition = _spawnPoint;
        Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", _spawnPoint);
        Normal("PersonWithAKnife callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Normal("PersonWithAKnife callout accepted.");
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "~y~Person With a Knife", "~b~Dispatch: ~w~Try to arrest the Suspect. Respond with ~r~Code 3");
        Functions.PlayScannerAudio("UNITS_RESPOND_CODE_03_01");

        _suspect = new Ped(_pedList[new Random().Next((int)_pedList.Length)], _spawnPoint, 0f)
        {
            BlockPermanentEvents = true,
            IsPersistent = true
        };
        _suspect.Tasks.Wander();

        _searcharea = _spawnPoint.Around2D(1f, 2f);
        _blip = new Blip(_searcharea, 80f)
        {
            Color = Color.Orange,
            Alpha = 0.5f
        };
        _blip.EnableRoute(Color.Orange);
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        Normal("PersonWithAKnife callout NOT accepted.");
        if (_blip) _blip.Delete();
        if (_suspect) _suspect.Delete();
        Functions.PlayScannerAudio(CalloutNoAnswer.PickRandom());
        Normal("PersonWithAKnife callout entities removed.");
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        if (_suspect.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !_isArmed)
        {
            _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
            _isArmed = true;
        }
        if (_suspect && _suspect.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !_hasBegunAttacking)
        {
            if (_scenario > 50)
            {
                _suspect.KeepTasks = true;
                _suspect.Tasks.FightAgainst(GPlayer);
                _hasBegunAttacking = true;
                Speech(PWAKSuspectSpeech.PickRandom(), 4000);
                switch (new Random().Next(1, 3))
                {
                    case 1:
                        Speech("~r~Suspect: ~w~I do not want to live anymore!", 4000);
                        break;
                    case 2:
                        Speech("~r~Suspect: ~w~Go away! - I'm not going back to the psychiatric hospital!", 4000);
                        break;
                    case 3:
                        Speech("~r~Suspect: ~w~I'm not going back to the psychiatric hospital!", 4000);
                        break;
                    default: break;
                }
                GameFiber.Sleep(2000);
            }
            else
            {
                if (!_hasPursuitBegun)
                {
                    _pursuit = Functions.CreatePursuit();
                    Functions.AddPedToPursuit(_pursuit, _suspect);
                    Functions.SetPursuitIsActiveForPlayer(_pursuit, true);
                    _hasPursuitBegun = true;
                }
            }
        }
        if (GPlayer.IsDead) End();
        if (Game.IsKeyDown(Settings.EndCall)) End();
        if (_suspect && _suspect.IsDead) End();
        if (_suspect && Functions.IsPedArrested(_suspect)) End();
        base.Process();
    }

    public override void End()
    {
        if (_suspect) _suspect.Dismiss();
        if (_blip) _blip.Delete();
        NotifyP("3dtextures", "mpgroundlogo_cops", "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Normal("PersonWithAKnife cleanup.");
        base.End();
    }
}