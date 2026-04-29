using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public abstract class CollisionHandler : MonoBehaviour
{
    protected HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();
    protected HashSet<GameObject> objectsEntered = new HashSet<GameObject>();
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
        if (!objectsEntered.Contains(colliderObject))
        {
            objectsEntered.Add(colliderObject);
            HandleDamageCollision(collider);
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        GameObject colliderObject = collider.gameObject;
        if (!objectsInTrigger.Contains(colliderObject))
        {
            objectsInTrigger.Add(colliderObject);
            HandleDamageStay(collider);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        GameObject colliderObject = collider.gameObject;
        objectsEntered.Remove(colliderObject);
    }
    protected virtual void LateUpdate()
    {
        objectsInTrigger.Clear();
        objectsEntered.RemoveWhere(obj => obj == null);
    }
    protected abstract void HandleDamageStay(Collider collider);
    protected abstract void HandleDamageCollision(Collider collider);
}