using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const float poisonDuration = 10f;
    private float maxhp;
    private float hp;
    private float healthRegen;
    private float timer;
    private float regenTime;
    private float armor;
    public System.Action<float> OnHealthChanged;
    public event Action Death;
    void Update()
    {
        if (timer <= regenTime)
        {
            timer += Time.deltaTime;
        }
        else if (hp < maxhp)
        {
            hp = Mathf.Min(hp + healthRegen * Time.deltaTime, maxhp);
            OnHealthChanged?.Invoke(healthDisplay());
        }
    }
    public void Initialize(float max, float regen, float regenTimeVar, float armval)
    {
        maxhp = max;
        hp = maxhp;
        healthRegen = regen;
        regenTime = regenTimeVar;
        armor = armval;
    }
    public void Initialize((float max, float regen, float regenTimeVar, float armval) var)
    {
        maxhp = var.max;
        hp = maxhp;
        healthRegen = var.regen;
        regenTime = var.regenTimeVar;
        armor = var.armval;
    }
    public void UpdateValues(float gain, float regenGain)
    {
        maxhp += gain;
        hp += gain;
        healthRegen = Mathf.Max(0, healthRegen + regenGain);
    }
    public void SetArmor(float newArm)
    {
        armor = newArm;
    }
    public void AddArmor(float armGain)
    {
        armor += armGain;
    }
    public void TakeDamage(Damage damageObj)
    {
        float damageValue = damageObj.GetDamage();
        TakeDamage(damageValue);
        
    }
    public void TakeDamage(float damageValue)
    {
        float damage = computeDamage(damageValue);
        hp -= damage;
        timer = 0;
        OnHealthChanged?.Invoke(healthDisplay());
        if (hp <= 0)
        {
            Death?.Invoke();
        }
    }
    public float healthDisplay()
    {
        return hp / maxhp;
    }
    public bool Heal(Damage damageComponent)
    {
        bool isDamaged = !this.FullHP();
        if (isDamaged)
        {
            hp = Mathf.Min(maxhp, hp + damageComponent.GetDamage());
            OnHealthChanged?.Invoke(healthDisplay());
        }
        return isDamaged;
    }
    public bool FullHP()
    {
        return hp == maxhp;
    }
    float computeDamage(float damageValue)
    {
        return Mathf.Max(damageValue * ((100 - armor) / 100), 0);
    }
    float computeDamage(Damage damageObject)
    {
        return Mathf.Max(damageObject.GetDamage() * ((100 - armor) / 100), 0) * (damageObject.isEnduring ? Time.deltaTime : 1f);
    }
    public void SetHPPercent(float healthPercent)
    {
        hp = maxhp * healthPercent;
    }
    public void superRegen(float superRegenValue)
    {
        hp = Mathf.Min(hp + superRegenValue * Time.deltaTime, maxhp);
        OnHealthChanged?.Invoke(healthDisplay());
    }
}
