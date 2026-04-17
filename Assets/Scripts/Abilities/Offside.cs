using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offside : DamagingAbility
{
    public GameObject Referee;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(10, 2, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void AdditionalInit()
    {

    }
    protected override void AbilityAction()
    {
        StartCoroutine(Reload());
        Referee = CharacterFactory.Instance.CreateReferee(Handler.Owner.transform.position.z);
        StartCoroutine(StartMover());
        Handler.LockMovementAI(1.5f);
        StartCoroutine(Handler.DisableOtherAbilities(1.5f, this));
        base.AbilityAction();
    }
    private IEnumerator StartMover()
    {
        yield return new WaitForSeconds(0.5f);
        BulletFactory.Instance.CreateMover(Handler.Owner, GetDamageValues());
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressedThisFrame;
    }
    protected override void AICheck()
    {
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
