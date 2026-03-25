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
    private bool IsAttacking = false;
    private float AttackDistance = 8.5f;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(5, 1.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
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
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(duration);
        Destroy(MeleeCollider);
        attacking = false;
        Handler.ReenableOtherAbilities();
        IsAttacking = false;
    }
    private void Shootanim()
    {
        Handler.DisableOtherAbilities(this);
        Handler.Owner.animator.SetTrigger("Melee");
        float clipLength = 1 / 2f;
        duration = clipLength;
        StartCoroutine(Handler.movementAI.LockMovement(duration));
        StartCoroutine(Handler.SetForcemovement(duration));
        StartCoroutine(Handler.movementAI.LockView(duration));
        Handler.movementAI.Speedup = speedup;
        StartCoroutine("Resetanim");
    }
    public new void Reset()
    {
        loaded = true;
        attacking = false;
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
        IsAttacking = true;
        MeleeCollider = BulletFactory.Instance.CreateMeleeCollider(Handler.Owner);
        MeleeCollider.GetComponent<Damage>().SetProperties(GetDamageValues());
        Shootanim();
        StartCoroutine("Reload");
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
    protected override void AICheck()
    {
        if (Handler.distanceToClosest < AttackDistance)
        {
            Handler.SetEvenLookDirection(Handler.closestEnemyNoTower.transform.position);
            if (loaded)
            {
                Handler.movementAI.MovementState = AIUtils.MovementState.IsFollowingTarget;    
                Handler.FinalAction = AbilityAction;
            }
            else if (!IsAttacking)
            {
                Handler.movementAI.MovementState = AIUtils.MovementState.IsCircling;
                Handler.movementAI.Speedup = 0.9f;
            }
        }
        else
        {
            Handler.movementAI.MovementState = AIUtils.MovementState.IsFollowingTarget;
            Handler.movementAI.Speedup = 1.25f;
            Handler.SetEvenLookDirection(Handler.closestEnemyNoTower.transform.position);
        }
    }
}