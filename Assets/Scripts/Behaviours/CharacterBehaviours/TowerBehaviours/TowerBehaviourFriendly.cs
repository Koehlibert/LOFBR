using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourFriendly : TowerBehaviour
{
    public override void Init()
    {
        this.Team = CombatUtils.Team.Player;
        base.Init();
    }
}