using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
public class EnemyBehaviour : DamageableEntity
{
    public GameObject enemybase;
    Renderer rend;
    public PlayerController player;
    public float followdistance;
    public float attackdistance;
    private float playerdistance;
    private float distance;
    private GameObject enemy;
    private float movementSpeed = 12;
    private Vector3 standarddirection = new Vector3(0f,0f,-1f);
    private NavMeshAgent nmAgent;
    public GameObject bullet;
    private Rigidbody bulletrig;
    private GameObject bulletinstance;
    Vector3 offset = new Vector3(0f,0f,-1f);
    public string enemytype;
    public GameObject closestCurrentEnemy;
    private ClosestFinder closestFinder;
    Animator animator;
    private float animSpeed;
    private float reloadtime;
    private bool loaded;
    public Image healthbar;
    public Image healthbarbg;
    protected override void Start()
    {
        base.Start();
        LastHit = false;
        rend = GetComponent<Renderer>();
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindObjectOfType<PlayerController>();
        enemytype = "Friendly";
        closestCurrentEnemy = null;
        closestFinder = new ClosestFinder(player, this.gameObject, master);
        enemybase = GameObject.FindWithTag(enemytype + "Base");
        animator = GetComponent<Animator>();
        loaded = true;
        reloadtime = 1.5f;
        hpsys.Initialize(100,0,0,0);
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
            tags = new List<string> { "Bullet", },
            eventType = DamageCollisionHandler.CollisionEventType.Enter,
            destroyOnHit = true,
            setLastHit = false
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletPlayer", },
            eventType = DamageCollisionHandler.CollisionEventType.Enter,
            destroyOnHit = true,
            setLastHit = true
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "MeleePlayer" },
            eventType = DamageCollisionHandler.CollisionEventType.Enter,
            destroyOnHit = false,
            setLastHit = true
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "UltBulletFriendly" },
            eventType = DamageCollisionHandler.CollisionEventType.Stay,
            destroyOnHit = false,
            setLastHit = true
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "Fire" },
            eventType = DamageCollisionHandler.CollisionEventType.TriggerStay,
            destroyOnHit = false,
            setLastHit = true
        });
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "MeleePlayer", "BulletPlayerShockwave"},
            eventType = DamageCollisionHandler.CollisionEventType.TriggerEnter,
            destroyOnHit = false,
            setLastHit = true
        });
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
        master.AddEnemy(this.gameObject);
    }
    void OnDisable()
    {
        master.RemoveEnemy(this.gameObject);
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if (bulletrig)
        {
            bulletrig.transform.position = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset;
        }
        if (player == null)
        {
            player = GameObject.FindObjectOfType<PlayerController>();
        }
        closestCurrentEnemy = closestFinder.FindClosestFriend();
        if (closestCurrentEnemy == null)
        {
            if (Vector3.Distance(this.transform.position,enemybase.transform.position)<=attackdistance)
            {
                attack(enemybase.transform.position);
                animSpeed = 0;
            }
            else if (transform.position.z >= master.respawnpointPlayer.transform.position.z)
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
                attack(closestCurrentEnemy.transform.position);
                animSpeed = 0;
            }
            else if (transform.position.z >= master.respawnpointPlayer.transform.position.z)
            {
                transform.Translate(standarddirection*movementSpeed*Time.deltaTime,Space.World);
                animSpeed = 1;
            }
        }
        animator.SetFloat("speedPercent",animSpeed);
    }
    public void getShanked(Damage damage)
    {
        LastHit = true;
        if (CombatUtils.DealDamage(damage, this))
        {
            Die();
        }
    }
    public void attack(Vector3 target)
    {   
        transform.LookAt(new Vector3(target.x,transform.position.y,target.z));
        if (loaded)
        {
            StartCoroutine("shootanim");
            StartCoroutine("reload");
        }
    }
    private IEnumerator reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        loaded = true;
    }
    private IEnumerator resetanim()
    {
        yield return new WaitForSeconds(0.25f);
        animator.Play("Default",0,0f);
    }
    private IEnumerator shootanim()
    {
        animator.Play("Shoot",0,0f);
        yield return new WaitForSeconds(0.1f);
        bulletinstance.GetComponent<Damage>().SetDamage(40,0);
        bulletrig.AddForce(gameObject.transform.forward*200000f*Time.deltaTime);
        bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        bulletrig = null;
        StartCoroutine("resetanim");
    }
}