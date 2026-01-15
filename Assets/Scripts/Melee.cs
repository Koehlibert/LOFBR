using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    public GameObject bullet;
    private float duration = .7f;
    private bool attacking;
    private Vector3 dir;
    private GameObject meleeCollider;
    private MasterScript master;
    new void Start()
    {
        base.Start();
        loaded = true;
        meleeCollider = FindObjectOfType<MeleeCollider>().gameObject;
        meleeCollider.SetActive(false);
        master = FindObjectOfType<MasterScript>();
    }
    void OnEnable()
    {
        player = GetComponent<PlayerController>();
    }
    void OnDisable()
    {
        reset();
    }
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Primary")&&(loaded)&&player.manasys.checkCost(manaCost))
        {
            StartCoroutine("shootanim");
            reloader.shoot();
            StartCoroutine("reload");
            player.manasys.useMana(manaCost);
            dir = player.transform.forward;
            attacking = true;
            meleeCollider.SetActive(true);
            meleeCollider.GetComponent<Damage>().SetDamage(35+player.levelsys.getLevel() * 3,0);
            StartCoroutine(player.LockMovement(duration));
            StartCoroutine(player.LockView(duration));
        }
        if (attacking)
        {
            transform.Translate(dir*player.GetSpeed()*Time.deltaTime,Space.World);
            Vector3 temp = transform.position;
            temp.y = 0;
            temp.x = Mathf.Clamp(temp.x, master.lowerAreaLimitX, master.upperAreaLimitX);
            float lowerZ = Mathf.Min(master.friendlySpawn.getZPos(), master.enemySpawn.getZPos());
            float upperZ = Mathf.Max(master.friendlySpawn.getZPos(), master.enemySpawn.getZPos());
            temp.z = Mathf.Clamp(temp.z, lowerZ, upperZ);
            transform.position = temp;
        }
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
        player.transform.position = new Vector3(player.transform.position.x, 0.7f, player.transform.position.z);
    }
    private IEnumerator resetanim()
    {
        yield return new WaitForSeconds(duration);
        meleeCollider.SetActive(false);
        attacking = false;
        player.animator.Play("Default",0,0f);
    }
    private IEnumerator shootanim()
    {
        player.animator.Play("Melee",0,0f);
        yield return new WaitForSeconds(0.1f);
        //soundsource.Play();
        StartCoroutine("resetanim");
    }
    public new void reset()
    {
        loaded = true;
        attacking = false;

    }
}