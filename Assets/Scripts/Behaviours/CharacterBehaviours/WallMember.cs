using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine.XR.WSA;
public class WallMember : MonoBehaviour
{
    [SerializeField] Renderer rend;
    private CombatUtils.Team Team;
    public void Init(CombatUtils.Team team)
    {
        this.Team = team;
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        rend.material = Team == CombatUtils.Team.Player
            ? MaterialLibrary.Instance.playerMaterial
            : MaterialLibrary.Instance.enemyMaterial;
    }
}