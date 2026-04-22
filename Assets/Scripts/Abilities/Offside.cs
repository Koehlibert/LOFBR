using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offside : DamagingAbility
{
    private InDistanceTracker inDistanceTracker;
    float DistanceToCheck = 30;
    int NEnemiesToTrigger = 5;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(225, 25, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void AdditionalInit()
    {
        if (!IsInteractive)
            inDistanceTracker = Handler.ClosestFinder.StartTrackingDist(DistanceToCheck, true);
    }
    protected override void AbilityAction()
    {
        StartCoroutine(Reload());
        StartCoroutine(StartMover());
        Handler.LockMovementAI(1.5f);
        StartCoroutine(Handler.DisableOtherAbilities(1.5f, this));
        base.AbilityAction();
    }
    private IEnumerator StartMover()
    {
        Handler.Owner.transform.LookAt(MasterScript.Instance.GetOpponentBase(Handler.Owner.Team).transform);
        Handler.Owner.animator.SetTrigger("CallRef");
        yield return new WaitForSeconds(0.25f);
        CharacterFactory.Instance.CreateReferee(Handler.Owner.Team, Handler.Owner.transform.position.z);
        yield return new WaitForSeconds(0.35f);
        BulletFactory.Instance.CreateMover(Handler.Owner, GetDamageValues());
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressedThisFrame;
    }
    protected override void AICheck()
    {
        if (loaded)
        {
            if (CheckManaCost())
            {
                bool condition1 = CharacterTracker.Instance.GetFurthestEnemy(Handler.Owner.Team).transform.position.z >
                                    MasterScript.Instance.GetOpponentSpawnZ(CombatUtils.GetOpposingTeam(Handler.Owner.Team)) * 0.75f;
                bool condition2 = inDistanceTracker.GetOverCount(NEnemiesToTrigger);
                bool condition3 = Handler.Owner.hpsys.healthDisplay() < 0.25;
                float prob = Mathf.Clamp01(0 + (condition1 ? 0.4f : 0) + (condition2 ? 0.35f : 0) + (condition3 ? 0.2f : 0));
                if (UnityEngine.Random.value < prob)
                {
                    SetFinalAction();
                }
            }
        }
    }
    protected override DamageInfo GetDamageValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(0.5f * (8 + OwnerLevelSys.GetLevel() * 2), OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, true, false);
            }
            else
            {
                return new DamageInfo(8 + OwnerLevelSys.GetLevel() * 2, OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, true, false);
            }
        }
        else
        {
            return new DamageInfo(10, 1, Handler.Owner.Team, false, true, false);
        }
    }
}
