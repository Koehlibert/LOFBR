using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;
    [SerializeField] GameObject BulletPrefab;
    public GameObject CreateBullet(DamageableEntity owner, bool destroyOnHit, Damage damage, Vector3 spawnlocation, float timer = 1.5f)
    {
        GameObject BulletInstance = Instantiate(BulletPrefab, spawnlocation, owner.gameObject.transform.rotation);
        BulletBehaviour bulletBehaviour = BulletInstance.AddComponent<BulletBehaviour>();
        bulletBehaviour.Init(owner, destroyOnHit, damage, timer);
        return(BulletInstance);
    }
}