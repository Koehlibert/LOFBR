using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float maxhp;
    private float hp;
    private float healthRegen;
    private float timer;
    private float regenTime;
    private float armor;
    private bool superRegen;
    private float superRegenValue;
    private float poison;
    private float poisonTime = 15f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (superRegen)
        {
            hp = Mathf.Min(hp+superRegenValue*Time.deltaTime, maxhp);
            poison = 0;
        }
        if (poison > 0)
        {
            hp = Mathf.Max(0,hp - poison*Time.deltaTime);
            poisonTime -= Time.deltaTime;
            if (poisonTime<= 0f)
            {
                poison = 0;
            }
        }
        if (timer<=regenTime)
        {
            timer += Time.deltaTime;
        }
        else if ((hp<maxhp)&&(poison == 0))
        {
            hp = Mathf.Min(hp+healthRegen*Time.deltaTime, maxhp);
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
    public void UpdateValues(float gain, float regenGain)
    {
        maxhp += gain;
        hp += gain;
        healthRegen = Mathf.Max(0,healthRegen + regenGain);
    }
    public void SetArmor(float newArm)
    {
        armor = newArm;
    }
    public void AddArmor(float armGain)
    {
        armor = Mathf.Max(0,armor + armGain);
        armor = Mathf.Min(100,armor);
    }
    public bool TakeDamage((float poisonValue, float damageValue) val)
    {
        poison = Mathf.Max(val.poisonValue, poison);
        float damage = computeDamage(val.damageValue);
        hp -= damage;
        timer = 0;
        if (hp <= 0)
        {
            hp = maxhp;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool TakeDamage(float damageValue)
    { 
        float damage = computeDamage(damageValue);
        hp -= damage;
        timer = 0;
        if (hp <= 0)
        {
            hp = maxhp;
            return true;
        }
        else
        {
            return false;
        }
    }
    public float healthDisplay()
    {
        return hp/maxhp;
    }
    public void ActivateSuperRegen(float val)
    {
        superRegen = true;
        superRegenValue = val;
    }
    public void DeactivateSuperRegen()
    {
        superRegen = false;
    }
    public void Heal((float x, float healValue) val)
    {
        hp = Mathf.Min(maxhp, hp + val.healValue);
    }
    public bool FullHP()
    {
        return hp == maxhp;
    }
    float computeDamage(float damageValue)
    {
        return Mathf.Max(damageValue * ((100 - armor) / 100), 0);
    }
}
