using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Dash : Ability
{
    private float dashDistance = 12;
    private bool IsSubscribed = false;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 2f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.CheckGeneralSkills });
    }
    protected override void AbilityAction()
    {
        Vector3 dir = movementAI.GetMovementDirection();
        if (dir.magnitude > 0)
        {
            StartCoroutine(DashAnim(dir));
        }
    }
    private IEnumerator DashAnim(Vector3 dir)
    {
        StartCoroutine(Reload());
        Handler.LockMovementAI(0.225f);
        Handler.Owner.animator.SetTrigger("Dash");
        movementAI.Speedup = 1.25f;
        StartCoroutine(movementAI.SetForcemovement(0.15f));
        yield return new WaitForSeconds(0.15f);
        if (dir.magnitude > dashDistance || IsInteractive)
            dir = dir.normalized * dashDistance;
        Handler.Owner.transform.position += dir;
        base.AbilityAction();
        if (!IsInteractive)
        {
            IsSubscribed = false;
        }
        yield return new WaitForSeconds(0.15f);
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.AlternativePressed;
    }
    protected override void AICheck()
    {
        if (loaded && !IsSubscribed && CheckManaCost())
        {
            IsSubscribed = true;
            movementAI.CouldDash += AbilityAction;
        }
    }
}
