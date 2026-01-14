using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
public class EnemyBehaviour : MonoBehaviour, IMortal
{
    public Health hpsys;
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
    private bool LastHit;
    Animator animator;
    private float animSpeed;
    private MasterScript master;
    private float reloadtime;
    private bool loaded;
    public Image healthbar;
    void Start()
    {
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
        hpsys = GetComponent<Health>();
        hpsys.Initialize(100,0,0,0);
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
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
        updateHealthBar();
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
    void OnCollisionEnter(Collision other)
    {
        if (other.HasAnyTag(new List<string>(){"Bullet","BulletPlayer", "BulletPlayerShockwave"}))
        {
            if (other.HasAnyTag(new List<string>(){"BulletPlayer","BulletPlayerShockwave"}))
            {
                LastHit = true;
            }
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
            if (other.HasAnyTag(new List<string>(){"Bullet","BulletPlayer"}))
            {
                Destroy(other.gameObject);
            }
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.HasAnyTag(new List<string>(){"UltBulletFriendly"}))
        {
            LastHit = true;
            other.gameObject.GetComponent<UltBulletBehaviour>().count--;
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.HasAnyTag(new List<string>(){"Fire"}))
        {
            LastHit = true;
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.HasAnyTag(new List<string>(){"MeleePlayer", "BulletPlayerShockwave"}))
        {
            LastHit = true;
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
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
    void updateHealthBar()
    {
        float hpval = hpsys.healthDisplay();
        if (healthbar.fillAmount > hpval)
        {
            healthbar.fillAmount = Mathf.Max(hpval, healthbar.fillAmount - Time.deltaTime);
        }
        if (healthbar.fillAmount < hpval)
        {
            healthbar.fillAmount = Mathf.Max(hpval, healthbar.fillAmount + Time.deltaTime);
        }
    }
    public void Die()
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
    public Health GetHealth()
    {
        return hpsys; 
    }
}