using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltAttack : DamagingAbility
{
    public GameObject bullet;
    new void Start()
    {
        base.Start();
        loaded = true;
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    protected override void AbilityAction()
    {
        GameObject wave = Instantiate(bullet, player.transform.position + new Vector3(0f, 0.4f, 0f), player.transform.rotation);
        wave.GetComponent<Damage>().SetProperties(GetDamageValues());
        StartCoroutine("reload");
        reloader.shoot();
        player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(70 + (player.levelsys.getLevel() - 2) * 6, 0, player.Team, true, false);
    }
}
