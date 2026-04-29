using System;
using UnityEngine;
public class DamageInfo
{
    public float damageValue;
    public float poisonValue;
    public CombatUtils.Team sourceTeamValue;
    public bool lastHit = false;
    public bool enduring = false;
    public bool CanBeParried = false;
    public DamageInfo(float damageValue, CombatUtils.Team sourceTeamValue, bool lastHit = false, bool enduring = false, bool canBeParried = true)
    {
        this.damageValue = damageValue;
        this.sourceTeamValue = sourceTeamValue;
        this.lastHit = lastHit;
        this.enduring = enduring;
        this.CanBeParried = canBeParried;
    }
}
public class Damage : MonoBehaviour
{
    private float damage;
    public CombatUtils.Team sourceTeam;
    public bool givesXP;
    public bool isEnduring;
    public event Action<DamageableEntity> DamageDealt;
    public bool isHealing = false;
    public bool CanBeParried = false;
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }
    public float GetDamage()
    {
        return damage;
    }
    public void DealDamage(DamageableEntity mortalObject)
    {
        DamageDealt?.Invoke(mortalObject);
        mortalObject.GetHealth().TakeDamage(this);
    }
    public void SetProperties(DamageInfo damageInfo)
    {
        damage = damageInfo.damageValue;
        sourceTeam = damageInfo.sourceTeamValue;
        givesXP = damageInfo.lastHit;
        isEnduring = damageInfo.enduring;
        CanBeParried = damageInfo.CanBeParried;
    }
    public void MakeHealing()
    {
        isHealing = true;
    }
}
