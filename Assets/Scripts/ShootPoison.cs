using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoison : Ability
{
    public GameObject bullet;
    Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    public AudioSource soundsource;
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
            bulletinstance.GetComponent<DestroyAfterTimePoison>().DelayedDestroy();
        }
    }
    void FixedUpdate()
    {
        if (bulletinstance)
        {
            bulletinstance.transform.position = player.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + player.transform.forward;
        }
        if (Input.GetButtonDown("Secondary") && (loaded) && player.manasys.checkCost(manaCost))
        {
            StartCoroutine("Shootanim");
            reloader.shoot();
            StartCoroutine("Reload");
            player.manasys.useMana(manaCost);
        }
    }
    private IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.4f);
        loaded = true;
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + player.transform.forward, player.transform.rotation);
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + player.transform.forward, player.transform.rotation);
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        player.animator.Play("Default", 0, 0f);
    }
    private IEnumerator Shootanim()
    {
        if (bulletinstance == null) yield break;
        player.animator.Play("Shoot", 0, 0f);
        yield return new WaitForSeconds(0.1f);
        soundsource.Play();
        bulletinstance.GetComponent<Damage>().SetProperties(16 + 4 * player.levelsys.getLevel(), 4f + 4f + 1.5f * player.levelsys.getLevel(), CombatUtils.Team.Player, true, true);
        bulletinstance.transform.rotation = transform.rotation;
        bulletinstance.GetComponent<DestroyAfterTimePoison>().DelayedDestroy();
        bulletinstance = null;
        StartCoroutine("Resetanim");
    }
}