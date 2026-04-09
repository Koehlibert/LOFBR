using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour
{
    public Health hpsys { get; set; }
    protected bool LastHit;
    public CombatUtils.Team Team;
    public Animator animator;
    public float AnimSpeed { get; set; }
    public GameObject enemyBase;
    public GameObject yourbase;
    public AIHandler aIHandler;
    protected bool IsMarked = false;
    protected bool ResetMarked = false;
    protected DamageCollisionHandler CollisionHandler;
    [SerializeField] protected Outline healthbarOutline;
    [SerializeField] protected Image healthbar;
    public virtual void Init()
    {
        LastHit = false;
        hpsys = this.gameObject.AddComponent<Health>();
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
    protected abstract void Die();
    public virtual Health GetHealth()
    {
        return hpsys;
    }
    public virtual void Kill()
    {
        Die();
    }
    public virtual void MarkThisForDeath()
    {
        CharacterTracker.Instance.SetMarkedEnemy(this);
        IsMarked = false;
        ChangeOutlineAlpha(1);
        StartCoroutine(ResetMark());
    }
    protected virtual IEnumerator ResetMark()
    {
        yield return new WaitForSeconds(4f);
        ChangeOutlineAlpha(0);
        CharacterTracker.Instance.UnSetMarkedEnemy(this);
    }
    public virtual void MarkHealthbar()
    {
        IsMarked = true;
        ResetMarked = false;
        ChangeOutlineAlpha(0.5f);
    }
    protected void ChangeOutlineAlpha(float alpha)
    {
        var tmp = healthbarOutline.effectColor;
        tmp.a = alpha;
        healthbarOutline.effectColor = tmp;
    }
}