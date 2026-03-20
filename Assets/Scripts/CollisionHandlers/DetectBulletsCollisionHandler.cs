using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class DetectBulletsCollisionHandler : CollisionHandler
{
    protected int NBulletsToTrigger;
    private List<GameObject> BulletList;
    public event Action BulletsDetected;
    public void Init(DamageableEntity owner, int bulletstoTrigger)
    {
        base.Init(owner);
        BulletList = new List<GameObject>();
        NBulletsToTrigger = bulletstoTrigger;
    }
    protected override void HandleEnduringDamage(Collider collider)
    {
    }
    protected override void HandleDamageCollision(Collider collider)
    {
        Damage damageComponent = collider.gameObject.GetComponent<Damage>();
        if (damageComponent != null && !damageComponent.isEnduring)
        {
            if (CombatUtils.CanDamage(damageComponent, Owner) && !damageComponent.isHealing)
            {
                BulletList.Add(collider.gameObject);
            }
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (BulletList.Contains(collider.gameObject))
        {
            BulletList.Remove(collider.gameObject);
        }
    }
    void Update()
    {
        this.transform.SetPositionAndRotation(Owner.transform.position + new Vector3(0f,2f,0f), Owner.transform.rotation);
        BulletList.RemoveAll(item => item == null);
        if (BulletList.Count >= NBulletsToTrigger)
            BulletsDetected?.Invoke();
    }
}