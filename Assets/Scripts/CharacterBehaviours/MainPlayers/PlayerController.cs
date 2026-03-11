using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Extensions;

public class PlayerController : MainPlayerBehaviour
{
    private float movementspeed;
    public int rotatespeed;
    private float flashspeed;
    public Image damageimage;
    public EnemyPlayerBehaviour enemyPlayer;
    public Color flashcolor = new Color(1f, 0f, 0f, 0.1f);
    public AudioSource soundsource;
    private int classID;
    private Skillset skillSet;
    private bool isDead = false;
    private DamageCollisionHandler handler;
    protected override void Start()
    {
        base.Start();
        enemyPlayer = FindAnyObjectByType<EnemyPlayerBehaviour>();
        manasys = GetComponent<Mana>();
        hpsys = GetComponent<Health>();
        LastHit = false;
        classID = PlayerPrefs.GetInt("classID");
        switch (classID)
        {
            case 1:
                skillSet = gameObject.AddComponent<SkillsetFighter>();
                break;
            case 2:
                skillSet = gameObject.AddComponent<SkillsetSupport>();
                break;
            case 3:
                skillSet = gameObject.AddComponent<SkillsetMelee>();
                break;
        }
        aIHandler = gameObject.AddComponent<AIHandler>();
        aIHandler.Init(this, new List<Ability>(), new List<AIModule>(), skillSet.GetSpeed());
        skillSet.Init(aIHandler);
        skillSet.LevelUnlock(1);
        var hpVals = skillSet.GetHPVals();
        hpsys.Initialize(hpVals.hpval, hpVals.regenval, hpVals.delay, hpVals.armorval);
        flashspeed = 2.5f;
        handler = GetComponent<DamageCollisionHandler>();
        EnableDamageFlash();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    void OnEnable()
    {
        isDead = false;
        damageimage.color = Color.clear;
    }
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (PlayerInputRouter.Instance.CheatedPressed)
        {
            hpsys.AddArmor(1000);
            Levelsys.GainExp(100);
            enemyPlayer.Levelsys.GainExp(100);
        }
        UpdateDamageImage();
    }
    void UpdateDamageImage()
    {
        if (!isDead)
        {
            damageimage.color = Color.Lerp(damageimage.color, Color.clear, flashspeed * Time.deltaTime);
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
    public override void LevelUp()
    {
        skillSet.LevelUnlock(Levelsys.GetLevel());
        hpsys.UpdateValues((Levelsys.GetLevel() - 1) * 25, 2);
        manasys.UpdateValues(50, Levelsys.GetLevel() * 0.25f);
    }
    public float GetSpeed()
    {
        return movementspeed;
    }
    protected override void Die()
    {
        isDead = true;
        flashcolor.a = 0.8f;
        damageimage.color = flashcolor;
        if (LastHit)
        {
            enemyPlayer.Levelsys.GainExp(5 + 5 * Levelsys.GetLevel());
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
        Levelsys.GainExp(5);
    }
}
