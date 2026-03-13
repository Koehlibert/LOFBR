using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
using System;
public abstract class MobBehaviour : DamageableEntity
{
    protected GameObject enemybase;
    protected MainPlayerBehaviour player;
    protected float followdistance = 25;
    protected float attackdistance = 10;
    private float movementSpeed = 12;
    private Vector3 standarddirection = new Vector3(0f, 0f, 1f);
    private NavMeshAgent nmAgent;
    private GameObject bulletinstance;
    private Rigidbody bulletrig;
    Vector3 offset = new Vector3(0f, 0f, 1f);
    protected CombatUtils.Team EnemyTeam;
    private ClosestFinder closestFinder;
    private float animSpeed;
    [SerializeField] Image healthbar;
    [SerializeField] Image healthbarbg;
    protected HumanBodyBones Bone;
    protected override void Start()
    {
        base.Start();
        LastHit = false;
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        EnemyTeam = CombatUtils.GetOpposingTeam(Team);
        enemybase = MasterScript.Instance.GetOpponentBase(EnemyTeam);
        closestFinder = new ClosestFinder(Team, this.gameObject);
        hpsys.Initialize(100, 0, 0, 0);
        Bone = HumanBodyBones.RightLowerLeg;
        healthbar.gameObject.SetActive(false);
        healthbarbg.gameObject.SetActive(false);
        MasterScript.Instance.AddMob(this);
        hpsys.OnHealthChanged += (healthPercent) =>
        {
            healthbar.gameObject.SetActive(true);
            healthbarbg.gameObject.SetActive(true);
            healthbar.fillAmount = healthPercent;
        };
        if (Team == CombatUtils.Team.Enemy)
        {
            offset.z *= -1;
            standarddirection.z *= -1;
        }
        aIHandler = gameObject.AddComponent<AIHandler>();
        ShootRightBasic shooter = gameObject.AddComponent<ShootRightBasic>();
        aIHandler.Init(this, new List<Ability>{shooter}, new List<AIModule>(), movementSpeed, false);
    }
    public void OnHealBulletHit(Damage damageComponent, GameObject bulletObject)
    {
        if (!hpsys.FullHP())
        {
            hpsys.Heal(damageComponent);
            player.Levelsys.GainExp(5);
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
        if (bulletinstance)
        {
            bulletinstance.GetComponent<BulletBehaviour>().DelayedDestroy();
        }
        Destroy(this.gameObject);
        MasterScript.Instance.RemoveMob(this);
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
    }
    protected void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (player == null)
        {
            player = MasterScript.Instance.GetOpponentPlayer(Team);
        }
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveZ", animSpeed);
    }
    public void getShanked(Damage damage)
    {
        LastHit = true;
        if (CombatUtils.DealDamage(damage, this))
        {
            Die();
        }
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        animator.Play("Default", 0, 0f);
    }
}