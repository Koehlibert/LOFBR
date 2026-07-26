using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootDoublePass : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.RightLowerLeg;
    private float Duration = 0.35f;
    private int PassStage;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(10f, 7.5f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.Retreating });
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressedThisFrame;
    }
    protected override GameObject CreateBullet()
    {
        GameObject bullet = BulletFactory.Instance.CreatePossessingBullet(Handler.Owner, GetDamageValues(), GetHealingValues(), true, Bone);
        bullet.GetComponent<BulletBehaviour>().OnBulletHit += (DamageableEntity tmp) => CreatePoisonStatus(tmp);
        return bullet;
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
    }
    protected override void AdditionalInit()
    {
        PassStage = 0;
        soundType = AbilitySoundType.Shoot;
        bulletinstance = CreateBullet();
    }
    protected override IEnumerator Shootanim()
    {
        Handler.Owner.animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.15f);
        PassStage = 0;
        StartCoroutine(Handler.DisableOtherAbilities(Duration, this));
        Handler.Owner.animator.SetFloat("moveX", 0);
        Handler.Owner.animator.SetFloat("moveZ", 0);
        movementAI.LockMovementAI(Duration);
        bulletinstance.GetComponent<BulletBehaviourPossessing>().Shoot(GetDamageValues());
        bulletinstance = null;
        /* bulletinstance.GetComponent<BulletBehaviourPossessing>().Activate(GetDamageValues());
        bulletinstance.GetComponent<BulletBehaviourPossessing>().UnsetRB(); */
    }
    private float PassStagePercent()
    {
        return (PassStage + 1) / 5;
    }
    protected override DamageInfo GetDamageValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(PassStagePercent() * 0.5f * (15 + 3 * OwnerLevelSys.GetLevel()), Handler.Owner.Team, true, false);
            }
            else
            {
                return new DamageInfo(PassStagePercent() * 15 + 3 * OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, false);
            }
        }
        else
        {
            return new DamageInfo(PassStagePercent() * 20, Handler.Owner.Team, true, false);
        }
    }
    protected DamageInfo GetHealingValues()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return new DamageInfo(PassStagePercent() * 0.5f * (20 + 2 * OwnerLevelSys.GetLevel()), Handler.Owner.Team, true, false);
            }
            else
            {
                return new DamageInfo(PassStagePercent() * 20 + 2.5f * OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, false);
            }
        }
        else
        {
            return new DamageInfo(PassStagePercent() * 15, Handler.Owner.Team, true, false);
        }
    }
    protected override void OnDeactivate(Ability callingAbility)
    {
        base.OnDeactivate(callingAbility);
    }
    private float GetPoisonDamage()
    {
        if (Handler.Owner is MainPlayerBehaviour)
        {
            if (Handler.Owner is MirrorImageBehaviour)
            {
                return PassStagePercent() * 0.5f * (8f + 1f * OwnerLevelSys.GetLevel());
            }
            else
            {
                return PassStagePercent() * 8f + 1f * OwnerLevelSys.GetLevel();
            }
        }
        else
        {
            return PassStagePercent() * 6;
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