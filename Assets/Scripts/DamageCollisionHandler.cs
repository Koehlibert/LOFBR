using System.Collections.Generic;
using UnityEngine;
using Extensions;

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
    private System.Action<GameObject> onHitCallback;
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
    
    public void SetOnHitCallback(System.Action<GameObject> callback)
    {
        onHitCallback = callback;
    }

    /* private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject, collision, CollisionEventType.Enter);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision.gameObject, collision, CollisionEventType.Stay);
    }

    private void OnTriggerEnter(Collider collision)
    {
        HandleCollision(collision.gameObject, null, CollisionEventType.TriggerEnter);
    } */
    private void OnTriggerStay(Collider collision)
    {
        //HandleCollision(collision.gameObject, null, CollisionEventType.TriggerStay);
        HandleDamageCollision(collision);
    }
    private void HandleDamageCollision(Collider collision)
    {
        Damage damageComponent = collision.gameObject.GetComponent<Damage>();
        if (damageComponent != null)
        {
            if(CombatUtils.CanDamage(damageComponent, damageableTarget))
            {
                if (damageComponent.givesXP)
                {
                    Debug.Log("XP Set");
                    damageableTarget.SetLastHit(true);
                }
                if (CombatUtils.DealDamage(damageComponent, mortalTarget))
                {
                    mortalTarget.Die();
                }
            }
        }
    }
    /* private void HandleCollision(GameObject other, Collision collision, CollisionEventType eventType)
    {
        foreach (var rule in collisionRules)
        {
            if (rule.eventType == eventType && other.HasAnyTag(rule.tags))
            {
                Damage damageComponent = other.GetComponent<Damage>();
                if (other.CompareTag("BulletHealFriendly") && damageableTarget != null)
                {
                    FriendlyBehaviour friendlyTarget = damageableTarget as FriendlyBehaviour;
                    if (friendlyTarget != null)
                    {
                        friendlyTarget.OnHealBulletHit(damageComponent, other);
                    }
                    if (rule.destroyOnHit)
                        Destroy(other);
                    return;
                }
                if (rule.setLastHit && damageableTarget != null)
                {
                    damageableTarget.SetLastHit(true);
                }
                if (other.CompareTag("Fire"))
                {
                    float damageValue = damageComponent.GetDamage(Time.deltaTime).damageValue;
                    if (damageComponent && CombatUtils.DealDamage(damageValue, mortalTarget))
                    {
                        mortalTarget.Die();
                    }
                }
                else
                {
                    if (damageComponent && CombatUtils.DealDamage(damageComponent, mortalTarget))
                    {
                        mortalTarget.Die();
                    }
                }
                onHitCallback?.Invoke(other);
                if (rule.destroyOnHit)
                {
                    Destroy(other);
                }
            }
        }
    } */
}