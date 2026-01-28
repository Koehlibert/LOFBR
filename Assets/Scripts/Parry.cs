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
    void Update()
    {
        if(!player)
        {
            parryCollider.SetActive(false);
        }
        if (Input.GetButtonDown("Secondary")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            parryCollider.SetActive(true);
            StartCoroutine("autoDisable");
            reloader.shoot();
            StartCoroutine("reload");
            player.manasys.useMana(manaCost);
        }
    }
    private IEnumerator autoDisable()
    {
        yield return new WaitForSeconds(duration);
        parryCollider.SetActive(false);
    }
}
