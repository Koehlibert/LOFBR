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
        
    }
    protected override void AdditionalInit()
    {
        if (Handler.Owner is MainPlayerBehaviour)
            AttackDistance = 25f;
        soundType = AbilitySoundType.Shoot;
    }
    protected override DamageInfo GetDamageValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(0.5f * (16 + 4 * OwnerLevelSys.GetLevel()), 0.5f * (8f + 1.5f * OwnerLevelSys.GetLevel()), Handler.Owner.Team, true, false);
            }
            else
            {
                return new DamageInfo(16 + 4 * OwnerLevelSys.GetLevel(), 8f + 1.5f * OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, false);
            }
        }
        else 
        {
            return new DamageInfo(25, 8, Handler.Owner.Team, true, false);
        }
    }
    protected override GameObject CreateBullet()
    {
        return BulletFactory.Instance.CreatePoisonBullet(Handler.Owner, true, Bone);
    }
}