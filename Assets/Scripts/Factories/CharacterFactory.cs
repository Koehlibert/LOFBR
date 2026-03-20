using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterFactory : MonoBehaviour
{
    public static CharacterFactory Instance;
    [SerializeField] GameObject Mob;
    [SerializeField] GameObject EnemyMob;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject CreateTeamMob(CombatUtils.Team team, Vector3 pos, Quaternion rot)
    {
        return team == CombatUtils.Team.Player ? CreatePlayerMob(pos, rot) : CreateEnemyMob(pos, rot);
    }
    public GameObject CreatePlayerMob(Vector3 pos, Quaternion rot)
    {
        GameObject mob = InstantiateMob(Mob, pos, rot);
        return mob;
    }
    public GameObject CreateEnemyMob(Vector3 pos, Quaternion rot)
    {
        GameObject mob = InstantiateMob(EnemyMob, pos, rot);
        return mob;
    }
    private GameObject InstantiateMob(GameObject mob, Vector3 pos, Quaternion rot)
    {
        return Instantiate(mob, pos, rot);
    }
}