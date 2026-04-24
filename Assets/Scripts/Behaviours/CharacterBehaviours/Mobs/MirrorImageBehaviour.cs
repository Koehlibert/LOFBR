using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.Services.Analytics;
public class MirrorImageBehaviour : MainPlayerBehaviour
{
    private float LifeTime;
    public void Init(CombatUtils.Team team, int classID, int level, float startingHealth)
    {
        this.Team = team;
        base.Init(classID);
        Levelsys.SetLevel(level);
        LifeTime = GetLifeTime(level);
        CharacterTracker.Instance.AddMob(this);
        if (level > 1)
        {
            StartCoroutine(DelayedLevels(level));
        }
        hpsys.SetHPPercent(startingHealth);
        StartCoroutine(DelayedDestroy(LifeTime));
    }
    private IEnumerator DelayedDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Deactivate();
    }
    private float GetLifeTime(int level)
    {
        return 8 + 2 * level;
    }
    private IEnumerator DelayedLevels(int level)
    {
        for (int i = 2; i <= level; i++)
        {
            yield return new WaitForSeconds(2f);
            skillSet.LevelUnlock(i);
        }
    }
    protected override void InitializeHPSys()
    {
        (float hpval, float regenval, float delay, float armorval) hpVals = skillSet.GetHPVals();
        hpVals.armorval *= -1.5f;
        hpsys.Initialize(hpVals);
    }
    protected override void Die()
    {
        InvokeDeathEvent();
        if (EnemyPlayer != null && LastHit)
        {
            if (EnemyPlayer.gameObject.activeSelf)
            {
                EnemyPlayer.Levelsys.GainExp(5 + 5 * Levelsys.GetLevel());
            }
        }
        CharacterTracker.Instance.RemoveMob(this);
        Destroy(this.gameObject);
    }
    public void Deactivate()
    {
        LastHit = false;
        Die();
    }
}