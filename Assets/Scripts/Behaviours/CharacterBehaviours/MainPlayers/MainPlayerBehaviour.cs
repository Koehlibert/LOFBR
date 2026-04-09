using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using UnityEditor.SceneManagement;
public abstract class MainPlayerBehaviour : CharacterBehaviour
{
    public Level Levelsys;
    public Mana manasys;
    public int ClassID;
    protected Skillset skillSet;
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
        CharacterTracker.Instance.UnSetMarkedEnemy(this);
        MasterScript.Instance.DieAndRespawn(Team);
    }
    public void OnHealXP()
    {
        Levelsys.GainExp(5);
    }
}