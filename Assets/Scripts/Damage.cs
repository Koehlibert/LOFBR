using System;
using UnityEngine;
public class DamageInfo
{
    public float damageValue;
    public float poisonValue;
    public CombatUtils.Team sourceTeamValue;
    public bool lastHit = false;
    public bool enduring = false;
    public DamageInfo(float damageValue, float poisonValue, CombatUtils.Team sourceTeamValue, bool lastHit = false, bool enduring = false)
    {
        this.damageValue = damageValue;
        this.poisonValue = poisonValue;
        this.sourceTeamValue = sourceTeamValue;
        this.lastHit = lastHit;
        this.enduring = enduring;
    }
}
public class Damage : MonoBehaviour
{
    private float damage;
    private float poison;
    public CombatUtils.Team sourceTeam;
    public bool givesXP;
    public bool isEnduring;
    public event Action DamageDealt;
    public bool isHealing = false;
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
        poison = 0;
    }
    public (float damageValue, float poisonValue) GetDamage()
    {
        return (damage, poison);
    }
    public bool DealDamage(IMortal mortalObject)
    {
        DamageDealt?.Invoke();
        return mortalObject.GetHealth().TakeDamage(this);
    }
    public void SetProperties(DamageInfo damageInfo)
    {
        damage = damageInfo.damageValue;
        poison = damageInfo.poisonValue;
        sourceTeam = damageInfo.sourceTeamValue;
        givesXP = damageInfo.lastHit;
        isEnduring = damageInfo.enduring;
    }
    public void MakeHealing()
    {
        isHealing = true;
    }
}
