using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLeft : DamagingAbility
{
    public GameObject bullet;
    Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    public AudioSource soundsource;
    public GameObject bulletinstance;
    new void Start()
    {
        manaCost = 5;
        loaded = true;
        reloadtime = 1.5f;
    }
    void OnEnable()
    {
        base.Start();
        StartCoroutine("Firstbullet");
        Reset();
    }
    void OnDisable()
    {
        if (bulletinstance)
        {
            bulletinstance.GetComponent<BulletBehaviour>().DelayedDestroy();
        }
    }
    private IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.4f);
        loaded = true;
        bulletinstance = BulletFactory.Instance.CreateBullet(player, true, HumanBodyBones.LeftLowerLeg);
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = BulletFactory.Instance.CreateBullet(player, true, HumanBodyBones.LeftLowerLeg);
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        player.animator.Play("Default", 0, 0f);
    }
    private IEnumerator Shootanim()
    {
        if (bulletinstance == null)
        {
            yield break;
        }
        player.animator.Play("Shoot", 0, 0f);
        yield return new WaitForSeconds(0.1f);
        soundsource.Play();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
        StartCoroutine("Resetanim");
    }

    protected override void AbilityAction()
    {
        StartCoroutine("Shootanim");
        reloader.shoot();
        StartCoroutine("Reload");
        player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(34 + 7 * player.levelsys.getLevel(), 0, CombatUtils.Team.Player, true);
    }
}