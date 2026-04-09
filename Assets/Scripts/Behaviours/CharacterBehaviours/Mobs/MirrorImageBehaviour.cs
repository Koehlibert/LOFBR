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
    private int Level;
    public void Init(CombatUtils.Team team, int classID, int level)
    {
        this.Team = team;
        this.Level = level;
        base.Init(classID);
    }

}