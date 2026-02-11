using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : DamagingAbility
{
    public GameObject bullet;
    private float duration = .5f;
    private bool attacking;
    private Vector3 dir;
    private GameObject meleeCollider;
    private float speedup = 1.5f;
    new void Start()
    {
        base.Start();
        loaded = true;
        meleeCollider = FindAnyObjectByType<MeleeCollider>().gameObject;
        meleeCollider.SetActive(false);
    }
    void OnDisable()
    {
        Reset();
    }
    void FixedUpdate()
    {
        if (attacking)
        {
            player.aIHandler.MovementDirection = dir;
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
    }
    private void Shootanim()
    {
        player.animator.SetTrigger("Melee");
        float clipLength = 1 / 2f;
        duration = clipLength;
        StartCoroutine(player.aIHandler.movementAI.LockMovement(duration));
        StartCoroutine(player.aIHandler.SetForcemovement(duration));
        StartCoroutine(player.LockView(duration));
        player.aIHandler.movementAI.Speedup = speedup;
        StartCoroutine("resetanim");
    }
    public new void Reset()
    {
        loaded = true;
        attacking = false;
    }
    protected override void AbilityAction()
    {
        Shootanim();
        reloader.shoot();
        StartCoroutine("reload");
        player.manasys.useMana(manaCost);
        dir = player.transform.forward;
        attacking = true;
        meleeCollider.SetActive(true);
        meleeCollider.GetComponent<Damage>().SetProperties(GetDamageValues());
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.PrimaryPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(35 + player.levelsys.getLevel() * 3, 0, player.Team, true, false);
    }
}