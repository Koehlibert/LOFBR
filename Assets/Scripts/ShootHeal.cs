using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHeal : DamagingAbility
{
    public GameObject bullet;
    Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    public GameObject bulletinstance;
    new void Start()
    {
        base.Start();
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
            bulletinstance.GetComponent<DestroyAfterTimeHeal>().DelayedDestroy();
        }
    }
    void FixedUpdate()
    {
        if (bulletinstance)
        {
            bulletinstance.transform.position = player.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + player.transform.forward;
        }
    }
    private IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.4f);
        loaded = true;
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + player.transform.forward, player.transform.rotation);
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + player.transform.forward, player.transform.rotation);
        loaded = true;
        player.transform.position = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z);
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        player.animator.Play("Default", 0, 0f);
    }
    private IEnumerator Shootanim()
    {
        player.animator.Play("Shoot", 0, 0f);
        yield return new WaitForSeconds(0.1f);
        if (bulletinstance == null) yield break;
        bulletinstance.GetComponent<Damage>().SetProperties(GetDamageValues());
        bulletinstance.transform.rotation = transform.rotation;
        bulletinstance.GetComponent<DestroyAfterTimeHeal>().DelayedDestroy();
        bulletinstance = null;
        StartCoroutine("Resetanim");
    }

    protected override void AbilityAction()
    {
        StartCoroutine(nameof(Shootanim));
        reloader.shoot();
        StartCoroutine(nameof(Reload));
        player.manasys.useMana(manaCost);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(40 + 5 * player.levelsys.getLevel(), 0, CombatUtils.Team.Player, false, false);
    }
}