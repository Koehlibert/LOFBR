using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : Ability
{
    private GameObject ParryCollider;
    private float duration = .6f;
    private GameObject BulletDetector;
    protected override void AdditionalInit()
    {
        if (!IsInteractive)
        {
            BulletDetector = BulletFactory.Instance.CreateBulletDetector(Handler.Owner, 1);
            BulletDetector.GetComponent<DetectBulletsCollisionHandler>().BulletsDetected += TryParry;
        }
    }
    public void TryParry()
    {
        if (loaded && Handler.Owner.isActiveAndEnabled && CheckManaCost())
        {
            SetFinalAction();
        }
    }
    public void OnDisable()
    {
        if (ParryCollider != null)
            Destroy(ParryCollider);
    }
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(12, 1.75f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot });
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        reloader = HUD.Instance.GetReload(HUD.Instance.SecondaryReloader);
    }
    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(duration);
        Destroy(ParryCollider);
    }
    public override void Activate()
    {
        base.Activate();
    }
    public override void Deactivate()
    {
        base.Deactivate();
        if (ParryCollider != null)
            Destroy(ParryCollider);
    }
    protected override void AbilityAction()
    {
        base.AbilityAction();
        movementAI.LockMovementAI(duration);
        StartCoroutine(Handler.DisableOtherAbilities(duration, this));
        ParryCollider = BulletFactory.Instance.CreateParryCollider(Handler.Owner);
        Handler.Owner.animator.SetTrigger("Parry");
        StartCoroutine(AutoDisable());
        StartCoroutine(Reload());
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressed;
    }
    protected override void AICheck()
    {
    }
}
