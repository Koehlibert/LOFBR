using System.ComponentModel.Design;
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
    public static void DealDamage(GameObject damageObject, DamageableEntity target)
    {
        damageObject
            .GetComponent<Damage>()
            .DealDamage(target);
    }
    public static void DealDamage(Collision collision, DamageableEntity target)
    {
        DealDamage(collision.gameObject, target);
    }
    public static void DealDamage(Collider collider, DamageableEntity target)
    {
        DealDamage(collider.gameObject, target);
    }
    public static void DealDamage(Damage damage, DamageableEntity target)
    {
        damage.DealDamage(target);
    }
    public static float GetDistance(GameObject object1, GameObject object2)
    {
        return Vector3.Distance(object2.transform.position,object1.transform.position);
    }
    public static bool InRange(GameObject object1, GameObject object2, float range)
    {
        bool isInRange = false;
        if ((object2 != null)&&(GetDistance(object1, object2) <=range))
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
