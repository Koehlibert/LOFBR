using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourEnemy : TowerBehaviour
{
    protected override void Start()
    {
        base.Start();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
}
