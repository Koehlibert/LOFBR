using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootBasic : DamagingAbility
{
    [SerializeField] GameObject bullet;
    protected Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    [SerializeField] AudioSource soundsource;
    protected GameObject bulletinstance;
    protected virtual GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateBullet(Handler.Owner, true, Bone);
    }
    protected abstract HumanBodyBones Bone { get; }
    protected override void Start()
    {
        base.Start();
        manaCost = 5;
        loaded = false;
        reloadtime = 1.5f;
    }
    protected override void OnEnable()
    {
        StartCoroutine(Firstbullet());
        Reset();
    }
    void OnDisable()
    {
        if (bulletinstance)
        {
            Destroy(bulletinstance);
        }
    }
    protected virtual IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.2f);
        bulletinstance = CreateBullet();
        loaded = true;
    }
    protected virtual IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = CreateBullet();
        loaded = true;
    }
    protected virtual IEnumerator Shootanim()
    {
        if (bulletinstance == null)
        {
            yield break;
        }
        Handler.Owner.animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.15f);
        soundsource?.Play();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
    }
    protected override void AbilityAction()
    {
        StartCoroutine(Shootanim());
        StartCoroutine(Reload());
        if (IsInteractive)
        {
            reloader.shoot();
            player.manasys.useMana(manaCost);
        }
    }
}