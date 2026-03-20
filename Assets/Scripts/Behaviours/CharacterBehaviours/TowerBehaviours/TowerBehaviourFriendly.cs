using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourFriendly : TowerBehaviour
{
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
}