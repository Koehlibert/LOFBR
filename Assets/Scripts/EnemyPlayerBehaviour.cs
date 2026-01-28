using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
public class EnemyPlayerBehaviour : DamageableEntity, IMainPlayer
{
    public Level levelsys;
    public float reloadtime;
    public GameObject enemybase;
    public PlayerController player;
    public float followdistance;
    private float attackdistance;
    private float playerdistance;
    private float distance;
    private GameObject enemy;
    private float movementSpeed = 12;
    private Vector3 standarddirection;
    public NavMeshAgent nmAgent;
    public GameObject bullet;
    Vector3 offset = new Vector3(0, -0.5f, -1.5f);
    public string enemytype;
    public GameObject closestCurrentEnemy;
    private GameObject yourbase;
    private float animSpeed;
    [SerializeField] private Animator animator;
    private GameObject bulletinstance;
    private Rigidbody bulletrig;
    private GameObject bulletinstance2;
    private Rigidbody bulletrig2;
    private bool loaded;
    private bool hurt;
    private float circledirection;
    private float avoidDistance;
    private bool isShocking;
    public checkShockWave detector;
    public checkShockWave detector2;
    private DetectBullets bulletdetector;
    public GameObject shockwave;
    private float reloadtimeShock;
    private bool loadedShock;
    private Vector3 closestposNoTower;
    private Vector3 secondClosestposNoTower;
    private Vector3 target;
    public GameObject shield;
    private bool loadedShield;
    private float reloadtimeShield = 6;
    private float reloadtimeUlt = 12;
    private bool loadedUlt;
    public GameObject BulletUlt;
    public Image healthbar;
    public Image manaBar;
    private GameObject shieldInstance;
    public Mana manasys;
    private ClosestFinder closestFinder;
    protected override void Start()
    {
        base.Start();
        attackdistance = 20;
        manasys = GetComponent<Mana>();
        levelsys = GetComponent<Level>();
        hpsys.Initialize(300, 3, 4, 5);
        standarddirection = new Vector3(0f, 0f, -1f);
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        reloadtime = 1.5f;
        player = GameObject.FindAnyObjectByType<PlayerController>();
        enemytype = "Friendly";
        healthbar.fillAmount = hpsys.healthDisplay();
        manaBar.fillAmount = manasys.getPercent();
        closestCurrentEnemy = null;
        enemybase = GameObject.FindWithTag(enemytype + "Base");
        yourbase = GameObject.FindWithTag("EnemyBase");
        closestFinder = new ClosestFinder(player, this.gameObject);
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        loaded = true;
        hurt = false;
        circledirection = 1;
        avoidDistance = 15;
        isShocking = false;
        loadedShock = true;
        loadedShield = true;
        reloadtimeShock = 6;
        detector.enabled = false;
        detector2.enabled = false;
        bulletdetector = FindAnyObjectByType<DetectBullets>();
        loadedUlt = false;
    }
    void OnEnable()
    {
        if (animator)
        {
            bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
            bulletrig = bulletinstance.GetComponent<Rigidbody>();
            if (levelsys)
            {
                if (levelsys.checkLevel(4))
                {
                    bulletinstance2 = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + offset, transform.rotation);
                    bulletrig2 = bulletinstance2.GetComponent<Rigidbody>();
                }
            }
        }
        loadedShock = true;
        loadedShield = true;
        loadedUlt = true;
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
    public override void Die()
    {
        if (player != null && LastHit)
        {
            if (player.gameObject.activeSelf)
            {
                player.levelsys.gainExp(5 + 5 * levelsys.getLevel());
            }
        }
        if (bulletinstance)
        {
            bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        }
        if (bulletinstance2)
        {
            bulletinstance2.GetComponent<DestroyAfterTime>().DelayedDestroy();
        }
        LastHit = false;
        hurt = false;
        loaded = true;
        loadedShock = true;
        isShocking = false;
        MasterScript.Instance.EnemyDieAndRespawn();
    }
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    public Transform GetTransform()
    {
        return this.transform;
    }
    public override Health GetHealth()
    {
        return hpsys;
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        UpdateBars();
        if (MasterScript.Instance.timeCounter % 150 == 0)
        {
            circledirection *= -1;
        }
        UpdateBullets();
        closestCurrentEnemy = closestFinder.FindClosestFriend();
        CheckUlt();
        if (loadedShock)
        {
            MoveShockCheckColliders();
        }
        hurt = CheckHurt();
        if ((hurt) && (transform.position.z <= yourbase.transform.position.z - 5))
        {
            if (!closestCurrentEnemy)
            {
                nmAgent.enabled = false;
            }
            else
            {
                Vector3 closestpos = closestCurrentEnemy.transform.position;
                if (Vector3.Distance(closestpos, transform.position) <= 40f)
                {
                    nmAgent.enabled = false;
                    transform.Translate(-standarddirection * movementSpeed * Time.deltaTime, Space.World);
                    animSpeed = 0.5f;
                    distance = Vector3.Distance(closestCurrentEnemy.transform.position, transform.position);
                    if (distance < attackdistance)
                    {
                        Attack(closestpos);
                        animSpeed = 0;
                    }
                }
            }
        }
        else if (!isShocking)
        {
            if (closestCurrentEnemy == null)
            {
                if (Vector3.Distance(this.transform.position, enemybase.transform.position) <= attackdistance)
                {
                    Attack(enemybase.transform.position);
                    animSpeed = 0;
                }
                else
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
                    animSpeed = 1;
                }
                else if (distance < attackdistance)
                {
                    nmAgent.enabled = false;
                    if (transform.position.x <= 1)
                    {
                        circledirection = 1;
                    }
                    else if (transform.position.x >= 19)
                    {
                        circledirection = -1;
                    }
                    if ((distance <= avoidDistance) && (transform.position.z <= yourbase.transform.position.z - 5))
                    {
                        Vector3 dir = transform.position - closestCurrentEnemy.transform.position;
                        if ((transform.position.x <= 1) || (transform.position.x >= 19))
                        {
                            dir = new Vector3(0, 0, 1);
                        }
                        transform.Translate(new Vector3(dir.x, 0, 1).normalized * movementSpeed * Time.deltaTime, Space.World);
                        Attack(closestCurrentEnemy.transform.position);
                        animSpeed = 1;
                    }
                    else
                    {
                        transform.Translate(circledirection * 0.25f * movementSpeed * Time.deltaTime, 0, 0, Space.World);
                        Attack(closestCurrentEnemy.transform.position);
                        animSpeed = 1;
                    }
                }
                else
                {
                    if (transform.position.z >= MasterScript.Instance.friendlySpawn.getZPos() + 10)
                    {
                        transform.Translate(standarddirection * movementSpeed * Time.deltaTime, Space.World);
                        animSpeed = 1;
                    }
                }
            }
            animator.SetFloat("speedPercent", animSpeed);
        }
        else if ((isShocking) && (loadedShock))
        {
            if (!closestCurrentEnemy)
            {
                nmAgent.enabled = false;
                isShocking = false;
                return;
            }
            if (Vector3.Distance(transform.position, closestCurrentEnemy.transform.position) <= 2.5)
            {
                Shock();
                isShocking = false;
                nmAgent.enabled = false;
            }
            else
            {
                if (Vector3.Distance(transform.position, closestCurrentEnemy.transform.position) <= 7.5)
                {
                    UseShield();
                }
                nmAgent.enabled = true;
                nmAgent.SetDestination(target);
                animSpeed = 1;
            }
        }
    }
    public void Attack(Vector3 target)
    {
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        if ((loaded) && manasys.checkCost(5))
        {
            StartCoroutine("Shootanim");
            StartCoroutine("Reload");
            manasys.useMana(5);
        }
    }
    void LevelUp()
    {
        reloadtime *= 0.95f;
        hpsys.UpdateValues((levelsys.getLevel() - 1) * 25, 0.5f);
        movementSpeed++;
        nmAgent.speed += 2;
        manasys.UpdateValues(1.2f, 1.35f);
        if (levelsys.checkLevel(2))
        {
            detector.enabled = true;
            detector2.enabled = true;
            reloadtimeShock *= 0.9f;
        }
    }
    private IEnumerator Reload()
    {
        loaded = false;
        yield return new WaitForSeconds(reloadtime);
        bulletinstance = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset, transform.rotation);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        loaded = true;
        if (levelsys.checkLevel(4))
        {
            bulletinstance2 = Instantiate(bullet, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + offset, transform.rotation);
            bulletrig2 = bulletinstance2.GetComponent<Rigidbody>();
        }
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
        bulletinstance.GetComponent<Damage>().SetProperties(34 + 7 * levelsys.getLevel(), 0, this.Team, true, true);
        bulletrig.AddForce(gameObject.transform.forward * 200000f * Time.deltaTime);
        bulletinstance.GetComponent<DestroyAfterTime>().DelayedDestroy();
        bulletrig = null;
        if (bulletrig2)
        {
            bulletrig2.transform.position = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + offset;
            bulletinstance2.GetComponent<Damage>().SetProperties(34 + 7 * levelsys.getLevel(), 0, this.Team, true, true);
            bulletrig2.AddForce(gameObject.transform.forward * 200000f * Time.deltaTime);
            bulletinstance2.GetComponent<DestroyAfterTime>().DelayedDestroy();
            bulletrig2 = null;
        }
        StartCoroutine("Resetanim");
    }
    private IEnumerator ReloadShock()
    {
        loadedShock = false;
        yield return new WaitForSeconds(reloadtimeShock);
        loadedShock = true;
    }
    void Shock()
    {
        GameObject wave = Instantiate(shockwave, transform.position + new Vector3(0f, 0.4f, 0f), transform.rotation);
        wave.GetComponent<Damage>().SetProperties(70 + (levelsys.getLevel() - 2) * 6, 0, this.Team, false, true);
        isShocking = false;
        loadedShock = false;
        manasys.useMana(75);
        StartCoroutine("ReloadShock");
    }
    public void GoShock(Vector3 location)
    {
        if ((loadedShock) && manasys.checkCost(75))
        {
            target = location;
            isShocking = true;
        }
    }
    public void UseShield()
    {
        if ((levelsys.checkLevel(3)) && (loadedShield) && manasys.checkCost(120))
        {
            shieldInstance = Instantiate(shield, transform.position, transform.rotation);
            shieldInstance.GetComponent<Shield>().SetPlayer(this);
            manasys.useMana(120);
            StartCoroutine("ReloadShield");
            StartCoroutine("DestroyShield");
        }
    }
    private void UpdateBars()
    {
        float hpval = hpsys.healthDisplay();
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, hpval, 5f * Time.deltaTime);
        manaBar.fillAmount = manasys.getPercent();
    }
    private void UpdateBullets()
    {
        if (bulletrig)
        {
            bulletrig.transform.position = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position + offset;
        }
        if (bulletrig2)
        {
            bulletrig2.transform.position = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position + offset;
        }
    }
    private IEnumerator ReloadShield()
    {
        loadedShield = false;
        yield return new WaitForSeconds(reloadtimeShield);
        loadedShield = true;
    }
    private IEnumerator ReloadUlt()
    {
        loadedUlt = false;
        yield return new WaitForSeconds(reloadtimeUlt);
        loadedUlt = true;
    }
    private IEnumerator DestroyShield()
    {
        hpsys.AddArmor(100);
        yield return new WaitForSeconds(1.5f);
        hpsys.AddArmor(-100);
        GameObject.Destroy(shieldInstance);
    }
    private void CheckUlt()
    {
        if ((MasterScript.Instance.allFriendlies.Count >= 4) && (loadedUlt) && (levelsys.checkLevel(5)) && manasys.checkCost(250))
        {
            GameObject ultInstance = Instantiate(BulletUlt, transform.position + transform.forward + new Vector3(0, 2, 0), transform.rotation);
            ultInstance.gameObject.GetComponent<Damage>().SetProperties(50 + (levelsys.getLevel() - 5) * 4.5f, 0, this.Team, false, true);
            StartCoroutine("ReloadUlt");
            manasys.useMana(250);
        }
    }
    private void MoveShockCheckColliders()
    {
        if (levelsys.getLevel() > 1)
        {
            GameObject[] closeEns = closestFinder.FindTwoClosestFriendlies();
            if (closeEns[0])
            {
                detector.enabled = true;
                closestposNoTower = closeEns[0].transform.position;
                detector.gameObject.transform.position = closestposNoTower;
                if (closeEns[1])
                {
                    detector2.enabled = true;
                    secondClosestposNoTower = closeEns[1].transform.position;
                    detector2.gameObject.transform.position = secondClosestposNoTower;
                }
                else
                {
                    detector2.enabled = false;
                }
            }
            else
            {
                detector.enabled = false;
            }
        }
        else
        {
            detector.enabled = false;
            detector2.enabled = false;
        }
    }
    private bool CheckHurt()
    {
        bool returnBool = hurt;
        if (!hurt && hpsys.healthDisplay() <= 0.25f)
        {
            returnBool = true;
        }
        if (hurt && (hpsys.healthDisplay() >= 0.4))
        {
            returnBool = false;
        }
        return returnBool;
    }
}
