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
    public bool IsInteractive;
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
    public virtual void Deactivate()
    {
        IsActive = false;
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
        reloadObject.SetActive(true);
        if (isInteractive & reloader != null)
        {
            ActivateReloader();
        }
    }
    protected virtual void InteractiveCheck()
    {
        if (InputPressed() && (loaded) && OwnerManaSys.checkCost(manaCost))
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
            reloader.Shoot();
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
    public void Reset()
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