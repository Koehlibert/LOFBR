using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour, IMortal
{
    protected Health hpsys;
    protected bool LastHit;
    protected MasterScript master;
    
    protected virtual void Start()
    {
        hpsys = GetComponent<Health>();
        master = FindObjectOfType<MasterScript>();
        SetupCollisionHandler();
    }
    
    protected virtual void SetupCollisionHandler()
    {
        DamageCollisionHandler handler = gameObject.AddComponent<DamageCollisionHandler>();
        ConfigureCollisionRules(handler);
    }
    
    protected abstract void ConfigureCollisionRules(DamageCollisionHandler handler);
    
    public virtual void SetLastHit(bool value)
    {
        LastHit = value;
    }
    
    public abstract void Die();
    public abstract Health GetHealth();
}