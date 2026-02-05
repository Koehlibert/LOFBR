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
    private GameObject closestCurrentEnemy;
    private ClosestFinder closestFinder;
    private float animSpeed;
    private float reloadtime;
    private bool loaded;
    [SerializeField] Image healthbar;
    [SerializeField] Image healthbarbg;
    protected HumanBodyBones Bone;
    protected override void Start()
    {
        base.Start();
        LastHit = false;
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        EnemyTeam = CombatUtils.GetOpposingTeam(Team);
        closestCurrentEnemy = null;
        enemybase = MasterScript.Instance.GetOpponentBase(EnemyTeam);
        closestFinder = new ClosestFinder(Team, this.gameObject);
        hpsys.Initialize(100, 0, 0, 0);
        loaded = true;
        reloadtime = 1.5f;
        Bone = HumanBodyBones.RightLowerLeg;
        bulletinstance = BulletFactory.Instance.CreateBullet(this, true, Bone);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
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
    }
    public void OnHealBulletHit(Damage damageComponent, GameObject bulletObject)
    {
        if (!hpsys.FullHP())
        {
            hpsys.Heal(damageComponent);
            player.levelsys.gainExp(5);
            Destroy(bulletObject);
        }
    }
    protected override void Die()
    {
        if ((player != null) && LastHit)
        {
            if (player.gameObject.activeSelf)
            {
                player.levelsys.gainExp(5);
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
        if (bulletrig)
        {
            bulletrig.transform.position = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset;
        }
        if (player == null)
        {
            player = MasterScript.Instance.GetOpponentPlayer(EnemyTeam);
        }
        closestCurrentEnemy = closestFinder.FindClosest();
        if (closestCurrentEnemy == null)
        {
            closestCurrentEnemy = enemybase;
        }
        else
        {
            float distance = Vector3.Distance(closestCurrentEnemy.transform.position, transform.position);
            float distToSpawn = Math.Abs(MasterScript.Instance.GetOpponentSpawnZ(EnemyTeam));
            if ((distance <= followdistance) && (distance > attackdistance))
            {
                nmAgent.enabled = true;
                nmAgent.SetDestination(closestCurrentEnemy.transform.position);
                animSpeed = 0.5f;
            }
            else if (distance < attackdistance)
            {
                nmAgent.enabled = false;
                Attack(closestCurrentEnemy.transform.position);
                animSpeed = 0;
            }
            else if (distToSpawn > 0)
            {
                transform.Translate(standarddirection * movementSpeed * Time.deltaTime, Space.World);
                animSpeed = 1;
            }
        }
        animator.SetFloat("speedPercent", animSpeed);
    }
    public void getShanked(Damage damage)
    {
        LastHit = true;
        if (CombatUtils.DealDamage(damage, this))
        {
            Die();
        }
    }
    public void Attack(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        if (loaded)
        {
            StartCoroutine("Shootanim");
            StartCoroutine("Reload");
        }
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = BulletFactory.Instance.CreateBullet(this, true, Bone);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        animator.Play("Default", 0, 0f);
    }
    private IEnumerator Shootanim()
    {
        animator.Play("Shoot", 0, 0f);
        yield return new WaitForSeconds(0.1f);
        bulletinstance.GetComponent<BulletBehaviour>().Shoot(GetDamageInfo());
        bulletrig = null;
        StartCoroutine("Resetanim");
    }
    private DamageInfo GetDamageInfo()
    {
        return new DamageInfo(40, 0, this.Team, true);
    }
}