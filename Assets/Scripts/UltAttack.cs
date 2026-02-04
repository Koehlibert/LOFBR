using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAttack : ShootBasic
{
    public GameObject ultBullet;

    protected override HumanBodyBones Bone => HumanBodyBones.Head;
    new void Start()
    {
        offset = new Vector3(0, 1, 0);
        loaded = true;
        reloadtime = 15f;
        manaCost = 250;
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.UltPressed;
    }
    protected override DamageInfo GetDamageValues()
    {
        return new DamageInfo(50+(player.levelsys.getLevel()-5)*6.5f, 0, CombatUtils.Team.Player, true, true);
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreateUltBullet(player, false, Bone);
    }
}
