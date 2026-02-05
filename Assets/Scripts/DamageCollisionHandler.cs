using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;

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
    private DamageableEntity damageableTarget;

    private void OnEnable()
    {
        damageableTarget = GetComponent<DamageableEntity>();
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
            if (CombatUtils.CanDamage(damageComponent, damageableTarget))
            {
                if (collider.gameObject.GetComponent<BulletBehaviourFollowingUlt>() != null)
                {
                    collider.gameObject.GetComponent<BulletBehaviourFollowingUlt>().count -= Time.deltaTime;
                }
                if (damageComponent.givesXP)
                {
                    damageableTarget.SetLastHit(true);
                }
                OnHitCallback?.Invoke();
                if (CombatUtils.DealDamage(damageComponent, damageableTarget))
                {
                    damageableTarget.Kill();
                }
            }
        }
    }
    private void HandleDamageCollision(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent != null && !damageComponent.isEnduring)
        {
            if (CombatUtils.CanDamage(damageComponent, damageableTarget) != damageComponent.isHealing) //this is bad
            {
                if (!damageComponent.isHealing)
                {
                    if (damageComponent.givesXP)
                    {
                        damageableTarget.SetLastHit(true);
                    }
                    OnHitCallback?.Invoke();
                    if (CombatUtils.DealDamage(damageComponent, damageableTarget))
                    {
                        damageableTarget.Kill();
                    }
                }
                else
                {
                    if (damageableTarget.GetHealth().Heal(damageComponent))
                    {
                        MasterScript.Instance.player.OnHealXP();
                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }
}