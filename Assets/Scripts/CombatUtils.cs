using UnityEngine;

public static class CombatUtils
{
    public static bool DealDamage(GameObject damageObject, IMortal target)
    {
        return damageObject
            .GetComponent<Damage>()
            .DealDamage(target);
    }
    public static bool DealDamage(Collision collision, IMortal target)
    {
        return DealDamage(collision.gameObject, target);
    }
    public static bool DealDamage(Collider collider, IMortal target)
    {
        return DealDamage(collider.gameObject, target);
    }
    public static bool DealDamage(Damage damage, IMortal target)
    {
        return damage.DealDamage(target);
    }
}
