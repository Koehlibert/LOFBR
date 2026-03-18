using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Extensions;
public class EnemyPlayerBehaviour : MainPlayerBehaviour
{
    public float reloadtime;
    public float followdistance;
    private float attackdistance;
    private float playerdistance;
    private float distance;
    private GameObject enemy;
    private float movementSpeed = 12;
    public NavMeshAgent nmAgent;
    public GameObject bullet;
    Vector3 offset = new Vector3(0, -0.5f, -1.5f);
    public string enemytype;
    public GameObject closestCurrentEnemy;
    private float animSpeed;
    private GameObject bulletinstance;
    private Rigidbody bulletrig;
    private GameObject bulletinstance2;
    private Rigidbody bulletrig2;
    private bool loaded;
    private bool hurt;
    private float circledirection;
    private float avoidDistance;
    private bool isShocking;
    private DetectBullets bulletdetector;
    public GameObject shockwave;
    private float reloadtimeShock;
    private bool loadedShock;
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
    protected override void Start()
    {
        base.Start();
        attackdistance = 20;
        hpsys.Initialize(300, 3, 4, 5);
        nmAgent = gameObject.GetComponent<NavMeshAgent>();
        reloadtime = 1.5f;
        enemytype = "Friendly";
        healthbar.fillAmount = hpsys.healthDisplay();
        manaBar.fillAmount = manasys.getPercent();
        loaded = true;
        hurt = false;
        circledirection = 1;
        avoidDistance = 15;
        isShocking = false;
        loadedShock = true;
        loadedShield = true;
        reloadtimeShock = 6;
        bulletdetector = FindAnyObjectByType<DetectBullets>();
        loadedUlt = false;
        aIHandler = gameObject.AddComponent<AIHandler>();
        ShootRightBasic shooter = gameObject.AddComponent<ShootRightBasic>();
        shooter.SetAttackDistance(20);
        UltAttack ultAttack = gameObject.AddComponent<UltAttack>();
        Stomp stomp = gameObject.AddComponent<Stomp>();
        aIHandler.Init(this, new List<Ability> { shooter, ultAttack, stomp }, new List<AIModule>(), movementSpeed, true);
    }
    void OnEnable()
    {
        StartCoroutine("Firstbullet");
        loadedShock = true;
        loadedShield = true;
        loadedUlt = true;
    }
    private IEnumerator Firstbullet()
    {
        yield return new WaitForSeconds(.4f);
        loaded = true;
        bulletinstance = BulletFactory.Instance.CreateBullet(this, true, HumanBodyBones.RightLowerLeg);
        bulletrig = bulletinstance.GetComponent<Rigidbody>();
        if (Levelsys.CheckLevel(4))
        {
            bulletinstance2 = BulletFactory.Instance.CreateBullet(this, true, HumanBodyBones.LeftLowerLeg);
            bulletrig2 = bulletinstance2.GetComponent<Rigidbody>();
        }
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Enemy;
    protected override void Die()
    {
        if (EnemyPlayer != null && LastHit)
        {
            if (EnemyPlayer.gameObject.activeSelf)
            {
                EnemyPlayer.Levelsys.GainExp(5 + 5 * Levelsys.GetLevel());
            }
        }
        if (bulletinstance)
        {
            bulletinstance.GetComponent<BulletBehaviour>().DelayedDestroy();
        }
        if (bulletinstance2)
        {
            bulletinstance2.GetComponent<BulletBehaviour>().DelayedDestroy();
        }
        LastHit = false;
        hurt = false;
        loaded = true;
        loadedShock = true;
        isShocking = false;
        base.Die();
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        UpdateBars();
        /* if (MasterScript.Instance.timeCounter % 150 == 0)
        {
            circledirection *= -1;
        }
        closestCurrentEnemy = closestFinder.FindClosest();
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
                    if (transform.position.x <= -19)
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
                    if (transform.position.z >= MasterScript.Instance.friendlySpawn.GetZPos() + 10)
                    {
                        transform.Translate(standarddirection * movementSpeed * Time.deltaTime, Space.World);
                        animSpeed = 1;
                    }
                }
            }
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
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveZ", animSpeed); */
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
    public override void LevelUp()
    {
        reloadtime *= 0.95f;
        hpsys.UpdateValues((Levelsys.GetLevel() - 1) * 25, 0.5f);
        movementSpeed++;
        nmAgent.speed += 2;
        manasys.UpdateValues(1.2f, 1.35f);
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
        wave.GetComponent<Damage>().SetProperties(GetShockDamage());
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
        if ((Levelsys.CheckLevel(3)) && (loadedShield) && manasys.checkCost(120))
        {
            shieldInstance = Instantiate(shield, transform.position, transform.rotation);
            shieldInstance.GetComponent<Shield>().SetOwner(this);
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
    private DamageInfo GetShockDamage()
    {
        return new DamageInfo(70 + (Levelsys.GetLevel() - 2) * 6, 0, this.Team, true);
    }
}
