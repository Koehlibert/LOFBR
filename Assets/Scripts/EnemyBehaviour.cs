using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
using System;
public class EnemyBehaviour : MobBehaviour
{
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
}