using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour
{
    protected Health hpsys;
    protected bool LastHit;
    public abstract CombatUtils.Team Team { get; }
    public Animator animator;
    protected virtual void Start()
    {
        LastHit = false;
        hpsys = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        SetupCollisionHandler();
    }
    protected virtual void SetupCollisionHandler()
    {
        DamageCollisionHandler handler = gameObject.AddComponent<DamageCollisionHandler>();
    }
    public virtual void SetLastHit(bool value)
    {
        LastHit = value;
    }
    protected abstract void Die();
    public virtual Health GetHealth()
    {
        return hpsys;
    }
    public virtual void Kill()
    {
        Die();
    }
}