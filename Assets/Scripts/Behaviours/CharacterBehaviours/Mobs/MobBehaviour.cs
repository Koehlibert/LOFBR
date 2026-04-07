using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.Services.Analytics;
public class MobBehaviour : DamageableEntity
{
    protected GameObject enemybase;
    protected MainPlayerBehaviour player;
    protected float followdistance = 25;
    protected float attackdistance = 10;
    private float movementSpeed = 12;
    private Vector3 standarddirection = new Vector3(0f, 0f, 1f);
    Vector3 offset = new Vector3(0f, 0f, 1f);
    private Renderer rend;
    protected CombatUtils.Team EnemyTeam;
    [SerializeField] Image healthbarbg;
    public override void Init()
    {
        base.Init();
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        rend.material = Team == CombatUtils.Team.Player
            ? MaterialLibrary.Instance.playerMaterial
            : MaterialLibrary.Instance.enemyMaterial;
        LastHit = false;
        EnemyTeam = CombatUtils.GetOpposingTeam(Team);
        enemybase = MasterScript.Instance.GetOpponentBase(EnemyTeam);
        hpsys.Initialize(100, 0, 0, 0);
        healthbar.gameObject.SetActive(false);
        healthbarbg.gameObject.SetActive(false);
        CharacterTracker.Instance.AddMob(this);
        hpsys.OnHealthChanged += (healthPercent) =>
        {
            healthbar.gameObject.SetActive(true);
            healthbarbg.gameObject.SetActive(true);
        };
        if (Team == CombatUtils.Team.Enemy)
        {
            offset.z *= -1;
            standarddirection.z *= -1;
        }
        aIHandler = gameObject.AddComponent<AIHandler>();
        ShootRightBasic shooter = gameObject.AddComponent<ShootRightBasic>();
        aIHandler.Init(this, new List<Ability> { shooter }, new List<AIModule>(), movementSpeed, false);
    }
    public void Init(CombatUtils.Team team)
    {
        this.Team = team;
        Init();
    }
    public void OnHealBulletHit(Damage damageComponent, GameObject bulletObject)
    {
        if (!hpsys.FullHP())
        {
            hpsys.Heal(damageComponent);
            CharacterTracker.Instance.GetPlayer(Team).Levelsys.GainExp(5);
            Destroy(bulletObject);
        }
    }
    protected override void Die()
    {
        if ((player != null) && LastHit)
        {
            if (player.gameObject.activeSelf)
            {
                player.Levelsys.GainExp(5);
            }
        }
        CharacterTracker.Instance.RemoveMob(this);
        Destroy(this.gameObject);
    }
    public void GetRezd()
    {
        aIHandler.LockAI(0.6f);
        animator.SetTrigger("GetRezd");
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
        if (ResetMarked)
        {
            ChangeOutlineAlpha(0);
            ResetMarked = false;
        }
        if (IsMarked)
        {
            IsMarked = false;
            ResetMarked = true;
        }
    }
    protected void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (player == null)
        {
            player = CharacterTracker.Instance.GetOpponentPlayer(Team);
        }
    }
    public void getShanked(Damage damage)
    {
        LastHit = true;
        if (CombatUtils.DealDamage(damage, this))
        {
            Die();
        }
    }
    public override void MarkHealthbar()
    {
        healthbar.gameObject.SetActive(true);
        healthbarbg.gameObject.SetActive(true);
        base.MarkHealthbar();
    }
}