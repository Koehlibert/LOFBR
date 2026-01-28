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
    public void setDistance(Transform pos2)
    {
        distToPlayer = Vector3.Distance(thing.transform.position, pos2.position);
    }
    public float getDistance()
    {
        return distToPlayer;
    }
    public ObjectWithDist clone()
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
    public void setDistance(Transform pos2)
    {
        distToPlayer = Vector3.Distance(pos, pos2.position);
    }
    public float getDistance()
    {
        return distToPlayer;
    }
    public Tombstone clone()
    {
        return new Tombstone(this.pos);
    }
}
public class MasterScript : MonoBehaviour
{
    public static MasterScript Instance;
    public PlayerController player;
    public ISpawner enemySpawn;
    public ISpawner friendlySpawn;
    public int respawntime;
    public GameObject respawnpointPlayer;
    public GameObject respawnpointEnemyPlayer;
    public bool gameOver;
    public bool victory;
    public int baseMaxHp;
    private EnemyPlayerBehaviour enemyPlayer;
    public RawImage defeatImage;
    public RawImage victoryImage;
    public GameObject friendlyArea;
    public GameObject enemyArea;
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
    }
    void Start()
    {
        ambientsource.Play();
        friendlyArea = GameObject.FindGameObjectWithTag("FriendlyArea");
        enemyArea = GameObject.FindGameObjectWithTag("EnemyArea");
        enemySpawn = GetComponent<EnemySpawner>();
        friendlySpawn = GetComponent<FriendlySpawner>();
        victoryImage.enabled = false;
        defeatImage.enabled = false;
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        player = FindAnyObjectByType<PlayerController>();
        enemyPlayer = FindAnyObjectByType<EnemyPlayerBehaviour>();
        continueBool = false;
        allEnemiesTowers = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyTower"));
        allFriendliesTowers = new List<GameObject>(GameObject.FindGameObjectsWithTag("FriendlyTower"));
        allEnemies = new List<GameObject>();
        allFriendlies = new List<GameObject>();
        rezPoolFriendly = new List<Tombstone>();
    }
    void Update()
    {
        timeCounter++;
        if ((gameOver) && (!continueBool))
        {
            GameOverMenu.SetActive(true);
            GameOverContinue.SetActive(true);
            friendlySpawn.setEnabled(false);
            enemySpawn.setEnabled(false);
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
    public void EnemyDieAndRespawn()
    {
        StartCoroutine("EnemyRespawnCoroutine");
        enemySpawn.speedUpSpawner(0.85f);
        MoveSpawner("Enemy");
    }
    public Vector3 CorrectTarget(Vector3 target)
    {
        return new Vector3(
            Mathf.Clamp(target.x, lowerAreaLimitX, upperAreaLimitX),
            target.y,
            Mathf.Clamp(target.z, friendlySpawn.getZPos(), enemySpawn.getZPos())
        );
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
    public void DieAndRespawn()
    {
        StartCoroutine("RespawnCoroutine");
        friendlySpawn.speedUpSpawner(0.85f);
        MoveSpawner("Friendly");

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
            player.flashcolor = new Color(1f, 0f, 0f, 0.1f);
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
        friendlySpawn.setEnabled(true);
        enemySpawn.setEnabled(true);
        defeatImage.enabled = false;
        victoryImage.enabled = false;
    }
    public void AddEnemy(GameObject enemy)
    {
        allEnemiesTowers.Add(enemy);
        allEnemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        allEnemiesTowers.Remove(enemy);
        allEnemies.Remove(enemy);
    }
    public void AddFriendly(GameObject friendly)
    {
        allFriendliesTowers.Add(friendly);
        allFriendlies.Add(friendly);
    }
    public void RemoveFriendly(GameObject friendly)
    {
        allFriendliesTowers.Remove(friendly);
        allFriendlies.Remove(friendly);
        rezPoolFriendly.Add(new Tombstone(friendly.transform.position));
        if (rezPoolFriendly.Count > 10)
        {
            rezPoolFriendly.RemoveAt(0);
        }
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
                tempList.Add(tomb.clone());
            }
            foreach (Tombstone tomb in tempList)
            {
                tomb.setDistance(player.transform);
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
        return t1.getDistance().CompareTo(t2.getDistance());
    }
    static int SortByDistanceObj(ObjectWithDist t1, ObjectWithDist t2)
    {
        return t1.getDistance().CompareTo(t2.getDistance());
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
                enemy.setDistance(player.transform);
            }
            damagedEnemies.Sort(SortByDistanceObj);
        }
        count = Mathf.Min(count, damagedEnemies.Count);
        return damagedEnemies.GetRange(0,count);
    }
    void MoveSpawner(string playerTeam)
    {
        GameObject area;
        float direction;
        GameObject respawnPoint;
        ISpawner spawner;
        GameObject floor;
        GameObject mover;
        bool bigEnough;
        if (playerTeam == "Friendly")
        {
            area = friendlyArea;
            direction = 1;
            respawnPoint = respawnpointPlayer;
            spawner = friendlySpawn;
            floor = friendlyFloor;
            mover = moverFriendly;
            bigEnough = area.transform.position.z < 90;
        }
        else if (playerTeam == "Enemy")
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
            Debug.Log("Invalid Team String: " + playerTeam);
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
            floor.transform.position = floor.transform.position + direction * new Vector3(0, 0, 5);
            floor.transform.localScale = floor.transform.localScale + new Vector3(-10, 0, 0);
            spawner.moveSpawner();
        }
    }
}