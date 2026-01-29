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
            if (CombatUtils.CanDamage(damageComponent, damageableTarget)) //this is bad
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
        }
    }
    private void HandleDamageCollision(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        HealingBullet heal = collider.gameObject.GetComponent<HealingBullet>();
        if (damageComponent != null && !damageComponent.isEnduring)
        {
            if (CombatUtils.CanDamage(damageComponent, damageableTarget) != (heal != null)) //this is bad
            {
                if (heal == null)
                {
                    if (damageComponent.givesXP)
                    {
                        damageableTarget.SetLastHit(true);
                    }
                    if (collider.gameObject.GetComponent<UltBulletBehaviour>() != null || collider.gameObject.GetComponent<UltBulletBehaviourEnemy>() != null)
                    {
                        if (collider.gameObject.GetComponent<UltBulletBehaviour>() != null)
                        {
                            collider.gameObject.GetComponent<UltBulletBehaviour>().count -= 1;
                        }
                        if (collider.gameObject.GetComponent<UltBulletBehaviourEnemy>() != null)
                        {
                            collider.gameObject.GetComponent<UltBulletBehaviourEnemy>().count -= 1;
                        }
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
                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }
}