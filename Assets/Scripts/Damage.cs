using UnityEngine;

public class Damage : MonoBehaviour
{
    private float damage;
    private float poison;
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
    public (float poisonValue, float damageValue) GetDamage()
    {
        return (poison, damage);
    }
    public (float poisonValue, float damageValue) GetDamage(float val)
    {
        return (poison*val, damage*val);
    }
    public bool DealDamage(IMortal mortalObject)
    {
        return(mortalObject.GetHealth().TakeDamage(this));
    }
}
