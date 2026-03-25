using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : DamagingAbility
{
    public GameObject bullet;
    private float duration = .5f;
    private bool attacking;
    private Vector3 dir;
    private GameObject MeleeCollider;
    private float speedup = 1.5f;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(5, 1.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking });
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.PrimaryReloader);
    }
    void OnDisable()
    {
        Reset();
    }
    void FixedUpdate()
    {
        if (attacking)
        {
            Handler.MovementDirection = dir;
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
        Destroy(MeleeCollider);
        attacking = false;
    }
    private void Shootanim()
    {
        Handler.Owner.animator.SetTrigger("Melee");
        float clipLength = 1 / 2f;
        duration = clipLength;
        StartCoroutine(Handler.movementAI.LockMovement(duration));
        StartCoroutine(Handler.SetForcemovement(duration));
        StartCoroutine(Handler.movementAI.LockView(duration));
        Handler.movementAI.Speedup = speedup;
        StartCoroutine("resetanim");
    }
    public new void Reset()
    {
        loaded = true;
        attacking = false;
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
        MeleeCollider = BulletFactory.Instance.CreateMeleeCollider(Handler.Owner);
        MeleeCollider.GetComponent<Damage>().SetProperties(GetDamageValues());
        Shootanim();
        StartCoroutine("reload");
        dir = Handler.Owner.transform.forward;
        attacking = true;
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.PrimaryPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(35 + OwnerLevelSys.GetLevel() * 3, 0, Handler.Owner.Team, true, false);
    }
}