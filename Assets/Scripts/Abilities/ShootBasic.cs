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
        return BulletFactory.Instance.CreateBullet(player, true, Bone);
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
        StartCoroutine("Firstbullet");
        Reset();
    }
    void OnDisable()
    {
        if (bulletinstance)
        {
            Destroy(bulletinstance);
        }
    }
    private IEnumerator Firstbullet()
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
    private IEnumerator Shootanim()
    {
        if (bulletinstance == null)
        {
            yield break;
        }
        player.animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.15f);
        soundsource.Play();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
    }
    protected override void AbilityAction()
    {
        StartCoroutine("Shootanim");
        reloader.shoot();
        StartCoroutine("Reload");
        player.manasys.useMana(manaCost);
    }
}