using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{
    private GameObject parryCollider;
    private float duration;

    public override string InputString => "AttackSecondary";

    new void Start()
    {
        base.Start();
        duration = .6f;
        loaded = true;
        parryCollider = FindAnyObjectByType<ParryColliderBehaviour>().gameObject;
        parryCollider.SetActive(false);
        player = FindAnyObjectByType<PlayerController>();
    }
    protected override void Update()
    {
        if (!player)
        {
            parryCollider.SetActive(false);
        }
        if (Input.GetButtonDown("Secondary") && (loaded) && player.manasys.checkCost(manaCost))
        {
            base.Update();
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
}
