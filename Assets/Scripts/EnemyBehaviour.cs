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
    private float movementSpeed = 12;
    private Vector3 standarddirection = new Vector3(0f, 0f, -1f);
    private NavMeshAgent nmAgent;
    public GameObject bullet;
    private Rigidbody bulletrig;
    private GameObject bulletinstance;
    Vector3 offset = new Vector3(0f, 0f, -1f);
    public string enemytype;
    public GameObject closestCurrentEnemy;
    private ClosestFinder closestFinder;
    [SerializeField] private Animator animator;
    private float animSpeed;
    private float reloadtime;
    private bool loaded;
    public Image healthbar;
    public Image healthbarbg;
    protected override void Start()
    {
        base.Start();
        rend = GetComponent<Renderer>();
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindAnyObjectByType<PlayerController>();
        enemytype = "Friendly";
        closestCurrentEnemy = null;
        closestFinder = new ClosestFinder(player, this.gameObject, master);
        enemybase = GameObject.FindWithTag(enemytype + "Base");
        loaded = true;
        reloadtime = 1.5f;
        hpsys.Initialize(100, 0, 0, 0);
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
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
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
        master = FindAnyObjectByType<MasterScript>();
        master.AddEnemy(this.gameObject);
    }
    void OnDisable()
    {
        master.RemoveEnemy(this.gameObject);
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (bulletrig)
        {
            bulletrig.transform.position = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset;
        }
        if (player == null)
        {
            player = GameObject.FindAnyObjectByType<PlayerController>();
        }
        closestCurrentEnemy = closestFinder.FindClosestFriend();
        if (closestCurrentEnemy == null)
        {
            if (Vector3.Distance(this.transform.position, enemybase.transform.position) <= attackdistance)
            {
                attack(enemybase.transform.position);
                animSpeed = 0;
            }
            else if (transform.position.z >= master.respawnpointPlayer.transform.position.z)
            {
                transform.Translate(standarddirection * movementSpeed * Time.deltaTime, Space.World);
                animSpeed = 1;
            }
        }
        else
        {
            distance = Vector3.Distance(closestCurrentEnemy.transform.position, transform.position);
            if ((distance <= followdistance) && (distance > attackdistance))
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
                transform.Translate(standarddirection * movementSpeed * Time.deltaTime, Space.World);
                animSpeed = 1;
            }
        }
        animator.SetFloat("speedPercent", animSpeed);
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
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
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
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
        animator.Play("Default", 0, 0f);
    }
    private IEnumerator shootanim()
    {
        animator.Play("Shoot", 0, 0f);
        yield return new WaitForSeconds(0.1f);
        bulletinstance.GetComponent<Damage>().SetProperties(40, 0, this.Team, true);
        bulletrig.AddForce(gameObject.transform.forward * 200000f * Time.deltaTime);
        bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        bulletrig = null;
        StartCoroutine("resetanim");
    }
}