using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class MainPlayerBehaviour : CharacterBehaviour
{
    public Level Levelsys;
    public Mana manasys;
    public int ClassID;
    protected Skillset skillSet;
    [SerializeField] protected Image manaBar;
    public void Init(int classID)
    {
        this.ClassID = classID;
        manasys = this.gameObject.AddComponent<Mana>();
        Levelsys = new Level();
        Levelsys.Init(this);
        base.Init();
    }
    protected override void InitializeAIHandler()
    {
        switch (ClassID)
        {
            case 1:
                skillSet = new SkillsetFighter(aIHandler);
                break;
            case 2:
                skillSet = new SkillsetSupport(aIHandler);
                break;
            case 3:
                skillSet = new SkillsetMelee(aIHandler);
                break;
        }
        aIHandler.Init(this, new List<Ability>(), new List<AIModule>(), skillSet.GetSpeed(), this is EnemyPlayerBehaviour);
        skillSet.LevelUnlock(1);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        UpdateBars();
    }
    private void UpdateBars()
    {
        float hpval = hpsys.healthDisplay();
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, hpval, 5f * Time.deltaTime);
        manaBar.fillAmount = manasys.getPercent();
        if (ResetMarked)
        {
            ChangeOutlineAlpha(0);
            ResetMarked = false;
        }
        if (IsMarked)
        {
            IsMarked = false;
            ResetMarked = true;
        }
    }
    public void ResetAfterDeath()
    {
        aIHandler.ResetAfterDeath();
    }
    protected override void InitializeHPSys()
    {
        hpsys.Initialize(skillSet.GetHPVals());
    }
    public virtual void LevelUp()
    {
        skillSet.LevelUnlock(Levelsys.GetLevel());
        hpsys.UpdateValues((Levelsys.GetLevel() - 1) * 25, Levelsys.GetLevel());
        manasys.UpdateValues(50, Levelsys.GetLevel() * 0.25f);
    }
    protected override void Die()
    {
        if (EnemyPlayer != null && LastHit)
        {
            if (EnemyPlayer.gameObject.activeSelf)
            {
                EnemyPlayer.Levelsys.GainExp(5 + 5 * Levelsys.GetLevel());
            }
        }
        LastHit = false;
        UnMarkThisForDeath();
        MasterScript.Instance.DieAndRespawn(this);
    }
    public void OnHealXP()
    {
        Levelsys.GainExp(5);
    }
}