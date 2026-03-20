using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourEnemy : TowerBehaviour
{
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
}
