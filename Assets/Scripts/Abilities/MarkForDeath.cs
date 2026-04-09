using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MarkForDeath : Ability
{
    private bool IsSelecting = false;
    private float MarkDistance = 30f;
    protected override AbilityInfo GetAbilityInfo()
    {
        return new AbilityInfo(15, 2f, new List<AIUtils.AIState> { AIUtils.AIState.Attacking, AIUtils.AIState.CheckShoot, AIUtils.AIState.CheckDistSkills, AIUtils.AIState.Retreating });
    }
    protected override void AdditionalInit()
    {
        reloadtime = 12;
    }
    protected override void InteractiveCheck()
    {
        if (!IsSelecting)
        {
            if (InputPressed() && loaded && CheckManaCost())
            {
                ToggleSelecting();
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(PlayerInputRouter.Instance.Look);
            DamageableEntity damageableEntity = null;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                damageableEntity = hit.collider.gameObject.GetComponent<DamageableEntity>();
                if (damageableEntity != null && damageableEntity.Team != Handler.Owner.Team)
                {
                    damageableEntity.MarkHealthbar();
                }
            }
            if (InputPressed())
            {
                ToggleSelecting();
                if (damageableEntity != null && damageableEntity.Team != Handler.Owner.Team)
                {
                    AbilityAction(damageableEntity);
                }
            }
        }
    }
    private void ToggleSelecting()
    {
        IsSelecting = !IsSelecting;
        if (IsSelecting)
        {
            Handler.DisableOtherAbilities(this);
            Time.timeScale = 0.35f;
        }
        else
        {
            Handler.ReenableOtherAbilities();
            Time.timeScale = 1;
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
