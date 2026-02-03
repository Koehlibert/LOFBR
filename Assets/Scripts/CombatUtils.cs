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
    public static bool InRange(GameObject object1, GameObject object2, float range)
    {
        bool isInRange = false;
        if ((object2 != null)&&(Vector3.Distance(object2.transform.position,object1.transform.position)<=range))
        {
            isInRange = true;
        }
        return isInRange;
    }
    public static Team GetOpposingTeam(Team team)
    {
        return team == Team.Enemy ? Team.Player : Team.Enemy;
    }
}
