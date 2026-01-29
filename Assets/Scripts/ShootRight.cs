using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRight : Ability
{
    public GameObject bullet;
    Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    public AudioSource soundsource;
    public GameObject bulletinstance;
    private Rigidbody bulletrig;
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
            bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        }
    }
    void FixedUpdate()
    {
        if (bulletrig)
        {
            bulletrig.transform.position = player.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + player.transform.forward;
        }
        if (Input.GetButtonDown("Primary") && (loaded) && player.manasys.checkCost(manaCost))
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
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + player.transform.forward, player.transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = Instantiate(bullet, player.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + player.transform.forward, player.transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
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
        bulletinstance.GetComponent<Damage>().SetProperties(34 + 7 * player.levelsys.getLevel(), 0, CombatUtils.Team.Player, true, true);
        bulletrig.AddForce(player.transform.forward * 2250);
        bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        bulletrig = null;
        StartCoroutine("Resetanim");
    }
}