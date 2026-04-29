using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class DamageableEntity : MonoBehaviour
{
    public Health hpsys { get; set; }
    protected bool LastHit;
    public CombatUtils.Team Team;
    public Animator animator;
    public GameObject enemyBase;
    public GameObject yourbase;
    protected bool IsMarked = false;
    protected bool ResetMarked = false;
    protected DamageCollisionHandler CollisionHandler;
    [SerializeField] protected Outline healthbarOutline;
    [SerializeField] protected Image healthbar;
    public event Action DeathEvent;
    public virtual void Init()
    {
        LastHit = false;
        hpsys = this.gameObject.AddComponent<Health>();
        hpsys.Death += Die;
        animator = GetComponentInChildren<Animator>();
        SetupCollisionHandler();
        if (healthbar != null)
        {
            hpsys.OnHealthChanged += (healthPercent) =>
            {
                healthbar.fillAmount = healthPercent;
            };
        }
        enemyBase = MasterScript.Instance.GetOpponentBase(Team);
        yourbase = MasterScript.Instance.GetOpponentBase(CombatUtils.GetOpposingTeam(Team));
    }
    protected virtual void SetupCollisionHandler()
    {
        CollisionHandler = gameObject.AddComponent<DamageCollisionHandler>();
        CollisionHandler.Init(this);
    }
    public virtual void SetLastHit(bool value)
    {
        LastHit = value;
    }
    protected virtual void Die()
    {
        InvokeDeathEvent();
    }
    public virtual Health GetHealth()
    {
        return hpsys;
    }
    public virtual void Kill()
    {
        Die();
    }
    public virtual void MarkThisForDeath(float duration)
    {
        CharacterTracker.Instance.SetMarkedEnemy(this);
        IsMarked = false;
        ChangeOutlineAlpha(1);
        StartCoroutine(ResetMark(duration));
    }
    protected virtual IEnumerator ResetMark(float duration)
    {
        yield return new WaitForSeconds(duration);
        UnMarkThisForDeath();
    }
    public virtual void UnMarkThisForDeath()
    {
        ChangeOutlineAlpha(0);
        CharacterTracker.Instance.UnSetMarkedEnemy(this);
    }
    public virtual void MarkHealthbar()
    {
        IsMarked = true;
        ResetMarked = false;
        ChangeOutlineAlpha(0.5f);
    }
    public virtual void MarkHealthbarDebug1()
    {
        IsMarked = true;
        ResetMarked = false;
        ChangeOutlineAlpha(0.25f);
    }
    public virtual void MarkHealthbarDebug2()
    {
        IsMarked = true;
        ResetMarked = false;
        ChangeOutlineAlpha(0.75f);
    }
    protected void ChangeOutlineAlpha(float alpha)
    {
        if (healthbarOutline != null)
        {
            var tmp = healthbarOutline.effectColor;
            tmp.a = alpha;
            healthbarOutline.effectColor = tmp;
        }
    }
    protected void InvokeDeathEvent()
    {
        DeathEvent?.Invoke();
    }
}