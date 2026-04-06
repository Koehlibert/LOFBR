using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBase : Base
{
    public override void Init()
    {
        this.Team = CombatUtils.Team.Enemy;
        base.Init();
    }
}