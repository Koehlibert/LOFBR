using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestFinder
{
    private IMainPlayer player;
    private GameObject selfObject;
    private MasterScript master;
    public ClosestFinder(IMainPlayer player, GameObject selfObject, MasterScript master)
    {
        this.player = player;
        this.selfObject = selfObject;
        this.master = master;
    }
    public void Initialize(IMainPlayer player, GameObject selfObject, MasterScript master)
    {
        this.player = player;
        this.selfObject = selfObject;
        this.master = master;
    }
    public GameObject FindClosestFriend()
    {
        List<GameObject> allFriendles = master.allFriendliesTowers;
        return FindClosest(allFriendles, player);
    }
    public GameObject[] FindTwoClosestFriendlies()
    {
        List<GameObject> allFriendlies = master.allFriendlies;
        return FindTwoClosest(allFriendlies, player);
    }
    public GameObject FindClosestFriendNoTower()
    {
        List<GameObject> allFriendles = master.allFriendlies;
        return FindClosest(allFriendles, player);
    }
    public GameObject FindClosestEnemy()
    {
        List<GameObject> allEnemies = master.allEnemiesTowers;
        return FindClosest(allEnemies, player);
    }
    public GameObject[] FindTwoClosestEnemies()
    {
        List<GameObject> allEnemies = master.allEnemiesTowers;
        return FindTwoClosest(allEnemies, player);
    }
    public GameObject FindClosestEnemyNoTower()
    {
        List<GameObject> allEnemies = master.allEnemies;
        return FindClosest(allEnemies, player);
    }
    public GameObject FindClosestHurtFriendly()
    {
        List<GameObject> hurtFriendlies = new List<GameObject>();
        List<GameObject> allFriendlies = master.allFriendlies;
        foreach (GameObject friendly in allFriendlies)
        {
            if(friendly.GetComponent<Health>().healthDisplay() < 1)
            {
                hurtFriendlies.Add(friendly);
            }
        }
        return FindClosest(hurtFriendlies, null);
    }
    private GameObject FindClosest(List<GameObject> allEnemies, IMainPlayer player)
    {
        GameObject closestEnemy = null;
        if (allEnemies.Count != 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                if (!currenemy)
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
            if ((player != null) && (player.GetGameObject().activeSelf) && (Vector3.Distance(selfObject.transform.position, player.GetTransform().position) < closestDistance))
            {
                closestEnemy = player.GetGameObject();
            }
            return closestEnemy;
        }
        else
        {
            if ((player != null) && (player.GetGameObject().activeSelf))
            {
                closestEnemy = player.GetGameObject();
            }
            return closestEnemy;
        }
    }
    private GameObject[] FindTwoClosest(List<GameObject> allEnemies, IMainPlayer player)
    {
        GameObject[] closeEnemies = new GameObject[2];
        if (allEnemies.Count != 0)
        {
            float secondclosestDistance = Mathf.Infinity;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject currenemy in allEnemies)
            {
                if (!currenemy)
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
        else
        {
            if ((player != null) && (player.GetGameObject().activeSelf))
            {
                closeEnemies[0] = player.GetGameObject();
            }
            return closeEnemies;
        }
    }
}
