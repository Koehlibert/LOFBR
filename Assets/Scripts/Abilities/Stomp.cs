using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : DamagingAbility
{
    public GameObject bullet;
    new void Start()
    {
        base.Start();
        loaded = true;
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    protected override void AbilityAction()
    {
        StartCoroutine("Shootanim");
        reloader.shoot();
        StartCoroutine("Reload");
        player.manasys.useMana(manaCost);
        StartCoroutine("Reload");
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
    private IEnumerator Shootanim()
    {
        StartCoroutine(player.LockMovement(0.95f));
        player.animator.Play("Stomp", 0, 0f);
        yield return new WaitForSeconds(0.7f);
        GameObject wave = Instantiate(bullet, player.transform.position + new Vector3(0f, 0.4f, 0f), player.transform.rotation);
        wave.GetComponent<Damage>().SetProperties(GetDamageValues());
        //soundsource.Play();
        StartCoroutine("Resetanim");
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        player.animator.Play("Default", 0, 0f);
    }
}
