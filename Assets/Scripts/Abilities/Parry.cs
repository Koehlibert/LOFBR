using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{
    private GameObject parryCollider;
    private float duration;
    protected override void AdditionalInit()
    {
        duration = .6f;
        parryCollider = FindAnyObjectByType<ParryColliderBehaviour>().gameObject;
        parryCollider.SetActive(false);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(8, 3.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SecondaryReloader);
    }
    protected override void InteractiveCheck()
    {
        if (!player)
        {
            parryCollider.SetActive(false);
        }
        if (InputPressed() && (loaded) && player.manasys.checkCost(manaCost))
        {
            base.InteractiveCheck();
        }
    }
    private IEnumerator autoDisable()
    {
        yield return new WaitForSeconds(duration);
        parryCollider.SetActive(false);
    }
    protected override void AbilityAction()
    {
        parryCollider.SetActive(true);
        StartCoroutine("autoDisable");
        reloader.shoot();
        StartCoroutine("reload");
        player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
}
