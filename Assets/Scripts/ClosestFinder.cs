using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class ClosestFinder
{
    private MainPlayerBehaviour player;
    private GameObject selfObject;
    private CombatUtils.Team targetTeam;
    private List<GameObject> AllObjects;
    private List<GameObject> AllObjectsNoTowers;
    private MainPlayerBehaviour GetPlayer(CombatUtils.Team team)
    {
        return team == CombatUtils.Team.Enemy ? MasterScript.Instance.enemyPlayer : MasterScript.Instance.player;
    }
    public ClosestFinder(CombatUtils.Team team, GameObject selfObject)
    {
        this.targetTeam = CombatUtils.GetOpposingTeam(team);
        this.selfObject = selfObject;
        this.player = GetPlayer(targetTeam);
        AllObjects = targetTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemiesTowers : MasterScript.Instance.allFriendliesTowers;
        AddPlayer(AllObjects);
        AllObjectsNoTowers = targetTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemies : MasterScript.Instance.allFriendlies;
        AddPlayer(AllObjectsNoTowers);
    }
    public ClosestFinder(CombatUtils.Team team, CombatUtils.Team targetTeam, GameObject selfObject)
    {
        this.targetTeam = targetTeam;
        this.selfObject = selfObject;
        this.player = GetPlayer(targetTeam);
        AllObjects = targetTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemiesTowers : MasterScript.Instance.allFriendliesTowers;
        AddPlayer(AllObjects);
        AllObjectsNoTowers = targetTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemies : MasterScript.Instance.allFriendlies;
        AddPlayer(AllObjectsNoTowers);
    }
    private void AddPlayer(List<GameObject> gameObjects)
    {
        if (player == null)
        {
            player = GetPlayer(targetTeam);
        }
        gameObjects.Add(player.gameObject);
    }
    public GameObject FindClosest(bool withPlayer = true, bool onlyHurt = false)
    {
        return FindClosest(AllObjects, withPlayer, onlyHurt);
    }
    public GameObject FindClosestNoTower(bool withPlayer = true, bool onlyHurt = false)
    {
        return FindClosest(AllObjectsNoTowers, withPlayer, onlyHurt);
    }
    public GameObject[] FindTwoClosest(bool withPlayer = true)
    {
        return FindTwoClosest(AllObjects, withPlayer);
    }
    private GameObject FindClosest(List<GameObject> allEnemies, bool withPlayer, bool onlyHurt = false)
    {
        GameObject closestEnemy = null;
        if (allEnemies.Count != 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                if ((!currenemy) || (onlyHurt && currenemy.GetComponent<Health>().FullHP()) || (!withPlayer && currenemy == player.gameObject))
                {
                    continue;
                }
                float distanceToEnemy = Vector3.Distance(currenemy.transform.position, selfObject.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = currenemy;
                }
            }
        }
        return closestEnemy;
    }
    private GameObject[] FindTwoClosest(List<GameObject> allEnemies, bool withPlayer)
    {
        GameObject[] closeEnemies = new GameObject[2];
        if (allEnemies.Count != 0)
        {
            float secondclosestDistance = Mathf.Infinity;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                if (!currenemy || !withPlayer && currenemy == player.gameObject)
                {
                    continue;
                }
                float distanceToEnemy = Vector3.Distance(currenemy.transform.position, selfObject.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    secondclosestDistance = closestDistance;
                    closestDistance = distanceToEnemy;
                    closeEnemies[1] = closeEnemies[0];
                    closeEnemies[0] = currenemy;
                }
            }
            if ((player != null) && player.gameObject.activeSelf && (Vector3.Distance(selfObject.transform.position, player.transform.position) < closestDistance))
            {
                closeEnemies[1] = closeEnemies[0];
                closeEnemies[0] = player.gameObject;
            }
            return closeEnemies;
        }
        return closeEnemies;
    }
    public int GetActiveEnemyNumber()
    {
        return AllObjectsNoTowers.Count;
    }
}
