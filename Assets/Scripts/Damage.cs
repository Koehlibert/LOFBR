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
        return(mortalObject.GetHealth().TakeDamage(this));
    }
}
