using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offside : Ability
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
        Handler.LockMovementAI(1.5f);
        /* Handler.Owner.animator.SetBool("Pushed", true);
        StartCoroutine(ResetAnimation(1.5f)); */
        StartCoroutine(Handler.DisableOtherAbilities(1.5f, this));
        base.AbilityAction();
    }
    private IEnumerator ResetAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        Handler.Owner.animator.SetBool("Pushed", false);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressedThisFrame;
    }
    protected override void AICheck()
    {
    }
}
