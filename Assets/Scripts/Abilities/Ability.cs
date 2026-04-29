using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public abstract class Ability : MonoBehaviour
{
    protected List<AIUtils.AIState> ActiveStates;
    protected AIHandler Handler { get; set; }
    protected MovementAI movementAI;
    public bool IsInteractive = false;
    protected Mana OwnerManaSys;
    protected Level OwnerLevelSys;
    public float reloadtime;
    protected bool loaded;
    protected Reload reloader;
    public float manaCost;
    protected AbilitySoundType soundType = AbilitySoundType.None;
    public bool ShouldStayActive;
    public bool IsActive;
    public virtual void Checker()
    {
        if (IsActive)
        {
            if (IsInteractive)
            {
                InteractiveCheck();
            }
            else
            {
                if (ActiveStates.Contains(Handler.AIState))
                {
                    AICheck();
                }
            }
        }
    }
    public void Deactivate(Ability callingAbility)
    {
        OnDeactivate(callingAbility);
        IsActive = false;
    }
    protected virtual void OnDeactivate(Ability callingAbility)
    {
    }
    public virtual void Activate()
    {
        IsActive = true;
    }
    protected void SetFinalAction(Action action, Vector3 target, AIUtils.AIState aIState, float lockAITimer)
    {
        SetFinalAction(action);
        movementAI.SetMovementDirection(target);
        Handler.SetAIState(aIState);
        Handler.LockAI(lockAITimer);
    }
    protected void SetFinalAction(Action action, Vector3 target, AIUtils.MovementState movementState, float lockAITimer)
    {
        SetFinalAction(action);
        movementAI.SetMovementDirection(target);
        movementAI.SetMovementState(movementState);
        if (lockAITimer != 0)
            Handler.LockAI(lockAITimer);
    }
    protected void SetFinalAction(Action action)
    {
        Handler.SetFinalAction(action);
    }
    protected void SetFinalActionLockMovement(Action action, float duration)
    {
        SetFinalAction(action);
        movementAI.LockMovementAI(duration);
    }
    protected void SetFinalAction()
    {
        Handler.SetFinalAction(AbilityAction);
    }
    protected virtual void OnEnable()
    {
        Reset();
    }
    public virtual void Init(bool isInteractive, AIHandler aIHandler, MovementAI handlerMovementAI)
    {
        IsActive = true;
        loaded = true;
        Handler = aIHandler;
        IsInteractive = isInteractive;
        movementAI = handlerMovementAI;
        SetAbilityInfo(GetAbilityInfo());
        AdditionalInit();
        if (Handler.Owner is MainPlayerBehaviour mainPlayerBehaviour)
        {
            OwnerManaSys = mainPlayerBehaviour.manasys;
            OwnerLevelSys = mainPlayerBehaviour.Levelsys;
        }
    }
    protected virtual void AdditionalInit()
    {

    }
    public virtual void Init(bool isInteractive, AIHandler aIHandler, MovementAI handlerMovementAI, GameObject reloadObject)
    {
        Init(isInteractive, aIHandler, handlerMovementAI);
        SetReloader(HUD.Instance.GetReload(reloadObject));
        if (isInteractive & reloader != null)
        {
            reloadObject.SetActive(true);
            ActivateReloader();
        }
    }
    protected virtual void InteractiveCheck()
    {
        if (InputPressed() && (loaded) && CheckManaCost())
        {
            AbilityAction();
        }
    }
    protected virtual void AICheck()
    {
        Debug.Log(this);
        throw new NotImplementedException();
    }
    protected abstract bool InputPressed();
    protected virtual void AbilityAction()
    {
        if (IsInteractive)
        {
            reloader?.Shoot();
        }
        if (OwnerManaSys != null)
        {
            OwnerManaSys.useMana(manaCost);
        }
    }
    protected bool CheckManaCost(float manaCostToCheck)
    {
        if (OwnerManaSys != null)
        {
            return OwnerManaSys.checkCost(manaCostToCheck);
        }
        else return true;
    }
    protected bool CheckManaCost()
    {
        return CheckManaCost(manaCost);
    }
    protected void PlaySound()
    {
        if (soundType != AbilitySoundType.None)
        {
            AudioClip clip = AudioManager.Instance.GetClip(soundType);
            AudioManager.Instance.PlaySound(clip, Handler.Owner.transform.position);
        }
    }
    public void ActivateReloader()
    {
        reloader.Activate();
    }
    public virtual void Reset()
    {
        loaded = true;
    }
    protected virtual IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    public void SetReloader(Reload val)
    {
        reloader = val;
        reloader.SetAbility(this);
    }
    protected abstract AbilityInfo GetAbilityInfo();
    private void SetAbilityInfo(AbilityInfo abilityInfo)
    {
        this.manaCost = abilityInfo.ManaCost;
        this.reloadtime = abilityInfo.Reloadtime;
        this.ActiveStates = abilityInfo.ActiveStates;
    }
}
public abstract class DamagingAbility : Ability
{
    protected abstract DamageInfo GetDamageValues();
}
public abstract class SelectionAbility : Ability
{
    protected bool IsSelecting = false;
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
            if (CancelInputPressed())
            {
                DisableSelection();
                return;
            }
            HandleSelection();
        }
    }
    protected virtual void ToggleSelecting()
    {
        IsSelecting = !IsSelecting;
        if (IsSelecting)
        {
            Handler.DisableOtherAbilities(this);
            //movementAI.LockMovementAI();
            Time.timeScale = 0.2f;
        }
        else
        {
            Handler.ReenableOtherAbilities();
            //movementAI.UnlockMovementAI();
            Time.timeScale = 1;
        }
    }
    protected virtual void DisableSelection()
    {
        ToggleSelecting();
    }
    protected virtual bool ConfirmInputPressed()
    {
        return PlayerInputRouter.Instance.PrimaryPressedThisFrame;
    }
    protected virtual bool CancelInputPressed()
    {
        return PlayerInputRouter.Instance.SecondaryPressedThisFrame;
    }
    protected abstract void HandleSelection();
}
public class AbilityInfo
{
    public float ManaCost;
    public float Reloadtime;
    public List<AIUtils.AIState> ActiveStates;
    public bool ShouldStayActive;
    public AbilityInfo(float manaCost, float reloadtime, List<AIUtils.AIState> activeStates, bool shouldStayActive = false)
    {
        this.ManaCost = manaCost;
        this.Reloadtime = reloadtime;
        this.ActiveStates = activeStates;
        this.ShouldStayActive = shouldStayActive;
    }
}
public enum AbilitySoundType
{
    None,
    Shoot,
    Stomp
}