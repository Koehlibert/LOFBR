using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootDoublePass : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.RightLowerLeg;
    private float Duration = 3.5f;
    private int PassStage;
    private Vector3 StartPosition;
    private float StepSize = 5f;
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
        GameObject bullet = BulletFactory.Instance.CreateBullet(Handler.Owner, false, Bone);
        bullet.GetComponent<BulletBehaviour>().OnBulletHit += (DamageableEntity tmp) => CreatePoisonStatus(tmp);
        return bullet;
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
    }
    protected override void AdditionalInit()
    {
        soundType = AbilitySoundType.Shoot;
        bulletinstance = CreateBullet();
    }
    protected override IEnumerator Shootanim()
    {
        Handler.Owner.animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.15f);
        PassStage = 0;
        Handler.DisableOtherAbilities(Duration, this);
        Handler.Owner.animator.SetFloat("moveX", 0);
        Handler.Owner.animator.SetFloat("moveZ", 0);
        movementAI.LockMovementAI(Duration);
        CharacterFactory.Instance.CreatePhantom(Handler.Owner, Duration, StepSize);
        StartPosition = Handler.Owner.animator.GetBoneTransform(Bone).position + Handler.Owner.transform.forward * 0.1f;
        StartPosition.y += 0.15f;
        bulletinstance.GetComponent<BulletBehaviour>().Activate(GetDamageValues());
        bulletinstance.GetComponent<BulletBehaviour>().UnsetRB();
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