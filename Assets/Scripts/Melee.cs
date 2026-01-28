using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability
{
    public GameObject bullet;
    private float duration = .5f;
    private bool attacking;
    private Vector3 dir;
    private GameObject meleeCollider;
    private MasterScript master;
    private float speedup = 1.5f;
    new void Start()
    {
        base.Start();
        loaded = true;
        meleeCollider = FindAnyObjectByType<MeleeCollider>().gameObject;
        meleeCollider.SetActive(false);
        master = FindAnyObjectByType<MasterScript>();
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
            meleeCollider.GetComponent<Damage>().SetProperties(35 + player.levelsys.getLevel() * 3, 0, player.Team, false, true);
        }
        if (attacking)
        {
            player.MoveCharacter(dir, speedup);
        }
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
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
        AnimatorClipInfo[] clipInfo = player.animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = 1 / 3.5f;
        duration = clipLength;
        StartCoroutine(player.LockMovement(duration));
        StartCoroutine(player.LockView(duration));
        yield return new WaitForSeconds(0.01f);
        StartCoroutine("resetanim");
    }
    public new void reset()
    {
        loaded = true;
        attacking = false;

    }
}