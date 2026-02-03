using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : DamagingAbility
{
    public GameObject ultBullet;
    new void Start()
    {
        base.Start();
        loaded = true;
        reloadtime = 15f;
        manaCost = 250;
    }
    protected override void AbilityAction()
    {
        GameObject ultInstance = Instantiate(ultBullet, player.transform.position + player.transform.forward*2 + new Vector3(0f,2f,0f), player.transform.rotation);
            ultInstance.GetComponent<Damage>().SetProperties(GetDamageValues());
            StartCoroutine("reload");
            reloader.shoot();
            player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(50+(player.levelsys.getLevel()-5)*4.5f, 0, CombatUtils.Team.Player, true, true);
    }
}
