using System;
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
    public void Init(Damage damagingDamage, Damage healing, CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        base.Init(owner, destroyOnHit, bone, timer);
        DamagingDamage = damagingDamage;
        HealingDamage = healing;
    }
    public void IncreaseCounter()
    {
        NumberOfHits++;
    }
    public void HitAction(DamageableEntity hitCharacter)
    {
        if (hitCharacter.Team == Owner.Team)
        {
            HealingAction(hitCharacter);
        }
        else
        {
            DamagingAction(hitCharacter);
        }
        IncreaseCounter();
    }
    private void DamagingAction(DamageableEntity hitCharacter)
    {
        damage = DamagingDamage;
    }
    public Damage GetActiveDamage()
    {
        return damage;
    }
    public void HealingAction(DamageableEntity hitCharacter)
    {
        damage = HealingDamage;
        if (hitCharacter is CharacterBehaviour characterBehaviour)
        {
           ActiveCharacterManager.Instance.ChangeActiveCharacter(characterBehaviour);
        }
        hitCharacter.DeathEvent += ActiveCharacterManager.Instance.ResetActiveCharacter;
    }
}
