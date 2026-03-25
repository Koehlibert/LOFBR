using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{
    private GameObject ParryCollider;
    private float duration;
    protected override void AdditionalInit()
    {
        duration = .6f;
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(12, 1.75f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SecondaryReloader);
    }
    private IEnumerator autoDisable()
    {
        yield return new WaitForSeconds(duration);
        Destroy(ParryCollider);
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
        ParryCollider = BulletFactory.Instance.CreateParryCollider(Handler.Owner);
        StartCoroutine("autoDisable");
        StartCoroutine("reload");
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
}
