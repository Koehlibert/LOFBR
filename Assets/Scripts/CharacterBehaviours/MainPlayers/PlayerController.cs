using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Extensions;

[RequireComponent(typeof(SkillsetMelee))]
[RequireComponent(typeof(SkillsetSupport))]
[RequireComponent(typeof(SkillsetFighter))]
[RequireComponent(typeof(DamageCollisionHandler))]
public class PlayerController : MainPlayerBehaviour
{
    public Mana manasys;
    private float movementspeed;
    public int rotatespeed;
    private float flashspeed;
    public Vector3 movement;
    public Image damageimage;
    public EnemyPlayerBehaviour enemyPlayer;
    public Color flashcolor = new Color(1f, 0f, 0f, 0.1f);
    private float animSpeed;
    public AudioSource soundsource;
    private bool moveLock = false;
    private bool lookLock;
    private int classID;
    private Skillset skillSet;
    private bool isDead = false;
    private DamageCollisionHandler handler;
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
        handler = GetComponent<DamageCollisionHandler>();
        EnableDamageFlash();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    void OnEnable()
    {
        moveLock = false;
        isDead = false;
        damageimage.color = Color.clear;
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (PlayerInputRouter.Instance.CheatedPressed)
        {
            hpsys.AddArmor(1000);
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
        if (!lookLock)
        {
            Vector2 mouseScreenPosition = PlayerInputRouter.Instance.Look;
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            if (playerPlane.Raycast(ray, out float hitDist))
            {
                Vector3 lookAtPoint = ray.GetPoint(hitDist);

                Vector3 dir = lookAtPoint - transform.position;
                dir.y = 0;

                if (dir.sqrMagnitude > 0.001f)
                {
                    transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                }
            }
        }
    }
    private void OnTakeDamage()
    {
        if (!isDead)
        {
            flashcolor.a = 0.8f * (1 - hpsys.healthDisplay());
            damageimage.color = flashcolor;
            soundsource.time = 0.4f;
            soundsource.Play();
        }
    }
    void MoveCharacter()
    {
        movement = new Vector3(-PlayerInputRouter.Instance.Move.y, 0, PlayerInputRouter.Instance.Move.x).normalized;
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
        hpsys.UpdateValues((levelsys.getLevel() - 1) * 25, 2);
        manasys.UpdateValues(50, levelsys.getLevel() * 0.25f);
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
    protected override void Die()
    {
        isDead = true;
        flashcolor.a = 0.8f;
        damageimage.color = flashcolor;
        if (LastHit)
        {
            enemyPlayer.levelsys.gainExp(5 + 5 * levelsys.getLevel());
        }
        LastHit = false;
        base.Die();
    }
    public void DisableDamageFlash()
    {
        handler.OnHitCallback -= OnTakeDamage;
    }
    public void EnableDamageFlash()
    {
        handler.OnHitCallback += OnTakeDamage;
    }
    public void OnHealXP()
    {
        levelsys.gainExp(5);
    }
}
