using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Dash : Ability
{
    public GameObject shield;
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
            if (dir.magnitude > dashDistance || IsInteractive)
                dir = dir.normalized * dashDistance;
            Handler.Owner.transform.position += dir;
            StartCoroutine("Reload");
            base.AbilityAction();
            if (!IsInteractive)
            {
                IsSubscribed = false;
            }
        }
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
