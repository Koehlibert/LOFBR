using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public abstract class CollisionHandler : MonoBehaviour
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
    protected List<CollisionRule> collisionRules = new List<CollisionRule>();
    protected DamageableEntity Owner;
    protected void RaiseOnHitCallback()
    {
        var eh = OnHitCallback;
        eh?.Invoke();
    }
    public virtual void Init(DamageableEntity owner)
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
    protected abstract void HandleEnduringDamage(Collider collider);
    protected abstract void HandleDamageCollision(Collider collider);
}