using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourFriendly : TowerBehaviour
{
    protected override void Start()
    {
        base.Start();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
}