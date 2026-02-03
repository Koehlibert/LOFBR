using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;
    [SerializeField] GameObject BulletPrefab;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject CreateBullet(DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = Instantiate(BulletPrefab, owner.animator.GetBoneTransform(bone).position, owner.gameObject.transform.rotation);
        BulletBehaviour bulletBehaviour = BulletInstance.GetComponent<BulletBehaviour>();
        bulletBehaviour.Init(owner, destroyOnHit, bone, timer);
        return(BulletInstance);
    }
}