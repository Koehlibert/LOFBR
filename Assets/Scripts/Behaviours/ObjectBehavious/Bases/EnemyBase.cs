using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBase : Base
{
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
}