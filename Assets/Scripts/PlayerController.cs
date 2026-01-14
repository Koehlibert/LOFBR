using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
public class PlayerController : MonoBehaviour, IMortal, IMainPlayer
{
    public Level levelsys;
    public Mana manasys; 
    public MasterScript master;
    private float movementspeed;
    public int rotatespeed;
    private float flashspeed;
    public Vector3 movement;
    public Image damageimage;
    private bool LastHit;
    public EnemyPlayerBehaviour enemyPlayer;
    public Color flashcolor = new Color(1f,0f,0f,0.1f);
    public Animator animator;
    private float animSpeed;
    public AudioSource soundsource;
    public Health hpsys;
    private bool moveLock = false;
    private bool lookLock;
    private int classID;
    private Skillset skillSet;
    void Start()
    {
        enemyPlayer = FindObjectOfType<EnemyPlayerBehaviour>();
        levelsys = GetComponent<Level>();
        manasys = GetComponent<Mana>();
        hpsys = GetComponent<Health>();
        animator = GetComponent<Animator>();
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
        flashspeed = 5f;
    }
    void OnEnable()
    {
        moveLock = false;
    }
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Cheat"))
        {
            levelsys.gainExp(100);
            enemyPlayer.levelsys.gainExp(100);
        }
        UpdateLookPosition();
        UpdateDamageImage();
        MoveCharakter(movement);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.HasAnyTag(new List<string>(){"BulletEnemy", "BulletEnemyPlayer", "BulletEnemyShockwave"}))
        {
            if (col.HasAnyTag(new List<string>(){"BulletEnemyPlayer", "BulletEnemyShockwave"}))
            {
                LastHit = true;
            }
            flashcolor.a = 0.8f*(1-hpsys.healthDisplay());
            damageimage.color = flashcolor;
            if (!CombatUtils.DealDamage(col, this))
            {
                soundsource.time = 0.35f;
                soundsource.Play();
            }
            else
            {
                Die();
            }
            if (!col.HasAnyTag(new List<string>(){"BulletEnemyShockwave"}))
            {
                Destroy(col.gameObject);
            }
        }
    }
    void UpdateDamageImage()
    {
        damageimage.color = Color.Lerp(damageimage.color, Color.clear, flashspeed * Time.deltaTime);
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
    void MoveCharakter(Vector3 movementv3)
    {
        float moveSideways = Input.GetAxis("Horizontal");
        float moveForward = -Input.GetAxis("Vertical");
        float rotateSideways = Input.GetAxis("Vertical");
        movement = new Vector3(moveForward, 0.0f, moveSideways);
        animSpeed = movement.normalized.magnitude;
        animator.SetFloat("speedPercent", animSpeed);
        if (!moveLock)
        {
            transform.Translate(movementv3*movementspeed*Time.deltaTime,Space.World);
        }
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
    public void Die()
    {
        if (LastHit)
        {
            enemyPlayer.levelsys.gainExp(5 + 5 * levelsys.getLevel());
        }
        LastHit = false;
        master.DieAndRespawn();
    }
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
    public Health GetHealth()
    {
        return hpsys;
    }
}
