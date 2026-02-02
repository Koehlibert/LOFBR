using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UltBulletBehaviour : UltBullet
{
    protected override void Start()
    {
        base.Start();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
}
