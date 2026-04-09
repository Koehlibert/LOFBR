using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootLeftBasic : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressedThisFrame;
    }
    protected override DamageInfo GetDamageValues()
    {
        if (IsInteractive)
        {
            return new DamageInfo(34 + 7 * OwnerLevelSys.GetLevel(), 0, Handler.Owner.Team, true);
        }
        else
        {
            return new DamageInfo(40, 0, Handler.Owner.Team, true);
        }
    }
}