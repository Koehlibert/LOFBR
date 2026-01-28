using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoison : Ability
{
    public GameObject bullet;
    Vector3 offset = new Vector3(0,-0.5f,1.5f);
    public GameObject bulletinstance;
    new void Start()
    {
        base.Start();
    }
    void OnEnable()
    {
        base.Start();
        StartCoroutine("firstbullet");
        reset();
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
        if (Input.GetButtonDown("Secondary")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            StartCoroutine("shootanim");
            reloader.shoot();
            StartCoroutine("reload");
            player.manasys.useMana(manaCost);
        }
    }
    private IEnumerator firstbullet()
    {
        yield return new WaitForSeconds(.4f);
        loaded = true;
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + player.transform.forward, player.transform.rotation);
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + player.transform.forward, player.transform.rotation);
        loaded = true;
        player.transform.position = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z);
    }
    private IEnumerator resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        player.animator.Play("Default",0,0f);
    }
    private IEnumerator shootanim()
    {
        player.animator.Play("Shoot",0,0f);
        yield return new WaitForSeconds(0.1f);
        if (bulletinstance == null) yield break;
        bulletinstance.GetComponent<Damage>().SetProperties(16 + 4*player.levelsys.getLevel(),4f + 4f + 1.5f * player.levelsys.getLevel(), CombatUtils.Team.Player, true, true);
        bulletinstance.transform.rotation = transform.rotation;
        bulletinstance.GetComponent<DestroyAfterTimePoison>().DelayedDestroy();
        bulletinstance = null;
        StartCoroutine("resetanim");
    }
}