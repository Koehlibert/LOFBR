using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class DamageCollisionHandler : CollisionHandler
{
    protected override void HandleDamageStay(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent?.isEnduring == true)
        {
            HandleEnduringDamage(damageComponent, collider);
        }
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        BulletBehaviourPossessing bulletBehaviourPossessing = collider.GetComponent<BulletBehaviourPossessing>();
        if (bulletBehaviourPossessing != null)
        {
            HandlePossessionBullet(bulletBehaviourPossessing);
        }
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent != null && !damageComponent.isEnduring)
        {
            if (CombatUtils.CanDamage(damageComponent, Owner) != damageComponent.isHealing)
            {
                if (!damageComponent.isHealing)
                {
                    HandleOneTimeDamage(damageComponent);
                }
                else
                {
                    HandleHealing(damageComponent, collider);
                }
            }
        }
    }
    private void HandleOneTimeDamage(Damage damageComponent)
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
    private void HandleHealing(Damage damageComponent, Collider collider)
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
    private void HandleEnduringDamage(Damage damageComponent, Collider collider)
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
    private void HandlePossessionBullet(BulletBehaviourPossessing bulletBehaviourPossessing)
    {
        if (CombatUtils.CanDamage(bulletBehaviourPossessing.team, Owner.Team))
        {
            
        }
        else
        {
            
        }
    }
}