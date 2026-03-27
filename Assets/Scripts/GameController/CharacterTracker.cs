using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterTracker : MonoBehaviour
{
    public static CharacterTracker Instance;
    public PlayerController player;
    public EnemyPlayerBehaviour enemyPlayer;
    public List<GameObject> allEnemies;
    public List<GameObject> allFriendlies;
    public List<GameObject> allEnemiesTowers;
    public List<GameObject> allFriendliesTowers;
    private List<Tombstone> rezPoolFriendly;
    private List<Tombstone> rezPoolEnemy;
    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        player = FindAnyObjectByType<PlayerController>();
        enemyPlayer = FindAnyObjectByType<EnemyPlayerBehaviour>();
        allEnemiesTowers = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyTower"));
        allFriendliesTowers = new List<GameObject>(GameObject.FindGameObjectsWithTag("FriendlyTower"));
        allEnemies = new List<GameObject>();
        allFriendlies = new List<GameObject>();
        rezPoolFriendly = new List<Tombstone>();
        rezPoolEnemy = new List<Tombstone>();
        MasterScript.Instance.InitializeCharacters();
    }
    public void AddMob(MobBehaviour Mob)
    {
        if (Mob.Team == CombatUtils.Team.Enemy)
        {
            allEnemiesTowers.Add(Mob.gameObject);
            allEnemies.Add(Mob.gameObject);
        }
        else
        {
            allFriendliesTowers.Add(Mob.gameObject);
            allFriendlies.Add(Mob.gameObject);
        }
    }
    public void RemoveMob(MobBehaviour Mob)
    {
        if (Mob.Team == CombatUtils.Team.Enemy)
        {
            rezPoolEnemy.Add(new Tombstone(Mob.transform.position));
            if (rezPoolEnemy.Count > 10)
            {
                rezPoolEnemy.RemoveAt(0);
            }
            allEnemiesTowers.Remove(Mob.gameObject);
            allEnemies.Remove(Mob.gameObject);
        }
        else
        {
            rezPoolFriendly.Add(new Tombstone(Mob.transform.position));
            if (rezPoolFriendly.Count > 10)
            {
                rezPoolFriendly.RemoveAt(0);
            }
            allFriendliesTowers.Remove(Mob.gameObject);
            allFriendlies.Remove(Mob.gameObject);
        }
    }
    public MainPlayerBehaviour GetOpponentPlayer(CombatUtils.Team team)
    {
        return GetPlayer(CombatUtils.GetOpposingTeam(team));
    }
    public MainPlayerBehaviour GetPlayer(CombatUtils.Team team)
    {
        return team == CombatUtils.Team.Player ? player : enemyPlayer;
    }
    public List<Vector3> GetRezPositions(int count, CombatUtils.Team team)
    {
        List<Tombstone> rezPool = team == CombatUtils.Team.Player ? rezPoolFriendly : rezPoolEnemy;
        if (rezPool.Count == 0)
        {
            return new List<Vector3>();
        }
        else
        {
            count = Mathf.Min(count, rezPool.Count);
            List<Tombstone> tempList = new List<Tombstone>();
            foreach (Tombstone tomb in rezPool)
            {
                tempList.Add(tomb.Clone());
            }
            foreach (Tombstone tomb in tempList)
            {
                tomb.SetDistance(player.transform);
            }
            tempList.Sort(SortByDistanceTomb);
            List<Vector3> posList = new List<Vector3>();
            for (int i = 0; i < count; i++)
            {
                posList.Add(tempList[i].GetPos());
                rezPool.Remove(tempList[i]);
            }
            return posList;
        }
    }
    public int GetRezPoolCount(CombatUtils.Team team)
    {
        return (team == CombatUtils.Team.Player ? rezPoolFriendly : rezPoolEnemy).Count;
    }
    static int SortByDistanceTomb(Tombstone t1, Tombstone t2)
    {
        return t1.GetDistance().CompareTo(t2.GetDistance());
    }
    static int SortByDistanceObj(ObjectWithDist t1, ObjectWithDist t2)
    {
        return t1.GetDistance().CompareTo(t2.GetDistance());
    }
    public List<ObjectWithDist> GetFlurryTargets(int count)
    {
        List<ObjectWithDist> damagedEnemies = new List<ObjectWithDist>();
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy.gameObject.GetComponent<Health>().healthDisplay() <= 0.9f)
            {
                damagedEnemies.Add(new ObjectWithDist(enemy));
            }
        }
        if (damagedEnemies.Count > 0)
        {
            foreach (ObjectWithDist enemy in damagedEnemies)
            {
                enemy.SetDistance(player.transform);
            }
            damagedEnemies.Sort(SortByDistanceObj);
        }
        count = Mathf.Min(count, damagedEnemies.Count);
        return damagedEnemies.GetRange(0, count);
    }
}
public class ObjectWithDist
{
    GameObject thing;
    float distToPlayer;
    public ObjectWithDist(GameObject stuff)
    {
        thing = stuff;
        distToPlayer = 0;
    }
    public GameObject GetObject()
    {
        return thing;
    }
    public void SetDistance(Transform pos2)
    {
        distToPlayer = Vector3.Distance(thing.transform.position, pos2.position);
    }
    public float GetDistance()
    {
        return distToPlayer;
    }
    public ObjectWithDist Clone()
    {
        return new ObjectWithDist(this.thing);
    }
}
public class Tombstone
{
    Vector3 pos;
    float distToPlayer;
    public Tombstone(Vector3 position)
    {
        pos = position;
        distToPlayer = 0;
    }
    public Vector3 GetPos()
    {
        return pos;
    }
    public void SetDistance(Transform pos2)
    {
        distToPlayer = Vector3.Distance(pos, pos2.position);
    }
    public float GetDistance()
    {
        return distToPlayer;
    }
    public Tombstone Clone()
    {
        return new Tombstone(this.pos);
    }
}