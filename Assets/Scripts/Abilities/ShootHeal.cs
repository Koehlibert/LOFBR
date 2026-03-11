using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHeal : ShootBasic
{
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SecondaryReloader);
    }
    protected override HumanBodyBones Bone => HumanBodyBones.LeftLowerLeg;
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(40 + 5 * player.Levelsys.GetLevel(), 0, CombatUtils.Team.Player, false, false);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateHealingBullet(player, true, Bone);
    }
}