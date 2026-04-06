using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourEnemy : TowerBehaviour
{
    public override void Init()
    {
        this.Team = CombatUtils.Team.Enemy;
        base.Init();
    }
}
