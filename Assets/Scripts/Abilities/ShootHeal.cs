using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShootHeal : ShootBasic
{
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SecondaryReloader);
    }
    protected override void AdditionalInit()
    {
        if (Handler.Owner is MainPlayerBehaviour)
            AttackDistance = 15f;
        soundType = AbilitySoundType.Shoot;
    }
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(40 + 5 * OwnerLevelSys.GetLevel(), 0, Handler.Owner.Team, false, false);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateHealingBullet(Handler.Owner, true, Bone);
    }
    protected override void AICheck()
    {
        GameObject closestHurtFriendly = Handler.ClosestFinder.FindClosestHurtFriendlies();
        if (closestHurtFriendly == null)
            return;
        if (CombatUtils.InRange(Handler.Owner.gameObject, closestHurtFriendly, AttackDistance))
        {
            Handler.movementAI.MovementState = AIUtils.MovementState.IsStanding;
            Handler.SetEvenLookDirection(closestHurtFriendly.transform.position);
            if (loaded)
            {
                Handler.FinalAction = AbilityAction;
            }
        }
        else
        {
            Handler.movementAI.MovementState = AIUtils.MovementState.IsFollowingTarget;
            Handler.movementAI.Speedup = 0.75f;
            Handler.SetEvenLookDirection(closestHurtFriendly.transform.position);
        }
    }
}