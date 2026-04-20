using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class MarkForDeath : SelectionAbility
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
        DamageableEntity damageableEntity = null;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            damageableEntity = hit.collider.gameObject.GetComponentInParent<DamageableEntity>();
            if (damageableEntity != null && damageableEntity.Team != Handler.Owner.Team)
            {
                damageableEntity.MarkHealthbar();
            }
        }
        if (ConfirmInputPressed())
        {
            ToggleSelecting();
            if (damageableEntity != null && damageableEntity.Team != Handler.Owner.Team)
            {
                AbilityAction(damageableEntity);
            }
        }
    }
    protected override void AbilityAction()
    {
        if (!IsInteractive)
        {
            AbilityAction(Handler.closestEnemy.GetComponent<DamageableEntity>());
        }
    }
    private void AbilityAction(DamageableEntity target)
    {
        base.AbilityAction();
        StartCoroutine(Reload());
        target.MarkThisForDeath();
    }
    protected override bool InputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressedThisFrame;
    }
    protected override void AICheck()
    {
        if (loaded && CheckManaCost() && Handler.distanceToClosest < MarkDistance)
        {
            SetFinalAction();
        }
    }
}
