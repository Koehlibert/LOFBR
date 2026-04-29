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
                return new DamageInfo(0.5f * (16 + 4 * OwnerLevelSys.GetLevel()), Handler.Owner.Team, true, false);
            }
            else
            {
                return new DamageInfo(16 + 4 * OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, false);
            }
        }
        else
        {
            return new DamageInfo(25, Handler.Owner.Team, true, false);
        }
    }
    protected override GameObject CreateBullet()
    {
        GameObject bullet = BulletFactory.Instance.CreatePoisonBullet(Handler.Owner, true, Bone);
        bullet.GetComponent<BulletBehaviourFollowing>().OnBulletHit += (DamageableEntity tmp) => CreatePoisonStatus(tmp);
        return bullet;
    }
    private float GetPoisonDamage()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return 0.5f * (8f + 1.5f * OwnerLevelSys.GetLevel());
            }
            else
            {
                return 8f + 1.5f * OwnerLevelSys.GetLevel();
            }
        }
        else
        {
            return 8;
        }
    }
    private void CreatePoisonStatus(DamageableEntity tmp)
    {
        if (tmp is CharacterBehaviour characterBehaviour)
        {
            PoisonEffect poisonEffect = characterBehaviour.gameObject.AddComponent<PoisonEffect>();
            poisonEffect.Init(5, GetPoisonDamage());
            characterBehaviour.AddStatusEffect(poisonEffect);
        }
    }
}