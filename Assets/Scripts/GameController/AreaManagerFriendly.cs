using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManagerFriendly : AreaManager
{
    protected override CombatUtils.Team Team => CombatUtils.Team.Player;
    public static AreaManagerFriendly Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override void Init()
    {
        Area = GameObject.FindGameObjectWithTag("FriendlyArea");
        Spawner = GetComponent<FriendlySpawner>();
        MoveDirection = 1f;
    }
}
