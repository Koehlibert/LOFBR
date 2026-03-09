using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootRightBasic : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.RightLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.PrimaryPressed;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsInteractive)
        {
            reloader = HUD.Instance.GetReload(HUD.Instance.PrimaryReloader);
            IsInteractive = Handler?.Owner is MainPlayerBehaviour;
        }
    }
    protected override DamageInfo GetDamageValues()
    {
        if (IsInteractive)
        {
            return new DamageInfo(34 + 7 * player.levelsys.getLevel(), 0, CombatUtils.Team.Player, true);
        }
        else
        {
            return new DamageInfo(40, 0, CombatUtils.Team.Player, true);
        }
    }
    protected override void AICheck()
    {
        if (Handler.AIState == AIUtils.AIState.CheckShoot || Handler.AIState == AIUtils.AIState.Attacking)
        {
            if (Handler.distanceToClosest < 10)
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
}