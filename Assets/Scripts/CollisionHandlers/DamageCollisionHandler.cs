using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class DamageCollisionHandler : CollisionHandler
{
    protected override void HandleEnduringDamage(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent?.isEnduring == true)
        {
            if (CombatUtils.CanDamage(damageComponent, Owner))
            {
                if (collider.gameObject.GetComponent<BulletBehaviourFollowingUlt>() != null)
                {
                    collider.gameObject.GetComponent<BulletBehaviourFollowingUlt>().count -= Time.deltaTime;
                }
                if (damageComponent.givesXP)
                {
                    Owner.SetLastHit(true);
                }
                RaiseOnHitCallback();
                if (CombatUtils.DealDamage(damageComponent, Owner))
                {
                    Owner.Kill();
                }
            }
        }
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent != null && !damageComponent.isEnduring)
        {
            if (CombatUtils.CanDamage(damageComponent, Owner) != damageComponent.isHealing)
            {
                if (!damageComponent.isHealing)
                {
                    if (damageComponent.givesXP)
                    {
                        Owner.SetLastHit(true);
                    }
                    RaiseOnHitCallback();
                    if (CombatUtils.DealDamage(damageComponent, Owner))
                    {
                        Owner.Kill();
                    }
                }
                else
                {
                    DamageableEntity bulletOwner = collider.GetComponent<BulletBehaviourFollowing>()?.Owner;
                    if (Owner == bulletOwner)
                    {
                        return;
                    }
                    if (Owner.GetHealth().Heal(damageComponent))
                    {
                        if (bulletOwner is MainPlayerBehaviour)
                        {
                            CharacterTracker.Instance.GetPlayer(Owner.Team).OnHealXP();
                        }
                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }
}