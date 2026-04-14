using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManagerEnemy : AreaManager
{
    protected override CombatUtils.Team Team => CombatUtils.Team.Enemy;
    public static AreaManagerEnemy Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override void Init()
    {
        Area = GameObject.FindGameObjectWithTag("EnemyArea");
        Spawner = GetComponent<EnemySpawner>();
        MoveDirection = -1f;
    }
}
