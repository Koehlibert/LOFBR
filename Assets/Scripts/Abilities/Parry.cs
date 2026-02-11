using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{
    private GameObject parryCollider;
    private float duration;
    new void Start()
    {
        base.Start();
        duration = .6f;
        loaded = true;
        parryCollider = FindAnyObjectByType<ParryColliderBehaviour>().gameObject;
        parryCollider.SetActive(false);
        player = FindAnyObjectByType<PlayerController>();
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
