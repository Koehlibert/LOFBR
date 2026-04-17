using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography.X509Certificates;

public class CharacterFactory : MonoBehaviour
{
    public static CharacterFactory Instance;
    [SerializeField] GameObject Mob;
    [SerializeField] GameObject Thrower;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject EnemyPlayer;
    [SerializeField] GameObject WallMember;
    [SerializeField] GameObject MirrorEntity;
    [SerializeField] GameObject Referee;
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
    private GameObject InstantiateCharacter(GameObject mob)
    {
        return Instantiate(mob);
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
    public GameObject CreateWallMember(CombatUtils.Team team, Vector3 localPos, GameObject parent)
    {
        GameObject wallMember = InstantiateCharacter(WallMember);
        wallMember.transform.SetParent(parent.transform);
        wallMember.transform.position = localPos;
        wallMember.AddComponent<WallMember>().Init(team);
        return wallMember;
    }
    public GameObject CreateMirrorEntity(CombatUtils.Team team, Vector3 pos, Quaternion rot, int classID, int level, float startingHealth)
    {
        GameObject mirrorImage = InstantiateCharacter(MirrorEntity, pos, rot);
        mirrorImage.GetComponent<MirrorImageBehaviour>().Init(team, classID, level, startingHealth);
        return mirrorImage;
    }
    public GameObject CreateReferee(CombatUtils.Team team, float posZ)
    {
        float x = team == CombatUtils.Team.Player ? AreaManagerFriendly.Instance.lowerAreaLimitX : AreaManagerFriendly.Instance.upperAreaLimitX;
        GameObject referee = InstantiateCharacter(Referee, new(x, 0, posZ), Quaternion.identity);
        referee.transform.LookAt(new Vector3(0,0,posZ));
        referee.AddComponent<RefereeBehaviour>().Init();
        return referee;
    }
}