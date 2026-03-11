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
    public PlayerController player;
    public float reloadtime;
    protected bool loaded;
    protected Reload reloader;
    public float manaCost;
    protected virtual void OnEnable()
    {
        Reset();
        if (IsInteractive)
        {
            player = MasterScript.Instance.player;
        }
    }
    public virtual void Init(bool isInteractive, AIHandler aIHandler)
    {
        loaded = true;
        SetAbilityInfo(GetAbilityInfo());
        Debug.Log(reloadtime);
        AdditionalInit();
        Handler = aIHandler;
        IsInteractive = isInteractive;
    }
    protected virtual void AdditionalInit()
    {
        
    }
    public virtual void Init(bool isInteractive, AIHandler aIHandler, GameObject reloadObject)
    {
        Init(isInteractive, aIHandler);
        setReloader(HUD.Instance.GetReload(reloadObject));
        reloadObject.SetActive(true);
        if (isInteractive & reloader != null)
        {
            ActivateReloader();
        }
    }
    protected virtual void InteractiveCheck()
    {
        if (!player)
        {
            player = MasterScript.Instance.player;
        }
        if (InputPressed() && (loaded) && player.manasys.checkCost(manaCost))
        {
            AbilityAction();
        }
    }
    protected virtual void AICheck()
    {
        throw new NotImplementedException();
    }
    void Update()
    {
        if (IsInteractive)
        {
            InteractiveCheck();
        }
    }
    protected abstract bool InputPressed();
    protected abstract void AbilityAction();
    public void ActivateReloader()
    {
        reloader.Activate();
    }
    public void Reset()
    {
        loaded = true;
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        loaded = true;
    }
    public void setReloader(Reload val)
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