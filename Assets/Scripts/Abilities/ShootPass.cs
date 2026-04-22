using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootPass : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.RightLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressedThisFrame;
    }
}