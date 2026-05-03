using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR;

public class PossessDebug : SelectionAbility
{
    private float MarkDistance = 30f;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 2f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.Retreating });
    }
    protected override void AdditionalInit()
    {
        reloadtime = 12;
    }
    protected override void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(PlayerInputRouter.Instance.Look);
        CharacterBehaviour characterBehaviour = null;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            characterBehaviour = hit.collider.gameObject.GetComponentInParent<CharacterBehaviour>();
            if (characterBehaviour != null && characterBehaviour.Team == Handler.Owner.Team)
            {
                characterBehaviour.MarkHealthbar();
            }
        }
        if (ConfirmInputPressed())
        {
            ToggleSelecting();
            if (characterBehaviour != null && characterBehaviour.Team == Handler.Owner.Team)
            {
                AbilityAction(characterBehaviour);
            }
        }
    }
    protected override void AbilityAction()
    {
        if (!IsInteractive)
        {
            AbilityAction(Handler.closestEnemy.GetComponent<CharacterBehaviour>());
        }
    }
    private void AbilityAction(CharacterBehaviour target)
    {
        base.AbilityAction();
        StartCoroutine(Reload());
        target.ToggleInteractive();
        Handler.Owner.ToggleInteractive();
        Handler.Owner.animator.SetFloat("moveX", 0);
        Handler.Owner.animator.SetFloat("moveZ", 0);
        Debug.Log("Animator float set");
        Handler.LockAI(Mathf.Infinity);
        movementAI.LockMovementAI();
        CameraController.Instance.SetNewTarget(target.gameObject);
        target.DeathEvent += Reset;
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressedThisFrame;
    }
    protected override void AICheck()
    {
        /* if (loaded && CheckManaCost() && Handler.distanceToClosest < MarkDistance)
        {
            SetFinalAction();
        } */
    }
    protected override void OnEnable()
    {
        base.Reset();
    }
    public override void Reset()
    {
        Handler.Owner.ToggleInteractive();
        Handler.UnlockAI();
        movementAI.UnlockMovementAI();
        CameraController.Instance.SetTargetToDefault();
    }
}
