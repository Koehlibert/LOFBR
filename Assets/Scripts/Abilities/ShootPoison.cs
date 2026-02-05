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
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(16 + 4 * player.levelsys.getLevel(), 4f + 4f + 1.5f * player.levelsys.getLevel(), CombatUtils.Team.Player, true, false);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreatePoisonBullet(player, true, Bone);
    }
}