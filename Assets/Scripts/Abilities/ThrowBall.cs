using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : DamagingAbility
{
    protected Vector3 offset = new(0, 7, 0);
    protected Coroutine reloadCoroutine;
    protected float AttackDistance = 25f;
    protected virtual GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateBullet(Handler.Owner, true, HumanBodyBones.RightHand);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(5f, 1.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void AdditionalInit()
    {
        reloadtime = 1.25f;
    }
    protected override IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    private IEnumerator Shootanim()
    {
        Handler.Owner.animator.Play("Throw", 0, 0f);
        yield return new WaitForSeconds(0.15f);
        GameObject bulletinstance = BulletFactory.Instance.CreateBullet(Handler.Owner, true, HumanBodyBones.RightHand);
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues(), 1250);
    }
    protected override DamageInfo GetDamageValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(0.5f * (50 + 5 * OwnerLevelSys.GetLevel()), Handler.Owner.Team, false, false, true);
            }
            else
            {
                return new DamageInfo(50 + 5 * OwnerLevelSys.GetLevel(), Handler.Owner.Team, false, false, true);
            }
        }
        else 
        {
            return new DamageInfo(45, Handler.Owner.Team, false, false, true);
        }
    }
    protected override void AbilityAction()
    {
        StartCoroutine(Shootanim());
        StartCoroutine(Reload());
        base.AbilityAction();
    }
    protected override void AICheck()
    {
        movementAI.SetEvenLookDirection(Handler.closestEnemy.transform.position);
        if (Handler.distanceToClosest < AttackDistance && loaded)
        {
            SetFinalAction();
        }
    }

    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.PrimaryPressed;
    }
}