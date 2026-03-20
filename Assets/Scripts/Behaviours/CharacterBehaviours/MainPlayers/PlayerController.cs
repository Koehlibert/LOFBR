using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MainPlayerBehaviour
{
    private float movementspeed;
    public int rotatespeed;
    private float flashspeed;
    private Image DamageImage;
    public Color flashcolor = new(1f, 0f, 0f, 0.1f);
    public AudioSource soundsource;
    private bool isDead = false;
    public override void Init()
    {
        ClassID = PlayerPrefs.GetInt("classID");
        base.Init(ClassID);
        DamageImage = HUD.Instance.DamageImage;
        flashspeed = 2.5f;
        EnableDamageFlash();
    }
    public override CombatUtils.Team Team => CombatUtils.Team.Player;
    void FixedUpdate()
    {
        StackingHandler.PushAwayFromNearbyObjects(this.gameObject);
        if (PlayerInputRouter.Instance.CheatedPressed)
        {
            hpsys.AddArmor(1000);
            Levelsys.GainExp(100);
            EnemyPlayer.Levelsys.GainExp(100);
        }
        UpdateDamageImage();
    }
    void UpdateDamageImage()
    {
        if (!isDead)
        {
            DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, flashspeed * Time.deltaTime);
        }
    }
    private void OnTakeDamage()
    {
        if (!isDead)
        {
            flashcolor.a = 0.8f * (1 - hpsys.healthDisplay());
            DamageImage.color = flashcolor;
            soundsource.time = 0.4f;
            soundsource.Play();
        }
    }
    public float GetSpeed()
    {
        return movementspeed;
    }
    protected override void Die()
    {
        isDead = true;
        flashcolor.a = 0.8f;
        DamageImage.color = flashcolor;
        base.Die();
    }
    public void DisableDamageFlash()
    {
        CollisionHandler.OnHitCallback -= OnTakeDamage;
    }
    public void EnableDamageFlash()
    {
        CollisionHandler.OnHitCallback += OnTakeDamage;
    }
    public void OnHealXP()
    {
        Levelsys.GainExp(5);
    }
}
