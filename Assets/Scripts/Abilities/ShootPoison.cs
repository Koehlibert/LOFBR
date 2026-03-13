using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoison : ShootBasic
{
   
    protected override HumanBodyBones Bone => HumanBodyBones.RightLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.PrimaryPressed;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.PrimaryReloader);
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(16 + 4 * OwnerLevelSys.GetLevel(), 4f + 4f + 1.5f * OwnerLevelSys.GetLevel(), CombatUtils.Team.Player, true, false);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreatePoisonBullet(Handler.Owner, true, Bone);
    }
}