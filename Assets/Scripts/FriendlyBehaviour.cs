using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
public class FriendlyBehaviour : MonoBehaviour, IMortal
{
    private Health hpsys;
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
    private bool LastHit;
    public GameObject closestCurrentEnemy;
    Animator animator;
    private float animSpeed;
    private MasterScript master;
    private bool loaded;
    private float reloadtime;
    public Image healthbar;
    private ClosestFinder closestFinder;
    void Start()
    {
        LastHit = false;
        rend = GetComponent<Renderer>();
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        enemytype = "Enemy";
        player = FindObjectOfType<EnemyPlayerBehaviour>();
        closestCurrentEnemy = null;
        enemybase = GameObject.FindWithTag(enemytype + "Base");
        animator = GetComponent<Animator>();
        hpsys = GetComponent<Health>();
        closestFinder = new ClosestFinder(player, this.gameObject, master);
        hpsys.Initialize(100,0,0,0);
        loaded = true;
        reloadtime = 1.5f;
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
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
        transform.position = new Vector3(transform.position.x,0,transform.position.z);
        float hpval = hpsys.healthDisplay();
        if (healthbar.fillAmount > hpval)
        {
            healthbar.fillAmount = Mathf.Max(hpval, healthbar.fillAmount - Time.deltaTime);
        }
        if (healthbar.fillAmount < hpval)
        {
            healthbar.fillAmount = Mathf.Max(hpval, healthbar.fillAmount + Time.deltaTime);
        }
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
    void OnCollisionEnter(Collision other)
    {
        if (other.HasAnyTag(new List<string>(){"BulletHealFriendly"}))
        {
            if(!hpsys.FullHP())
            {
                hpsys.Heal(other.gameObject.GetComponent<Damage>().GetDamage());
                Destroy(other.gameObject);
                master.player.levelsys.gainExp(2);
            }
        }
        else if (other.HasAnyTag(new List<string>(){"BulletEnemy","BulletEnemyPlayer","BulletEnemyShockwave"}))
        {
            if (other.HasAnyTag(new List<string>(){"BulletEnemyPlayer","BulletEnemyShockwave"}))
            {
                LastHit = true;
            }
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
            if (other.HasAnyTag(new List<string>(){"BulletEnemy","BulletEnemyPlayer"}))
            {
                Destroy(other.gameObject);
            }
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.HasAnyTag(new List<string>(){"UltBulletEnemy"}))
        {
            if (other == null) return;
            LastHit = true;
            other.gameObject.GetComponent<UltBulletBehaviour>().count--;
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.HasAnyTag(new List<string>(){"BulletEnemyShockwave"}))
        {
            LastHit = true;
            if (CombatUtils.DealDamage(other, this))
            {
                Die();
            }
        }
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
        return this.hpsys;
    }
}
