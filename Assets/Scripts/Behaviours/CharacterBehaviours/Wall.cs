using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.Services.Analytics;
public class Wall : DamageableEntity
{
    private Renderer rend;
    private int MemberCount;
    public override void Init()
    {
        base.Init();
        for (int i = 1; i <= MemberCount; i++)
        {
            
        }
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        rend.material = Team == CombatUtils.Team.Player
            ? MaterialLibrary.Instance.playerMaterial
            : MaterialLibrary.Instance.enemyMaterial;
        LastHit = false;
        hpsys.Initialize(100, 0, 0, 0);
        healthbar.gameObject.SetActive(false);
        aIHandler = gameObject.AddComponent<AIHandler>();
        aIHandler.Init(this, new List<Ability>(), new List<AIModule>(), 0, false);
    }
    public void Init(CombatUtils.Team team, int memberCount)
    {
        this.Team = team;
        this.MemberCount = memberCount;
        Init();
    }
    protected override void Die()
    {
        Destroy(this.gameObject);
    }
}