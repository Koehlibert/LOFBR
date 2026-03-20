using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlySpawner : SpawnerBehaviour
{
    protected override CombatUtils.Team Team => CombatUtils.Team.Player;
}
