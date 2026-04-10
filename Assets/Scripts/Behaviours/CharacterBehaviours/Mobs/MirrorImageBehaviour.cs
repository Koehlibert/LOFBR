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
    public void Init(CombatUtils.Team team, int classID, int level)
    {
        this.Team = team;
        base.Init(classID);
        Levelsys.SetLevel(level);
        CharacterTracker.Instance.AddMob(this);
        if (level > 1)
        {
            for (int i = 2; i <= level; i++)
            {
                skillSet.LevelUnlock(i);
            }
        }
    }
    protected override void InitializeHPSys()
    {
        (float hpval, float regenval, float delay, float armorval) hpVals = skillSet.GetHPVals();
        hpVals.hpval *= 0.4f;
        hpVals.armorval *= -1.5f;
        hpsys.Initialize(hpVals);
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
        CharacterTracker.Instance.RemoveMob(this);
    }
}