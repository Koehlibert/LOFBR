using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : ShootBasic
{
    public GameObject ultBullet;
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected int NEnemiesToTrigger = 4;
    protected float DistanceToCheck = 30;
    private InDistanceTracker inDistanceTracker;
    protected override void AdditionalInit()
    {
        offset = new Vector3(0, 1, 0);
        if (!IsInteractive)
            inDistanceTracker = Handler.ClosestFinder.StartTrackingDist(DistanceToCheck, true);
    }
    protected override IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.2f);
    }
    protected override IEnumerator Shootanim()
    {
        StartCoroutine(Handler.DisableOtherAbilities(1.6f, this));
        movementAI.LockMovementAI(1.6f);
        Handler.Owner.animator.Play("Backflip", 0, 0f);
        yield return new WaitForSeconds(0.6f);
        bulletinstance = CreateBullet();
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageValues());
        StartCoroutine("Resetanim");
    }
    protected override IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(1f);
        Handler.Owner.animator.Play("Default", 0, 0f);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(50 + (OwnerLevelSys.GetLevel() - 5) * 6.5f, 0, Handler.Owner.Team, true, true, false);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateUltBullet(Handler.Owner, false, Bone);
    }
    protected override void AICheck()
    {
        if (loaded && inDistanceTracker.GetOverCount(NEnemiesToTrigger))
        {
            movementAI.SetMovementState(AIUtils.MovementState.IsStanding);
            movementAI.SetEvenLookDirection(Handler.closestEnemy.transform.position);
            SetFinalAction();
        }
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15f, 60, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills });
    }
}
