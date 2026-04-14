using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class MasterScript : MonoBehaviour
{
    public static MasterScript Instance;
    public int respawntime;
    public bool gameOver;
    public bool victory;
    public int baseMaxHp;
    public RawImage defeatImage;
    public RawImage victoryImage;
    public GameObject moverFriendly;
    public GameObject moverEnemy;
    public GameObject HUD;
    public GameObject GameOverMenu;
    public GameObject GameOverContinue;
    public float timeCounter;
    private bool continueBool;
    public Vector3[] TowerPos = new Vector3[]
    {
    new Vector3(20, 0, 25),
    new Vector3(20, 0, 50),
    new Vector3(20, 0, 75),
    new Vector3(20, 0, 100),
    new Vector3(-20, 0, 25),
    new Vector3(-20, 0, 50),
    new Vector3(-20, 0, 75),
    new Vector3(-20, 0, 100)
    };
    private void Awake()
    {
        Instance = this;
        victoryImage.enabled = false;
        defeatImage.enabled = false;
        GameOverMenu.SetActive(false);
        GameOverContinue.SetActive(false);
        continueBool = false;
        AreaManagerFriendly.Instance.Init();
        AreaManagerEnemy.Instance.Init();
        CharacterTracker.Instance.Init();
    }
    void Update()
    {
        timeCounter++;
        if (gameOver && (!continueBool))
        {
            GameOverMenu.SetActive(true);
            GameOverContinue.SetActive(true);
            AreaManagerFriendly.Instance.Spawner.SetEnabled(false);
            AreaManagerEnemy.Instance.Spawner.SetEnabled(false);
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
            Mathf.Clamp(target.x, AreaManagerFriendly.Instance.lowerAreaLimitX, AreaManagerFriendly.Instance.upperAreaLimitX),
                        target.y,
            Mathf.Clamp(target.z, AreaManagerFriendly.Instance.Spawner.GetZPos(), AreaManagerEnemy.Instance.Spawner.GetZPos()));
    }
    public Vector3 CorrectTarget(Vector3 target, float border)
    {
        return new Vector3(
            Mathf.Clamp(target.x, AreaManagerFriendly.Instance.lowerAreaLimitX + border, AreaManagerFriendly.Instance.upperAreaLimitX - border),
                        target.y,
            Mathf.Clamp(target.z, AreaManagerFriendly.Instance.Spawner.GetZPos() + border, AreaManagerEnemy.Instance.Spawner.GetZPos()) - border);
    }
    public void DieAndRespawn(MainPlayerBehaviour mainPlayer)
    {
        StartCoroutine(RespawnCoroutine(mainPlayer));
        if (mainPlayer.Team == CombatUtils.Team.Player)
        {
            AreaManagerFriendly.Instance.StartShrink();
        }
        else
        {
            AreaManagerEnemy.Instance.StartShrink();
        }
    }
    public IEnumerator RespawnCoroutine(MainPlayerBehaviour mainPlayer)
    {
        if (mainPlayer.Team == CombatUtils.Team.Player)
        {
            AudioManager.Instance.PlayerDies();
        }
        mainPlayer.gameObject.SetActive(false);
        yield return new WaitForSeconds(respawntime);
        if ((!gameOver) || GameOverContinue)
        {
            mainPlayer.transform.position = mainPlayer.Team == CombatUtils.Team.Player ? AreaManagerFriendly.Instance.RespawnPoint.transform.position : AreaManagerEnemy.Instance.RespawnPoint.transform.position;
            mainPlayer.gameObject.SetActive(true);
            mainPlayer.ResetAfterDeath();
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
        AreaManagerFriendly.Instance.Spawner.SetEnabled(true);
        AreaManagerEnemy.Instance.Spawner.SetEnabled(true);
        defeatImage.enabled = false;
        victoryImage.enabled = false;
    }
    public float GetOpponentSpawnZ(CombatUtils.Team Team)
    {
        return Team == CombatUtils.Team.Player ? AreaManagerEnemy.Instance.RespawnPoint.transform.position.z : AreaManagerFriendly.Instance.RespawnPoint.transform.position.z;
    }
    public GameObject GetOpponentBase(CombatUtils.Team enemyTeam)
    {
        return enemyTeam == CombatUtils.Team.Enemy ? CharacterTracker.Instance.friendlyBase : CharacterTracker.Instance.enemyBase;
    }
    internal void InitializeCharacters()
    {
        CharacterTracker.Instance.friendlyBase.GetComponent<Base>().Init(CombatUtils.Team.Player);
        CharacterTracker.Instance.enemyBase.GetComponent<Base>().Init(CombatUtils.Team.Enemy);
        CharacterTracker.Instance.player.Init();
        CharacterTracker.Instance.enemyPlayer.Init();
    }
}