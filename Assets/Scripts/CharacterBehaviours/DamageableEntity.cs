using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour
{
    public Health hpsys { get; set; }
    protected bool LastHit;
    public abstract CombatUtils.Team Team { get; }
    public Animator animator;
    public float AnimSpeed { get; set; }
    public GameObject enemyBase;
    public GameObject yourbase;
    public AIHandler aIHandler;
    protected virtual void Start()
    {
        LastHit = false;
        hpsys = this.gameObject.AddComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        SetupCollisionHandler();
        enemyBase = MasterScript.Instance.GetOpponentBase(Team);
        yourbase = MasterScript.Instance.GetOpponentBase(CombatUtils.GetOpposingTeam(Team));
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