using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
public class PlayerController : DamageableEntity, IMainPlayer
{
    public Level levelsys;
    public Mana manasys; 
    private float movementspeed;
    public int rotatespeed;
    private float flashspeed;
    public Vector3 movement;
    public Image damageimage;
    public EnemyPlayerBehaviour enemyPlayer;
    public Color flashcolor = new Color(1f,0f,0f,0.1f);
    [SerializeField] public Animator animator;
    private float animSpeed;
    public AudioSource soundsource;
    private bool moveLock = false;
    private bool lookLock;
    private int classID;
    private Skillset skillSet;
    private bool isDead = false;
    protected override void Start()
    {
        base.Start();
        enemyPlayer = FindAnyObjectByType<EnemyPlayerBehaviour>();
        levelsys = GetComponent<Level>();
        manasys = GetComponent<Mana>();
        hpsys = GetComponent<Health>();
        LastHit = false;
        classID = PlayerPrefs.GetInt("classID");
        switch (classID)
        {
            case 1:
                skillSet = GetComponent<SkillsetFighter>();
                break;
            case 2:
                skillSet = GetComponent<SkillsetSupport>();
                break;
            case 3:
                skillSet = GetComponent<SkillsetMelee>();
                break;

        }
        skillSet.enabled = true;
        skillSet.BaseUnlock();
        var hpVals = skillSet.GetHPVals();
        hpsys.Initialize(hpVals.hpval, hpVals.regenval, hpVals.delay, hpVals.armorval);
        movementspeed = skillSet.GetSpeed();
        flashspeed = 2.5f;
        DamageCollisionHandler handler = GetComponent<DamageCollisionHandler>();
        handler.SetOnHitCallback(OnTakeDamage);
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    void OnEnable()
    {
        moveLock = false;
        isDead = false;
        damageimage.color = Color.clear;
    }
    /* protected override void ConfigureCollisionRules(DamageCollisionHandler handler)
    {
        handler.AddRule(new DamageCollisionHandler.CollisionRule
        {
            tags = new List<string> { "BulletEnemy", "BulletEnemyPlayer" },
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
    } */
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (Input.GetButtonDown("Cheat"))
        {
            levelsys.gainExp(100);
            enemyPlayer.levelsys.gainExp(100);
        }
        UpdateLookPosition();
        UpdateDamageImage();
        MoveCharacter();
    }
    void UpdateDamageImage()
    {
        if (!isDead)
        {
            damageimage.color = Color.Lerp(damageimage.color, Color.clear, flashspeed * Time.deltaTime);
        }
    }
    void UpdateLookPosition()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;
        if ((playerPlane.Raycast(ray, out hitDist)) && (!lookLock))
        {
            Vector3 lookAtPoint = ray.GetPoint(hitDist);
            Quaternion lookAtRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
            lookAtRotation.x = 0;
            lookAtRotation.z = 0;
            transform.rotation = lookAtRotation;
        }
    }
    private void OnTakeDamage(GameObject damageSource)
    {
        if (!isDead)
        {
            flashcolor.a = 0.8f*(1-hpsys.healthDisplay());
            damageimage.color = flashcolor;
        }
    }
    void MoveCharacter()
    {
        float moveSideways = Input.GetAxis("Horizontal");
        float moveForward = -Input.GetAxis("Vertical");
        float rotateSideways = Input.GetAxis("Vertical");
        movement = new Vector3(moveForward, 0, moveSideways);
        if (!moveLock)
        {
            MoveCharacter(movement);
        }
    }
    public void MoveCharacter(Vector3 direction)
    {
        animSpeed = direction.normalized.magnitude;
        animator.SetFloat("speedPercent", animSpeed);
        transform.position = MasterScript.Instance.CorrectTarget(transform.position + direction * movementspeed * Time.deltaTime);
    }
    public void MoveCharacter(Vector3 direction, float speedup)
    {
        MoveCharacter(direction * speedup);
    }
    void LevelUp()
    {
        skillSet.LevelUnlock(levelsys.getLevel());
        hpsys.UpdateValues((levelsys.getLevel()-1)*25,2);
        manasys.UpdateValues(50, levelsys.getLevel()*0.25f);
    }
    public float GetSpeed()
    {
        return movementspeed;
    }
    public IEnumerator LockMovement(float duration)
    {
        moveLock = true;
        yield return new WaitForSeconds(duration);
        moveLock = false;
    }
    public IEnumerator LockView(float duration)
    {
        lookLock = true;
        yield return new WaitForSeconds(duration);
        lookLock = false;
    }
    public override void Die()
    {
        isDead = true;
        flashcolor.a = 0.8f;
        damageimage.color = flashcolor;
        if (LastHit)
        {
            enemyPlayer.levelsys.gainExp(5 + 5 * levelsys.getLevel());
        }
        LastHit = false;
        MasterScript.Instance.DieAndRespawn();
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
}
