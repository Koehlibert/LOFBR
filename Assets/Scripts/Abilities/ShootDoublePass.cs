using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ShootDoublePass : ShootBasic
{
    protected override HumanBodyBones Bone => HumanBodyBones.RightLowerLeg;
    private bool IsPassing = false;
    private float Duration = 3.5f;
    private float timeCounter;
    private int PassStage;
    private Vector3 StartPosition;
    private Vector3 ballTarget;
    private float StepSize = 5f;
    float startTime;
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
        return BulletFactory.Instance.CreateBullet(Handler.Owner, false, Bone);
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
        timeCounter = 0;
        PassStage = 0;
        startTime = Time.time;
        IsPassing = true;
        Handler.DisableOtherAbilities(Duration, this);
        Handler.Owner.animator.SetFloat("moveX", 0);
        Handler.Owner.animator.SetFloat("moveZ", 0);
        movementAI.LockMovementAI(Duration);
        CharacterFactory.Instance.CreatePhantom(Handler.Owner, Duration, StepSize);
        StartPosition = Handler.Owner.animator.GetBoneTransform(Bone).position + Handler.Owner.transform.forward * 0.1f;
        StartPosition.y += 0.15f;
        SetBallTarget();
        bulletinstance.GetComponent<BulletBehaviour>().Activate(GetDamageValues());
        bulletinstance.GetComponent<BulletBehaviour>().UnsetRB();
    }
    protected override void InteractiveCheck()
    {
        if (!IsPassing)
            base.InteractiveCheck();
        else
            BehaviourWhilePassing();
    }
    private void BehaviourWhilePassing()
    {
        timeCounter += Time.deltaTime;
        MoveBall();
        if (timeCounter > (Duration / 5) * (PassStage + 1) - 0.15f)
        {
            NextStage();
        }
    }
    private void NextStage()
    {
        PassStage++;
        if (PassStage % 2 == 0)
        {
            Handler.Owner.animator.SetTrigger("Shoot");
            StartCoroutine(DelayedMoveTarget());
            PlaySound();
        }
        if (PassStage == 5)
        {
            IsPassing = false;
            Destroy(bulletinstance);
        }
    }
    private IEnumerator DelayedMoveTarget()
    {
        yield return new WaitForSeconds(0.15f);
        bulletinstance.GetComponent<BulletBehaviour>().Activate(GetDamageValues());
        SetBallTarget();
    }
    private void MoveBall()
    {
        float t = Mathf.PingPong((Time.time - startTime) / (Duration / 5), 1f);
        bulletinstance.transform.position = Vector3.Lerp(StartPosition, ballTarget, t);
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
                return new DamageInfo(PassStagePercent() * 0.5f * (15 + 3 * OwnerLevelSys.GetLevel()), PassStagePercent() * 0.5f * (8f + 1f * OwnerLevelSys.GetLevel()), Handler.Owner.Team, true, false);
            }
            else
            {
                return new DamageInfo(PassStagePercent() * 15 + 3 * OwnerLevelSys.GetLevel(), PassStagePercent() * 8f + 1f * OwnerLevelSys.GetLevel(), Handler.Owner.Team, true, false);
            }
        }
        else
        {
            return new DamageInfo(PassStagePercent() * 20, PassStagePercent() * 6, Handler.Owner.Team, true, false);
        }
    }
    protected override void OnDeactivate(Ability callingAbility)
    {
        base.OnDeactivate(callingAbility);
        IsPassing = false;
    }
    private void SetBallTarget()
    {
        ballTarget = StartPosition + (StepSize * (PassStage + 2) + 2.5f) * Handler.Owner.transform.forward;
    }
}