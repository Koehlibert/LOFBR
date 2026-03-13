using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootBasic : DamagingAbility
{
    [SerializeField] GameObject bullet;
    protected Vector3 offset = new Vector3(0, -0.5f, 1.5f);
    [SerializeField] AudioSource soundsource;
    protected GameObject bulletinstance;
    protected Coroutine reloadCoroutine;
    protected float attackDistance;
    protected virtual GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateBullet(Handler.Owner, true, Bone);
    }
    protected abstract HumanBodyBones Bone { get; }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(5f, 1.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void AdditionalInit()
    {
        attackDistance = 10f;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Firstbullet());
        Reset();
    }
    void OnDisable()
    {
        if (bulletinstance)
        {
            Destroy(bulletinstance);
        }
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
    }
    protected virtual IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.2f);
        bulletinstance = CreateBullet();
        loaded = true;
    }
    protected virtual IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        yield return new WaitUntil(() => Handler.movementAI.MoveLock == false);
        bulletinstance = CreateBullet();
        loaded = true;
    }
    protected virtual IEnumerator Shootanim()
    {
        if (bulletinstance == null)
        {
            yield break;
        }
        Handler.Owner.animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.15f);
        soundsource?.Play();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
    }
    protected override void AbilityAction()
    {
        StartCoroutine(Shootanim());
        reloadCoroutine = StartCoroutine(Reload());
        if (IsInteractive)
        {
            reloader.shoot();
            OwnerManaSys.useMana(manaCost);
        }
    }
    protected override void AICheck()
    {
        if (Handler.distanceToClosest < attackDistance)
        {
            Handler.movementAI.MovementState = AIUtils.MovementState.IsStanding;
            Handler.SetEvenLookDirection(Handler.closestEnemy.transform.position);
            if (loaded)
            {
                Handler.FinalAction = AbilityAction;
            }
        }
        else
        {
            Handler.movementAI.MovementState = AIUtils.MovementState.IsFollowingTarget;
            Handler.movementAI.Speedup = 0.75f;
            Handler.SetEvenLookDirection(Handler.closestEnemy.transform.position);
        }
    }
}