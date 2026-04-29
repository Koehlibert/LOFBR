using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Damage))]
public class BulletBehaviourPossessing : BulletBehaviour
{
    public int NumberOfHits = 0;
    private Damage DamagingDamage;
    private Damage HealingDamage;
    public void Init(Damage damagingDamage, Damage healing, DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        base.Init(owner, destroyOnHit, bone, timer);
        DamagingDamage = damagingDamage;
        HealingDamage = healing;
    }
    public void IncreaseCounter()
    {
        NumberOfHits++;
    }
    public Damage GetDamage()
    {
        return DamagingDamage;
    }
    public Damage GetHealing()
    {
        return HealingDamage;
    }
}
