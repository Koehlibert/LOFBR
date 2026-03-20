using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SpawnerBehaviour
{
    protected override CombatUtils.Team Team => CombatUtils.Team.Enemy;
}
