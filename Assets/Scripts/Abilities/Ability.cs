using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public abstract class Ability : MonoBehaviour
{
    protected List<AIUtils.AIState> ActiveStates;
    protected AIHandler Handler { get; set; }
    public bool IsInteractive;
    protected Mana OwnerManaSys;
    protected Level OwnerLevelSys;
    public float reloadtime;
    protected bool loaded;
    protected Reload reloader;
    public float manaCost;
    protected AbilitySoundType soundType = AbilitySoundType.None;
    public virtual void Checker()
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
    protected void SetFinalAction(Action action, Vector3 target, AIUtils.AIState aIState, float lockAITimer)
    {
        Handler.FinalAction = action;
        Handler.MovementDirection = target;
        Handler.SetAIState(aIState);
        Handler.LockAI(lockAITimer);
    }
    protected virtual void OnEnable()
    {
        Reset();
    }
    public virtual void Init(bool isInteractive, AIHandler aIHandler)
    {
        loaded = true;
        Handler = aIHandler;
        SetAbilityInfo(GetAbilityInfo());
        AdditionalInit();
        IsInteractive = isInteractive;
        if (Handler.Owner is MainPlayerBehaviour mainPlayerBehaviour)
        {
            OwnerManaSys = mainPlayerBehaviour.manasys;
            OwnerLevelSys = mainPlayerBehaviour.Levelsys;
        }
    }
    protected virtual void AdditionalInit()
    {

    }
    public virtual void Init(bool isInteractive, AIHandler aIHandler, GameObject reloadObject)
    {
        Init(isInteractive, aIHandler);
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
    private IEnumerator Reload()
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
    public AbilityInfo(float manaCost, float reloadtime, List<AIUtils.AIState> activeStates)
    {
        this.ManaCost = manaCost;
        this.Reloadtime = reloadtime;
        this.ActiveStates = activeStates;
    }
}
public enum AbilitySoundType
{
    None,
    Shoot,
    Stomp
}