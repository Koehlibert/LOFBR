using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootLeftBasic : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SecondaryReloader);
        IsInteractive = Handler?.Owner is MainPlayerBehaviour;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(34 + 7 * player.levelsys.getLevel(), 0, CombatUtils.Team.Player, true);
    }
}