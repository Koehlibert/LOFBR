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
    private IMortal mortalTarget;
    private DamageableEntity damageableTarget;

    private void OnEnable()
    {
        mortalTarget = GetComponent<IMortal>();
        damageableTarget = GetComponent<DamageableEntity>();
    }

    public void AddRule(CollisionRule rule)
    {
        collisionRules.Add(rule);
    }
    private void OnTriggerEnter(Collider collision)
    {
        HandleDamageCollision(collision);
    }
    private void HandleDamageCollision(Collider collision)
    {
        Damage damageComponent = collision.gameObject.GetComponent<Damage>();
        HealingBullet heal = collision.gameObject.GetComponent<HealingBullet>();
        if (damageComponent != null)
        {
            if (CombatUtils.CanDamage(damageComponent, damageableTarget) != (heal != null)) //this is bad
            {
                if (heal == null)
                {
                    if (damageComponent.givesXP)
                    {
                        damageableTarget.SetLastHit(true);
                    }
                    OnHitCallback?.Invoke();
                    if (CombatUtils.DealDamage(damageComponent, mortalTarget))
                    {
                        mortalTarget.Die();
                    }
                }
                else
                {
                    //somehow this needs to give xp
                    if (mortalTarget.GetHealth().Heal(damageComponent))
                    {
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
    }
}