using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootBasic : DamagingAbility
{
    protected Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    protected GameObject bulletinstance;
    protected Coroutine reloadCoroutine;
    protected float AttackDistance = 10f; 
    protected virtual GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateBullet(Handler.Owner, true, Bone);
    }
    protected override DamageInfo GetDamageValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(0.5f * (34 + 7 * OwnerLevelSys.GetLevel()), 0, Handler.Owner.Team, true);
            }
            else
            {
                return new DamageInfo(34 + 7 * OwnerLevelSys.GetLevel(), 0, Handler.Owner.Team, true);
            }
        }
        else 
        {
            return new DamageInfo(40, 0, Handler.Owner.Team, true);
        }
    }
    protected abstract HumanBodyBones Bone { get; }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(5f, 1.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.Retreating });
    }
    protected override void AdditionalInit()
    {
        if (Handler.Owner is MainPlayerBehaviour)
            AttackDistance = 20f;
        soundType = AbilitySoundType.Shoot;
        if (bulletinstance != null)
        {
            Debug.Log("init issue");
            Debug.Break();
        }
        bulletinstance = CreateBullet();
    }
    public void SetAttackDistance(float attackDistance)
    {
        AttackDistance = attackDistance;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        //StartCoroutine(Firstbullet());
    }
    void OnDisable()
    {
        OnDeactivate(this);
    }
    void OnDestroy()
    {
        OnDeactivate(this);     
    }
    protected override void OnDeactivate(Ability callingAbility)
    {
        if (bulletinstance && (callingAbility is not ShootBasic))
        {
            Destroy(bulletinstance);
            bulletinstance = null;
        }
    }
    public override void Activate()
    {
        base.Activate();
        if (loaded && bulletinstance == null)
        {
            bulletinstance = CreateBullet();
        }
        //StartCoroutine(Firstbullet());
    }
    protected override IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        yield return new WaitUntil(() => movementAI.MoveLock == false);
        if (IsActive)
            bulletinstance = CreateBullet();
        loaded = true;
    }
    protected virtual IEnumerator Shootanim()
    {
        Handler.Owner.animator.SetTrigger("Shoot");
        StartCoroutine(Handler.DisableOtherAbilities(0.3f, this));
        yield return new WaitForSeconds(0.15f);
        if (bulletinstance == null)
        {
            yield break;
        }
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
        bulletinstance = null;
    }
    protected override void AbilityAction()
    {
        StartCoroutine(Shootanim());
        reloadCoroutine = StartCoroutine(Reload());
        base.AbilityAction();
        PlaySound();
    }
    protected override void AICheck()
    {
        if (Handler.distanceToClosest < AttackDistance)
        {
            movementAI.SetMovementState(AIUtils.MovementState.IsStanding);
            movementAI.SetEvenLookDirection(Handler.closestEnemy.transform.position);
            if (loaded)
            {
                SetFinalAction();
            }
        }
        else
        {
            movementAI.SetMovementState(AIUtils.MovementState.IsFollowingTarget);
            movementAI.Speedup = 0.75f;
            movementAI.SetEvenLookDirection(Handler.closestEnemy.transform.position);
        }
    }
}