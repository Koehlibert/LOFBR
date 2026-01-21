using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
using System;
public class FriendlyBehaviour : DamageableEntity
{
    public GameObject enemybase;
    Renderer rend;
    public EnemyPlayerBehaviour player;
    public float followdistance;
    public float attackdistance;
    private float distance;
    private float movementSpeed = 12;
    private Vector3 standarddirection  = new Vector3(0f,0f,1f);
    private NavMeshAgent nmAgent;
    public GameObject bullet;
    private GameObject bulletinstance;
    private Rigidbody bulletrig;
    Vector3 offset = new Vector3(0,0f,1f);
    public string enemytype;
    public GameObject closestCurrentEnemy;
    Animator animator;
    private float animSpeed;
    private bool loaded;
    private float reloadtime;
    public Image healthbar;
    public Image healthbarbg;
    private ClosestFinder closestFinder;
    private float offsetFloor = 2f;
    protected override void Start()
    {
        base.Start();
        LastHit = false;
        rend = GetComponent<Renderer>();
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        enemytype = "Enemy";
        player = FindObjectOfType<EnemyPlayerBehaviour>();
        closestCurrentEnemy = null;
        enemybase = GameObject.FindWithTag(enemytype + "Base");
        animator = GetComponent<Animator>();
        closestFinder = new ClosestFinder(player, this.gameObject, master);
        hpsys.Initialize(100,0,0,0);
        loaded = true;
        reloadtime = 1.5f;
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        healthbar.gameObject.SetActive(false);
        healthbarbg.gameObject.SetActive(false);
        hpsys.OnHealthChanged += (healthPercent) =>
        {
            healthbar.gameObject.SetActive(true);
            healthbarbg.gameObject.SetActive(true);
            healthbar.fillAmount = healthPercent;
        };
    }
    protected override void ConfigureCollisionRules(DamageCollisionHandler handler)
    {
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletHealFriendly" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = false
        });
        
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletEnemy", "BulletEnemyPlayer", },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = true,
            setLastHit = true
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletEnemyShockwave" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = false,
            setLastHit = true
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "UltBulletEnemy" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerStay,
            setLastHit = true
        });
    }
    
    public void OnHealBulletHit(Damage damageComponent, GameObject bulletObject)
    {
        if (!hpsys.FullHP())
        {
            hpsys.Heal(damageComponent);
            master.player.levelsys.gainExp(5);
            Destroy(bulletObject);
        }
    }
    
    public override void Die()
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
            bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        }
        Destroy(this.gameObject);
    }
    
    public override Health GetHealth()
    {
        return hpsys;
    }
    void OnEnable()
    {
        master = FindObjectOfType<MasterScript>();
        master.AddFriendly(this.gameObject);
    }
    void OnDisable()
    {
        master.RemoveFriendly(this.gameObject);
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        transform.position = new Vector3(transform.position.x, offsetFloor, transform.position.z);
        if (bulletrig)
        {
            bulletrig.transform.position = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset;
        }
        if (player == null)
        {
            player = GameObject.FindObjectOfType<EnemyPlayerBehaviour>();
        }
        closestCurrentEnemy = closestFinder.FindClosestEnemy();
        if (closestCurrentEnemy == null)
        {
            if (Vector3.Distance(this.transform.position,enemybase.transform.position)<=attackdistance)
            {
                Attack(enemybase.transform.position);
                animSpeed = 0;
            }
            else if (transform.position.z <= master.respawnpointEnemyPlayer.transform.position.z)
            {
                transform.Translate(standarddirection*movementSpeed*Time.deltaTime,Space.World);
                animSpeed = 1;
            }
        }
        else
        {
            distance = Vector3.Distance(closestCurrentEnemy.transform.position, transform.position);
            if ((distance <= followdistance)&&(distance > attackdistance))
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
            else if (transform.position.z <= master.respawnpointEnemyPlayer.transform.position.z)
            {
                transform.Translate(standarddirection*movementSpeed*Time.deltaTime,Space.World);
                animSpeed = 1;
            }
        }
        animator.SetFloat("speedPercent",animSpeed);
    }
    public void Attack(Vector3 target)
    {   
        transform.LookAt(new Vector3(target.x,transform.position.y,target.z));
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
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        loaded = true;
    }
    private IEnumerator Resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        animator.Play("Default",0,0f);
    }
    private IEnumerator Shootanim()
    {
        animator.Play("Shoot",0,0f);
        yield return new WaitForSeconds(0.1f);
        bulletinstance.GetComponent<Damage>().SetDamage(40,0);
        bulletrig.AddForce(gameObject.transform.forward*200000f*Time.deltaTime);
        bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        bulletrig = null;
        StartCoroutine("Resetanim");
    }
}
