using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttackHeal : Ability
{
    public GameObject ultBullet;

    public override string InputString => "Skill";

    new void Start()
    {
        base.Start();
        loaded = true;
        player = GetComponent<PlayerController>();
    }
    private IEnumerator reload()
    {
        loaded = false;
        Instantiate(ultBullet,player.transform);
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }

    protected override void AbilityAction()
    {
        GameObject ultInstance = Instantiate(ultBullet, player.transform.position + player.transform.forward*2 + new Vector3(0f,2f,0f), player.transform.rotation);
            StartCoroutine("reload");
            reloader.shoot();
            player.manasys.useMana(manaCost);
    }
}
