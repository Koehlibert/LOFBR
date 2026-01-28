using UnityEngine;

public static class CombatUtils
{
    public enum Team
    {
        Player,
        Enemy
    }
    public static bool CanDamage(Team source, Team target)
    {
        return source != target;
    }
    public static bool CanDamage(DamageableEntity sourceObject, DamageableEntity targetObject)
    {
        return CanDamage(sourceObject.Team, targetObject.Team);
    }
    public static bool CanDamage(Damage sourceDamage, DamageableEntity targetObject)
    {
        return CanDamage(sourceDamage.sourceTeam, targetObject.Team);
    }
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
    public static bool DealDamage(float damageValue, IMortal target)
    {
        Damage damage = new GameObject().AddComponent<Damage>();
        damage.SetDamage(damageValue);
        bool result = damage.DealDamage(target);
        GameObject.Destroy(damage.gameObject);
        return result;
    }
}
