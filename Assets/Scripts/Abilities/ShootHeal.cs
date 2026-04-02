using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShootHeal : ShootBasic
{
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
            movementAI.SetMovementState(AIUtils.MovementState.IsStanding);
            movementAI.SetEvenLookDirection(closestHurtFriendly.transform.position);
            if (loaded)
            {
                SetFinalAction();
            }
        }
        else
        {
            movementAI.SetMovementState(AIUtils.MovementState.IsFollowingTarget);
            movementAI.Speedup = 0.75f;
            movementAI.SetEvenLookDirection(closestHurtFriendly.transform.position);
        }
    }
}