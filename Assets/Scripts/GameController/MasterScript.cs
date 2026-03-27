using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class MasterScript : MonoBehaviour
{
    public static MasterScript Instance;
    public EnemySpawner enemySpawn;
    public FriendlySpawner friendlySpawn;
    public int respawntime;
    public GameObject respawnpointPlayer;
    public GameObject respawnpointEnemyPlayer;
    public bool gameOver;
    public bool victory;
    public int baseMaxHp;
    public RawImage defeatImage;
    public RawImage victoryImage;
    public GameObject friendlyArea;
    public GameObject enemyArea;
    public GameObject enemyBase;
    public GameObject friendlyBase;
    public GameObject friendlyFloor;
    public GameObject enemyFloor;
    public GameObject moverFriendly;
    public GameObject moverEnemy;
    public GameObject HUD;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public float timeCounter;
    private bool continueBool;
    public float upperAreaLimitX = 18;
    public float lowerAreaLimitX = -18;
    private void Awake()
    {
        Instance = this;
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
        CharacterTracker.Instance.Init();
    }
    void Update()
    {
        timeCounter++;
        if (gameOver && (!continueBool))
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
        AudioManager.Instance.PlayerDies();
        CharacterTracker.Instance.player.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawntime);
        if ((!gameOver) || GameOverContinue)
        {
            CharacterTracker.Instance.player.transform.position = respawnpointPlayer.transform.position;
            CharacterTracker.Instance.player.gameObject.SetActive(true);
        }
    }
    public IEnumerator EnemyRespawnCoroutine()
    {
        CharacterTracker.Instance.enemyPlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawntime);
        if ((!gameOver) || GameOverContinue)
        {
            CharacterTracker.Instance.enemyPlayer.transform.position = respawnpointEnemyPlayer.transform.position;
            CharacterTracker.Instance.enemyPlayer.gameObject.SetActive(true);
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
    public float GetOpponentSpawnZ(CombatUtils.Team Team)
    {
        return Team == CombatUtils.Team.Player ? respawnpointEnemyPlayer.transform.position.z : respawnpointPlayer.transform.position.z;
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
    internal void InitializeCharacters()
    {
        friendlyBase.AddComponent<FriendlyBase>().Init();
        enemyBase.AddComponent<EnemyBase>().Init();
        CharacterTracker.Instance.player.Init();
        CharacterTracker.Instance.enemyPlayer.Init();
        foreach (GameObject enemyTower in CharacterTracker.Instance.allEnemiesTowers)
            enemyTower.GetComponent<TowerBehaviourEnemy>().Init();
        foreach (GameObject friendlyTower in CharacterTracker.Instance.allFriendliesTowers)
            friendlyTower.GetComponent<TowerBehaviourFriendly>().Init();
    }
}