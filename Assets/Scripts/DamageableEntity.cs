using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour, IMortal
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
    public abstract void Die();
    public abstract Health GetHealth();
}