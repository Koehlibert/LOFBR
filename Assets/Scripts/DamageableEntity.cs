using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour, IMortal
{
    protected Health hpsys;
    protected bool LastHit;
    protected MasterScript master;
    public abstract CombatUtils.Team Team {get;}
    protected virtual void Start()
    {
        LastHit = false;
        hpsys = GetComponent<Health>();
        master = FindAnyObjectByType<MasterScript>();
        SetupCollisionHandler();
    }
    
    protected virtual void SetupCollisionHandler()
    {
        DamageCollisionHandler handler = gameObject.AddComponent<DamageCollisionHandler>();
        //ConfigureCollisionRules(handler);
    }
    
    public virtual void SetLastHit(bool value)
    {
        LastHit = value;
    }
    
    public abstract void Die();
    public abstract Health GetHealth();
}