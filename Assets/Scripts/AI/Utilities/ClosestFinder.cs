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
    private int LastFrameComputed = -1;
    private int LastFrameComputedNoTower = -1;
    private GameObject CachedClosest;
    private GameObject CachedClosestNoTower;
    private List<InDistanceTracker> inDistanceTrackers;
    private MainPlayerBehaviour GetPlayer(CombatUtils.Team team)
    {
        return team == CombatUtils.Team.Enemy ? MasterScript.Instance.enemyPlayer : MasterScript.Instance.player;
    }
    public ClosestFinder(CombatUtils.Team team, GameObject selfObject)
        : this(team, CombatUtils.GetOpposingTeam(team), selfObject)
    {
    }
    public ClosestFinder(CombatUtils.Team team, CombatUtils.Team targetTeam, GameObject selfObject)
    {
        this.targetTeam = targetTeam;
        this.selfObject = selfObject;
        this.player = GetPlayer(targetTeam);
        inDistanceTrackers = new List<InDistanceTracker>();
        AllObjects = targetTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemiesTowers : MasterScript.Instance.allFriendliesTowers;
        AllObjectsNoTowers = targetTeam == CombatUtils.Team.Enemy ? MasterScript.Instance.allEnemies : MasterScript.Instance.allFriendlies;
    }
    public GameObject FindClosest(bool withPlayer = true, bool onlyHurt = false)
    {
        if (LastFrameComputed != Time.frameCount)
        {
            CachedClosest = FindClosest(AllObjects, withPlayer, onlyHurt);
            LastFrameComputed = Time.frameCount;
        }
        return CachedClosest;
    }
    public GameObject FindClosestNoTower(bool withPlayer = true, bool onlyHurt = false)
    {
        if (LastFrameComputedNoTower != Time.frameCount)
        {
            CachedClosestNoTower = FindClosest(AllObjectsNoTowers, withPlayer, onlyHurt, true);
            LastFrameComputedNoTower = Time.frameCount;
        }
        return CachedClosestNoTower;
    }
    public List<GameObject> FindNClosest(int n, bool withPlayer)
    {
        return FindClosestN(AllObjectsNoTowers, n, withPlayer);
    }
    private GameObject FindClosest(List<GameObject> allEnemies, bool withPlayer, bool onlyHurt = false, bool TrackNumber = false)
    {
        foreach (InDistanceTracker inDistanceTracker in inDistanceTrackers)
        {
            inDistanceTracker.ResetCounter();
        }
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        if (withPlayer && player.isActiveAndEnabled)
        {
            closestEnemy = player.gameObject;
            closestDistance = Vector3.Distance(player.transform.position, selfObject.transform.position);
            foreach (InDistanceTracker inDistanceTracker in inDistanceTrackers)
            {
                inDistanceTracker.CheckInDistance(closestDistance, true);
            }
        }
        foreach (GameObject currenemy in allEnemies)
        {
            if (!currenemy || (onlyHurt && currenemy.GetComponent<Health>().FullHP()))
            {
                continue;
            }
            float distanceToEnemy = Vector3.Distance(currenemy.transform.position, selfObject.transform.position);
            foreach (InDistanceTracker inDistanceTracker in inDistanceTrackers)
            {
                inDistanceTracker.CheckInDistance(distanceToEnemy, false);
            }
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = currenemy;
            }
        }
        return closestEnemy;
    }
    private List<GameObject> FindClosestN(List<GameObject> allEnemies, int n, bool withPlayer)
    {
        var validEnemies = new List<GameObject>(allEnemies);
        if (withPlayer && player.isActiveAndEnabled)
        {
            validEnemies.Add(player.gameObject);
        }
        validEnemies.Sort((a, b) =>
        {
            float distA = Vector3.Distance(a.transform.position, selfObject.transform.position);
            float distB = Vector3.Distance(b.transform.position, selfObject.transform.position);
            return distA.CompareTo(distB);
        });
        return validEnemies.Take(n).ToList();
    }
    public int GetActiveEnemyNumber()
    {
        return AllObjectsNoTowers.Count;
    }
    public InDistanceTracker StartTrackingDist(float distToCheck, bool withPlayer)
    {
        InDistanceTracker inDistanceTracker = new InDistanceTracker(distToCheck, withPlayer, this);
        inDistanceTrackers.Add(inDistanceTracker);
        return inDistanceTracker;
    }
    public void StopTrackingDist(InDistanceTracker inDistanceTracker)
    {
        inDistanceTrackers.Remove(inDistanceTracker);
    }
}
public class InDistanceTracker
{
    private float DistToCheck { get; }
    private int EnemiesInDistance { get; set; }
    private ClosestFinder ClosestFinder;
    private bool withPlayer;
    public InDistanceTracker(float distToCheck, bool withPlayer, ClosestFinder closestFinder)
    {
        this.DistToCheck = distToCheck;
        ClosestFinder = closestFinder;
        EnemiesInDistance = 0;
    }
    public void ResetCounter()
    {
        EnemiesInDistance = 0;
    }
    public bool GetOverCount(int numberToCheck)
    {
        return EnemiesInDistance >= numberToCheck;
    }
    public void CheckInDistance(float distance, bool isPlayer)
    {
        if (distance < DistToCheck && (!isPlayer || (withPlayer & isPlayer)))
            EnemiesInDistance++;
    }
}