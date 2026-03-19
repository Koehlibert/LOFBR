using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;
using NUnit.Framework;

public class DamageCollisionHandler : MonoBehaviour
{
    [System.Serializable]
    public class CollisionRule
    {
        public List<string> tags = new List<string>();
        public CollisionEventType eventType;
        public bool destroyOnHit = false;
        public bool setLastHit = false;
    }
    public event Action OnHitCallback;
    public enum CollisionEventType { Enter, Stay, TriggerStay, TriggerEnter }
    private List<CollisionRule> collisionRules = new List<CollisionRule>();
    private DamageableEntity Owner;
    public void Init(DamageableEntity owner)
    {
        Owner = owner;
    }
    public void AddRule(CollisionRule rule)
    {
        collisionRules.Add(rule);
    }
    private void OnTriggerEnter(Collider collider)
    {
        HandleDamageCollision(collider);
    }
    private void OnTriggerStay(Collider collider)
    {
        HandleEnduringDamage(collider);
    }
    private void HandleEnduringDamage(Collider collider)
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
                OnHitCallback?.Invoke();
                if (CombatUtils.DealDamage(damageComponent, Owner))
                {
                    Owner.Kill();
                }
            }
        }
    }
    private void HandleDamageCollision(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent != null && !damageComponent.isEnduring)
        {
            Debug.Log(Owner);
            Debug.Log(damageComponent);
            if (CombatUtils.CanDamage(damageComponent, Owner) != damageComponent.isHealing)
            {
                if (!damageComponent.isHealing)
                {
                    if (damageComponent.givesXP)
                    {
                        Owner.SetLastHit(true);
                    }
                    OnHitCallback?.Invoke();
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
                        if(bulletOwner is MainPlayerBehaviour)
                        {
                            MasterScript.Instance.player.OnHealXP();
                        }
                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }
}