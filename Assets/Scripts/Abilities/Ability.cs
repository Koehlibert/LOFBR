using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public abstract class Ability : MonoBehaviour
{
    protected bool IsActive { get; set; }
    public bool IsInteractive { get; set; }
    protected AIHandler Handler { get; set; }
    public virtual void Checker()
    {
        
    }
    protected AIUtils.AIState ActiveState { get; set; }
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
    public Reload reloader;
    public float manaCost;
    protected virtual void Start()
    {
        player = GetComponent<PlayerController>();
        Handler = GetComponent<AIHandler>();
    }
    protected virtual void OnEnable()
    {
        Reset();
        player = GetComponent<PlayerController>();
        Handler = GetComponent<AIHandler>();
    }
    protected virtual void Update()
    {
        if(!player)
        {
            player = GetComponent<PlayerController>();
        }
        if(InputPressed() && (loaded) && player.manasys.checkCost(manaCost))
        {
            AbilityAction();
        }
    }
    protected abstract bool InputPressed();
    protected abstract void AbilityAction();
    public void Activate()
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
        reloader.setAbility(this);
    }
}
public abstract class DamagingAbility : Ability
{
    protected abstract DamageInfo GetDamageValues();
}