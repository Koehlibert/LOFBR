using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
public class MasterScript : MonoBehaviour
{
    public static MasterScript Instance;
    public PlayerController player;
    public EnemySpawner enemySpawn;
    public FriendlySpawner friendlySpawn;
    public int respawntime;
    public GameObject respawnpointPlayer;
    public GameObject respawnpointEnemyPlayer;
    public bool gameOver;
    public bool victory;
    public int baseMaxHp;
    public EnemyPlayerBehaviour enemyPlayer;
    public RawImage defeatImage;
    public RawImage victoryImage;
    public GameObject friendlyArea;
    public GameObject enemyArea;
    public GameObject enemyBase;
    public GameObject friendlyBase;
    public GameObject friendlyFloor;
    public GameObject enemyFloor;
    public AudioClip death;
    public AudioSource soundsource;
    public AudioSource ambientsource;
    public GameObject moverFriendly;
    public GameObject moverEnemy;
    public List<GameObject> allEnemies;
    public List<GameObject> allFriendlies;
    public List<GameObject> allEnemiesTowers;
    public List<GameObject> allFriendliesTowers;
    public GameObject HUD;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public float timeCounter;
    private bool continueBool;
    private List<Tombstone> rezPoolFriendly;
    public float upperAreaLimitX = 18;
    public float lowerAreaLimitX = -18;
    private void Awake()
    {
        Instance = this;
        ambientsource.Play();
        friendlyArea = GameObject.FindGameObjectWithTag("FriendlyArea");
        enemyArea = GameObject.FindGameObjectWithTag("EnemyArea");
        friendlyBase = GameObject.FindGameObjectWithTag("FriendlyBase");
        enemyBase = GameObject.FindGameObjectWithTag("EnemyBase");
        enemySpawn = GetComponent<EnemySpawner>();
        friendlySpawn = GetComponent<FriendlySpawner>();
        victoryImage.enabled = false;
        defeatImage.enabled = false;
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        continueBool = false;
        player = FindAnyObjectByType<PlayerController>();
        enemyPlayer = FindAnyObjectByType<EnemyPlayerBehaviour>();
        allEnemiesTowers = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyTower"));
        allFriendliesTowers = new List<GameObject>(GameObject.FindGameObjectsWithTag("FriendlyTower"));
        allEnemies = new List<GameObject>();
        allFriendlies = new List<GameObject>();
        rezPoolFriendly = new List<Tombstone>();
        friendlyBase.AddComponent<FriendlyBase>().Init();
        enemyBase.AddComponent<EnemyBase>().Init();
        player.Init();
        enemyPlayer.Init();
        foreach (GameObject enemyTower in allEnemiesTowers)
            enemyTower.GetComponent<TowerBehaviourEnemy>().Init();
        foreach (GameObject friendlyTower in allFriendliesTowers)
            friendlyTower.GetComponent<TowerBehaviourFriendly>().Init();
    }
    void Update()
    {
        timeCounter++;
        if ((gameOver) && (!continueBool))
        {
            GameOverMenu.SetActive(true);
            GameOverContinue.SetActive(true);
            friendlySpawn.SetEnabled(false);
            enemySpawn.SetEnabled(false);
            if (victory)
            {
                victoryImage.enabled = true;
            }
            else
            {
                defeatImage.enabled = true;
            }
        }
    }
    public Vector3 CorrectTarget(Vector3 target)
    {
        return new Vector3(
            Mathf.Clamp(target.x, lowerAreaLimitX, upperAreaLimitX),
            target.y,
            Mathf.Clamp(target.z, friendlySpawn.GetZPos(), enemySpawn.GetZPos())
        );
    }
    public void DieAndRespawn(CombatUtils.Team team)
    {
        if (team == CombatUtils.Team.Player)
        {
            StartCoroutine(RespawnCoroutine());
            friendlySpawn.SpeedUpSpawner(1f);
        }
        else
        {
            StartCoroutine(EnemyRespawnCoroutine());
            enemySpawn.SpeedUpSpawner(1f);
        }
        MoveSpawner(team);
    }
    public IEnumerator RespawnCoroutine()
    {
        soundsource.Play();
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawntime);
        if ((!gameOver) || GameOverContinue)
        {
            player.transform.position = respawnpointPlayer.transform.position;
            player.gameObject.SetActive(true);
            //player.flashcolor = new Color(1f, 0f, 0f, 0.1f);
        }
    }
    public IEnumerator EnemyRespawnCoroutine()
    {
        enemyPlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawntime);
        if ((!gameOver) || GameOverContinue)
        {
            enemyPlayer.transform.position = respawnpointEnemyPlayer.transform.position;
            enemyPlayer.gameObject.SetActive(true);
        }
    }
    public void ToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void ContinueGame()
    {
        continueBool = true;
        HUD.SetActive(true);
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        gameOver = false;
        friendlySpawn.SetEnabled(true);
        enemySpawn.SetEnabled(true);
        defeatImage.enabled = false;
        victoryImage.enabled = false;
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
            allEnemiesTowers.Remove(Mob.gameObject);
            allEnemies.Remove(Mob.gameObject);
        }
        else
        {
            //TOFIX:: ADD FOR ENEMY
            rezPoolFriendly.Add(new Tombstone(Mob.transform.position));
            if (rezPoolFriendly.Count > 10)
            {
                rezPoolFriendly.RemoveAt(0);
            }
            allFriendliesTowers.Remove(Mob.gameObject);
            allFriendlies.Remove(Mob.gameObject);
        }
    }
    public MainPlayerBehaviour GetOpponentPlayer(CombatUtils.Team Team)
    {
        return Team == CombatUtils.Team.Player ? enemyPlayer : player;
    }
    public float GetOpponentSpawnZ(CombatUtils.Team Team)
    {
        return Team == CombatUtils.Team.Player ? respawnpointEnemyPlayer.transform.position.z : respawnpointPlayer.transform.position.z;
    }
    public List<Vector3> GetRezPositions(int count)
    {
        if (rezPoolFriendly.Count == 0)
        {
            return new List<Vector3>();
        }
        else
        {
            count = Mathf.Min(count, rezPoolFriendly.Count);
            List<Tombstone> tempList = new List<Tombstone>();
            foreach (Tombstone tomb in rezPoolFriendly)
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
                rezPoolFriendly.Remove(tempList[i]);
            }
            return posList;
        }
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
    public GameObject GetOpponentBase(CombatUtils.Team enemyTeam)
    {
        return enemyTeam == CombatUtils.Team.Enemy ? enemyBase : friendlyBase;
    }
    void MoveSpawner(CombatUtils.Team playerTeam)
    {
        GameObject area;
        float direction;
        GameObject respawnPoint;
        SpawnerBehaviour spawner;
        GameObject floor;
        GameObject mover;
        bool bigEnough;
        if (playerTeam == CombatUtils.Team.Player)
        {
            area = friendlyArea;
            direction = 1;
            respawnPoint = respawnpointPlayer;
            spawner = friendlySpawn;
            floor = friendlyFloor;
            mover = moverFriendly;
            bigEnough = area.transform.position.z < 90;
        }
        else if (playerTeam == CombatUtils.Team.Enemy)
        {
            area = enemyArea;
            direction = -1;
            respawnPoint = respawnpointEnemyPlayer;
            spawner = enemySpawn;
            floor = enemyFloor;
            mover = moverEnemy;
            bigEnough = enemyArea.transform.position.z > -90;
        }
        else
        {
            area = null;
            direction = 0;
            respawnPoint = null;
            spawner = null;
            floor = null;
            mover = null;
            bigEnough = false;
        }
        if (bigEnough)
        {
            Instantiate(mover, respawnPoint.transform.position - direction * new Vector3(0, 0, 5), Quaternion.identity);
            area.transform.position = area.transform.position + direction * new Vector3(0, 0, 10);
            floor.transform.position = floor.transform.position - direction * new Vector3(0, 0, 5);
            floor.transform.localScale = floor.transform.localScale + new Vector3(-10, 0, 0);
            spawner.MoveSpawner();
        }
    }
}