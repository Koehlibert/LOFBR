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
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviour bulletBehaviour = BulletInstance.AddComponent<BulletBehaviour>();
        bulletBehaviour.Init(owner, destroyOnHit, bone, timer);
        return BulletInstance;
    }
    public GameObject CreatePoisonBullet(DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviourFollowing bulletBehaviourPoison = BulletInstance.AddComponent<BulletBehaviourFollowing>();
        bulletBehaviourPoison.Init(owner, destroyOnHit, bone, false, true, CombatUtils.GetOpposingTeam(owner.Team));
        return BulletInstance;
    }
    public GameObject CreateHealingBullet(DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviourFollowing bulletBehaviourHeal = BulletInstance.AddComponent<BulletBehaviourFollowing>();
        bulletBehaviourHeal.Init(owner, destroyOnHit, bone, true, false, owner.Team);
        BulletInstance.GetComponent<Damage>().MakeHealing();
        return BulletInstance;
    }
    public GameObject CreateUltBullet(DamageableEntity owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviourFollowing bulletBehaviourUlt = BulletInstance.AddComponent<BulletBehaviourFollowingUlt>();
        BulletInstance.transform.localScale *= 2;
        bulletBehaviourUlt.Init(owner, destroyOnHit, bone, false, false, CombatUtils.GetOpposingTeam(owner.Team), 40, 25, 5, 10);
        return BulletInstance;
    }
    private GameObject InstantiateBullet(DamageableEntity owner, HumanBodyBones bone)
    {
        return Instantiate(BulletPrefab, owner.animator.GetBoneTransform(bone).position, owner.transform.rotation);
    }
}