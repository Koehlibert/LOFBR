using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public abstract class CollisionHandler : MonoBehaviour
{
    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();
    private HashSet<GameObject> objectsEnteredThisFrame = new HashSet<GameObject>();
    public event Action OnHitCallback;
    public enum CollisionEventType { Enter, Stay, TriggerStay, TriggerEnter }
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
    private void OnTriggerEnter(Collider collider)
    {
        GameObject colliderObject = collider.gameObject;
        if (!objectsEnteredThisFrame.Contains(colliderObject))
        {
            objectsEnteredThisFrame.Add(colliderObject);
            HandleDamageCollision(collider);
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        GameObject colliderObject = collider.gameObject;
        if (!objectsInTrigger.Contains(colliderObject))
        {
            objectsInTrigger.Add(colliderObject);
            HandleEnduringDamage(collider);
        }
    }
    void LateUpdate()
    {
        objectsInTrigger.Clear();
        objectsEnteredThisFrame.Clear();
    }
    protected abstract void HandleEnduringDamage(Collider collider);
    protected abstract void HandleDamageCollision(Collider collider);
}