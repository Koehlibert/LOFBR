using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR;

public class PossessDebug : SelectionAbility
{
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
            if (IsValidTarget(characterBehaviour))
            {
                characterBehaviour.MarkHealthbar();
            }
        }
        if (ConfirmInputPressed())
        {
            ToggleSelecting();
            if (IsValidTarget(characterBehaviour))
            {
                AbilityAction(characterBehaviour);
            }
        }
    }
    public bool IsValidTarget(CharacterBehaviour characterBehaviour)
    {
        return characterBehaviour != null && characterBehaviour.Team == Handler.Owner.Team && !(characterBehaviour is TowerBehaviour);
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
        ActiveCharacterManager.Instance.ChangeActiveCharacter(target);
        target.DeathEvent += ActiveCharacterManager.Instance.ResetActiveCharacter;
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
/*     public override void Reset()
    {
        ActiveCharacterManager.Instance.ResetActiveCharacter();
    } */
}
