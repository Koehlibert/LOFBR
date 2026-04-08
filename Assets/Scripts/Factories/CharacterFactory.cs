using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterFactory : MonoBehaviour
{
    public static CharacterFactory Instance;
    [SerializeField] GameObject Mob;
    [SerializeField] GameObject Thrower;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject EnemyPlayer;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject CreateTeamMob(CombatUtils.Team team, Vector3 pos, Quaternion rot)
    {
        GameObject mob = InstantiateCharacter(Mob, pos, rot);
        mob.GetComponent<MobBehaviour>().Init(team);
        return mob;
    }
    private GameObject InstantiateCharacter(GameObject mob, Vector3 pos, Quaternion rot)
    {
        return Instantiate(mob, pos, rot);
    }
    public GameObject RezMob(CombatUtils.Team team, Vector3 pos, Quaternion rot)
    {
        GameObject mob = CreateTeamMob(team, pos, rot);
        mob.GetComponent<MobBehaviour>().GetRezd();
        return mob;
    }
    public GameObject CreateTeamTower(CombatUtils.Team team, Vector3 pos, Quaternion rot)
    {
        GameObject tower = InstantiateCharacter(Thrower, pos, rot);
        tower.GetComponent<TowerBehaviour>().Init(team);
        return tower;
    }
    public GameObject CreateTeamPlayer(CombatUtils.Team team, Vector3 pos, Quaternion rot)
    {
        GameObject playerToInst = team == CombatUtils.Team.Enemy ? EnemyPlayer : Player;
        GameObject player = InstantiateCharacter(playerToInst, pos, rot);
        return player;
    }
}