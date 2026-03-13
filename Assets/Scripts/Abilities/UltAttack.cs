using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : ShootBasic
{
    public GameObject ultBullet;
    private ShootRightBasic ShootRight;
    private ShootLeftBasic ShootLeft;
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected override void AdditionalInit()
    {
        offset = new Vector3(0, 1, 0);
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
        ShootLeft = Handler.Owner?.GetComponent<ShootLeftBasic>();
        ShootRight = Handler.Owner?.GetComponent<ShootRightBasic>();
    }
    protected override IEnumerator Shootanim()
    {
        ShootLeft.enabled = false;
        ShootRight.enabled = false;
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
        ShootLeft.enabled = true;
        ShootRight.enabled = true;
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(50 + (OwnerLevelSys.GetLevel() - 5) * 6.5f, 0, CombatUtils.Team.Player, true, true);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateUltBullet(Handler.Owner, false, Bone);
    }
    protected override void AICheck()
    {
        if (Handler.ClosestFinder.GetActiveEnemyNumber() >= 3)
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
