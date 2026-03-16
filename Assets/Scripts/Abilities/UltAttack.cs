using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : ShootBasic
{
    public GameObject ultBullet;
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected List<Ability> DisabledAbilities;
    protected int NEnemiesToTrigger = 4;
    protected float DistanceToCheck = 30;
    protected override void AdditionalInit()
    {
        offset = new Vector3(0, 1, 0);
        Handler.ClosestFinder.StartTrackingDist(DistanceToCheck);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsInteractive)
        {
            reloader = HUD.Instance.GetReload(HUD.Instance.UltReloader);
        }
    }
    protected override IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.2f);
    }
    protected override IEnumerator Shootanim()
    {
        DisabledAbilities = Handler.DisableOtherAbilities(this);
        StartCoroutine(Handler.movementAI.LockMovement(1.6f));
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
        Handler.ReenableOtherAbilities(DisabledAbilities);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(50 + (OwnerLevelSys.GetLevel() - 5) * 6.5f, 0, Handler.Owner.Team, true, true);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateUltBullet(Handler.Owner, false, Bone);
    }
    protected override void AICheck()
    {
        Debug.Log(Handler.ClosestFinder.GetEnemiesInDist());
        if (Handler.ClosestFinder.GetEnemiesInDist() >= NEnemiesToTrigger)
        {
            Handler.movementAI.MovementState = AIUtils.MovementState.IsStanding;
            Handler.SetEvenLookDirection(Handler.closestEnemy.transform.position);
            if (loaded)
            {
                Handler.FinalAction = AbilityAction;
            }
        }
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15f, 250, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills});
    }
}
