using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class ClosestFinder
{
    private IMainPlayer player;
    private GameObject selfObject;
    private CombatUtils.Team enemyTeam;
    private List<GameObject> AllObjects;
    private List<GameObject> AllObjectsNoTowers;
    private IMainPlayer GetPlayer(CombatUtils.Team team)
    {
        return team == CombatUtils.Team.Enemy ? MasterScript.Instance.enemyPlayer : MasterScript.Instance.player;
    }
    public ClosestFinder(CombatUtils.Team team, GameObject selfObject)
    {
        this.enemyTeam = CombatUtils.GetOpposingTeam(team);
        this.selfObject = selfObject;
        this.player = GetPlayer(enemyTeam);
        AllObjects = enemyTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemiesTowers : MasterScript.Instance.allFriendliesTowers;
        AddPlayer(AllObjects);
        AllObjectsNoTowers = enemyTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemies : MasterScript.Instance.allFriendlies;
        AddPlayer(AllObjectsNoTowers);
    }
    private void AddPlayer(List<GameObject> gameObjects)
    {
        if (player == null)
        {
            player = GetPlayer(enemyTeam);
        }
        gameObjects.Add(player.GetGameObject());
        Debug.Log(gameObjects.Count());
    }
    public GameObject FindClosest(bool withPlayer = true)
    {
        return FindClosest(AllObjects, withPlayer);
    }
    public GameObject FindClosestNoTower(bool withPlayer = true)
    {
        return FindClosest(AllObjectsNoTowers, withPlayer);
    }
    public GameObject[] FindTwoClosest(bool withPlayer = true)
    {
        return FindTwoClosest(AllObjects, withPlayer);
    }
    public GameObject FindClosestHurtFriendly()
    {
        List<GameObject> hurtFriendlies = new List<GameObject>();
        List<GameObject> allFriendlies = MasterScript.Instance.allFriendlies;
        foreach (GameObject friendly in allFriendlies)
        {
            if (friendly.GetComponent<Health>().healthDisplay() < 1)
            {
                hurtFriendlies.Add(friendly);
            }
        }
        return FindClosest(hurtFriendlies, false);
    }
    private GameObject FindClosest(List<GameObject> allEnemies, bool withPlayer)
    {
        GameObject closestEnemy = null;
        if (allEnemies.Count != 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                if (!currenemy || !withPlayer && currenemy == player.GetGameObject())
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
                if (!currenemy || !withPlayer && currenemy == player.GetGameObject())
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
            if ((player != null) && (player.GetGameObject().activeSelf) && (Vector3.Distance(selfObject.transform.position, player.GetTransform().position) < closestDistance))
            {
                closeEnemies[1] = closeEnemies[0];
                closeEnemies[0] = player.GetGameObject();
            }
            return closeEnemies;
        }
        return closeEnemies;
    }
}
