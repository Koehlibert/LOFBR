using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : DamagingAbility
{
    public GameObject bullet;
    private float duration = .6f;
    private bool attacking;
    private Vector3 dir;
    private GameObject MeleeCollider;
    private float speedup = 1.5f;
    private float AttackDistance = 8.5f;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(8, 2.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
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
    public override void Activate()
    {
        base.Activate();
        attacking = false;
    }
    public override void Deactivate()
    {
        base.Deactivate();
        if (MeleeCollider != null)
            Destroy(MeleeCollider);
    }
    protected override void InteractiveCheck()
    {
        if (attacking)
        {
            movementAI.SetMovementDirection(dir);
        }
        else
            base.InteractiveCheck();
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(duration);
        Destroy(MeleeCollider);
        attacking = false;
    }
    public new void Reset()
    {
        if (MeleeCollider != null)
            Destroy(MeleeCollider);
        loaded = true;
        attacking = false;
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
        MeleeCollider = BulletFactory.Instance.CreateMeleeCollider(Handler.Owner);
        MeleeCollider.GetComponent<Damage>().SetProperties(GetDamageValues());
        StartCoroutine(Handler.DisableOtherAbilities(duration, this));
        Handler.Owner.animator.SetTrigger("Melee");
        movementAI.LockMovementAI(duration);
        movementAI.Speedup = speedup;
        StartCoroutine(movementAI.SetForcemovement(duration));
        StartCoroutine(Resetanim());
        StartCoroutine(Reload());
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
        if (attacking)
        {
            movementAI.SetMovementDirection(dir);
            //movementAI.SetMovementState(AIUtils.MovementState.IsFollowingTarget);
        }
        else
        {
            movementAI.SetEvenLookDirection(Handler.closestEnemyNoTower.transform.position);
            if (Handler.distanceToClosest < AttackDistance)
            {
                if (loaded)
                {
                    movementAI.SetMovementState(AIUtils.MovementState.IsFollowingTarget);
                    SetFinalAction();
                }
                else
                {
                    movementAI.SetMovementState(AIUtils.MovementState.IsCircling);
                    movementAI.Speedup = 0.9f;
                }
            }
            else
            {
                movementAI.SetMovementState(AIUtils.MovementState.IsFollowingTarget);
                movementAI.Speedup = 1.25f;
            }
        }
    }
}