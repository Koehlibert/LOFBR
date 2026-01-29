using UnityEngine;

public class Damage : MonoBehaviour
{
    private float damage;
    private float poison;
    public CombatUtils.Team sourceTeam;
    public bool DestroyOnHit;
    public bool givesXP;
    public bool isEnduring;
    public void SetDamage(float damageValue, float poisonValue)
    {
        damage = damageValue;
        poison = poisonValue;
    }
    public void SetDamage(float damageValue)
    {
        damage = damageValue;
        poison = 0;
    }
    public (float damageValue, float poisonValue) GetDamage()
    {
        return (damage, poison);
    }
    public (float damageValue, float poisonValue) GetDamage(float val)
    {
        return (damage*val, poison*val);
    }
    public bool DealDamage(IMortal mortalObject)
    {
        if(DestroyOnHit)
        {
            Destroy(this.gameObject);
        }
        return mortalObject.GetHealth().TakeDamage(this);
    }
    public void SetProperties(float damageValue, float poisonValue, CombatUtils.Team sourceTeamValue, bool destroyOnHit, bool lastHit = false, bool enduring = false)
    {
        damage = damageValue;
        poison = poisonValue;
        sourceTeam = sourceTeamValue;
        DestroyOnHit = destroyOnHit;
        givesXP = lastHit;
        isEnduring = enduring;
    }
}
