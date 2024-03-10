// Author: Scottywonderful
// Created: 28th Feb 2024
// Version: 0.4.6.0

namespace SWLCallouts.Callouts;

[CalloutInfo("[SWL] Reports of a Person With A Knife", CalloutProbability.Medium)]
public class SWLPersonWithAKnife : Callout
{
    private readonly string[] pedList = new string[] { "A_F_M_SouCent_01", "A_F_M_SouCent_02", "A_M_Y_Skater_01", "A_M_M_FatLatin_01", "A_M_M_EastSA_01", "A_M_Y_Latino_01", "G_M_Y_FamDNF_01",
                                              "G_M_Y_FamCA_01", "G_M_Y_BallaSout_01", "G_M_Y_BallaOrig_01", "G_M_Y_BallaEast_01", "G_M_Y_StrPunk_02", "S_M_Y_Dealer_01", "A_M_M_RurMeth_01",
                                              "A_M_M_Skidrow_01", "A_M_Y_MexThug_01", "G_M_Y_MexGoon_03", "G_M_Y_MexGoon_02", "G_M_Y_MexGoon_01", "G_M_Y_SalvaGoon_01", "G_M_Y_SalvaGoon_02",
                                              "G_M_Y_SalvaGoon_03", "G_M_Y_Korean_01", "G_M_Y_Korean_02", "G_M_Y_StrPunk_01" };
    private Ped _suspect;
    private Vector3 SpawnPoint;
    private Vector3 searcharea;
    private Blip Blip;
    private LHandle pursuit;
    private int scenario = 0;
    private bool hasBegunAttacking = false;
    private bool isArmed = false;
    private bool hasPursuitBegun = false;

    public override bool OnBeforeCalloutDisplayed()
    {
        scenario = new Random().Next(0, 100);
        SpawnPoint = World.GetNextPositionOnStreet(GPlayer.Position.Around(1000f));
        ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 100f);
        CalloutMessage = "[SWL]~w~ Reports of a Person With a Knife.";
        CalloutPosition = SpawnPoint;
        Functions.PlayScannerAudioUsingPosition("ATTENTION_ALL_UNITS ASSAULT_WITH_AN_DEADLY_WEAPON CIV_ASSISTANCE IN_OR_ON_POSITION", SpawnPoint);
        Game.LogTrivial("SWLCallouts - Person With A Knife callout offered.");

        return base.OnBeforeCalloutDisplayed();
    }

    public override bool OnCalloutAccepted()
    {
        Game.LogTrivial("SWLCallouts - Person With A Knife callout accepted.");
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "~y~Person With a Knife", "~b~Dispatch: ~w~Try to arrest the Suspect. Respond with ~r~Code 3");
        Functions.PlayScannerAudio("UNITS_RESPOND_CODE_03_01");

        _suspect = new Ped(pedList[new Random().Next((int)pedList.Length)], SpawnPoint, 0f)
        {
            BlockPermanentEvents = true,
            IsPersistent = true
        };
        _suspect.Tasks.Wander();

        searcharea = SpawnPoint.Around2D(1f, 2f);
        Blip = new Blip(searcharea, 80f)
        {
            Color = Color.Orange,
            Alpha = 0.5f
        };
        Blip.EnableRoute(Color.Orange);
        return base.OnCalloutAccepted();
    }

    public override void OnCalloutNotAccepted()
    {
        if (Blip) Blip.Delete();
        if (_suspect) _suspect.Delete();
        base.OnCalloutNotAccepted();
    }

    public override void Process()
    {
        GameFiber.StartNew(delegate
        {
            if (_suspect.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !isArmed)
            {
                _suspect.Inventory.GiveNewWeapon("WEAPON_KNIFE", 500, true);
                isArmed = true;
            }
            if (_suspect && _suspect.DistanceTo(GPlayer.GetOffsetPosition(Vector3.RelativeFront)) < 18f && !hasBegunAttacking)
            {
                if (scenario > 40)
                {
                    _suspect.KeepTasks = true;
                    _suspect.Tasks.FightAgainst(GPlayer);
                    hasBegunAttacking = true;
                    switch (new Random().Next(1, 3))
                    {
                        case 1:
                            Game.DisplaySubtitle("~r~Suspect: ~w~I do not want to live anymore!", 4000);
                            break;
                        case 2:
                            Game.DisplaySubtitle("~r~Suspect: ~w~Go away! - I'm not going back to the psychiatric hospital!", 4000);
                            break;
                        case 3:
                            Game.DisplaySubtitle("~r~Suspect: ~w~I'm not going back to the psychiatric hospital!", 4000);
                            break;
                        default: break;
                    }
                    GameFiber.Wait(2000);
                }
                else
                {
                    if (!hasPursuitBegun)
                    {
                        pursuit = Functions.CreatePursuit();
                        Functions.AddPedToPursuit(pursuit, _suspect);
                        Functions.SetPursuitIsActiveForPlayer(pursuit, true);
                        hasPursuitBegun = true;
                    }
                }
            }
            if (GPlayer.IsDead) End();
            if (Game.IsKeyDown(Settings.EndCall)) End();
            if (_suspect && _suspect.IsDead) End();
            if (_suspect && Functions.IsPedArrested(_suspect)) End();
        }, "Person With a Knife [SWLCallouts]");
        base.Process();
    }

    public override void End()
    {
        if (_suspect) _suspect.Dismiss();
        if (Blip) Blip.Delete();
        Game.DisplayNotification(SWLicon, SWLicon, "~w~SWLCallouts", "[SWL] ~y~Welfare Check", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
        Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH ALL_UNITS_CODE4 NO_FURTHER_UNITS_REQUIRED");

        Game.LogTrivial("SWLCallouts - Person With A Knife cleanup.");
        base.End();
    }
}