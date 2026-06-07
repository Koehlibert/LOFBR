using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject ShockwavePrefab;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject Fire;
    [SerializeField] GameObject MeleeCollider;
    [SerializeField] GameObject ParryCollider;
    [SerializeField] GameObject ManaDrainer;
    [SerializeField] GameObject BulletDetector;
    [SerializeField] GameObject ArmorAura;
    [SerializeField] GameObject SuperRegenAura;
    [SerializeField] GameObject MoveCollider;
    private void Awake()
    {
        Instance = this;
    }
    public GameObject CreateBullet(CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviour bulletBehaviour = BulletInstance.AddComponent<BulletBehaviour>();
        bulletBehaviour.Init(owner, destroyOnHit, bone, timer);
        return BulletInstance;
    }
    public GameObject CreatePoisonBullet(CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviourFollowing bulletBehaviourPoison = BulletInstance.AddComponent<BulletBehaviourFollowing>();
        bulletBehaviourPoison.Init(owner, destroyOnHit, bone, false, true, CombatUtils.GetOpposingTeam(owner.Team));
        return BulletInstance;
    }
    public GameObject CreateHealingBullet(CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviourFollowing bulletBehaviourHeal = BulletInstance.AddComponent<BulletBehaviourFollowing>();
        bulletBehaviourHeal.Init(owner, destroyOnHit, bone, true, false, owner.Team);
        BulletInstance.GetComponent<Damage>().MakeHealing();
        return BulletInstance;
    }
    public GameObject CreateUltBullet(CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float timer = 1.5f)
    {
        GameObject BulletInstance = InstantiateBullet(owner, bone);
        BulletBehaviourFollowing bulletBehaviourUlt = BulletInstance.AddComponent<BulletBehaviourFollowingUlt>();
        BulletInstance.transform.localScale *= 2;
        bulletBehaviourUlt.Init(owner, destroyOnHit, bone, false, false, CombatUtils.GetOpposingTeam(owner.Team), 40, 25, 5, 10);
        return BulletInstance;
    }
    private GameObject InstantiateBullet(CharacterBehaviour owner, HumanBodyBones bone)
    {
        return Instantiate(BulletPrefab, owner.animator.GetBoneTransform(bone).position, owner.transform.rotation);
    }
    public GameObject CreateShockwave(CharacterBehaviour owner, bool destroyOnHit, HumanBodyBones bone, float maxRadius = 14f)
    {
        GameObject Shockwave = InstantiateShockwave(owner, bone);
        Shockwave.AddComponent<Shock>().Init(maxRadius);
        return Shockwave;
    }
    private GameObject InstantiateShockwave(CharacterBehaviour owner, HumanBodyBones bone)
    {
        return Instantiate(ShockwavePrefab, owner.animator.GetBoneTransform(bone).position, owner.transform.rotation);
    }
    public GameObject CreateShield(CharacterBehaviour owner)
    {
        GameObject shieldInstance = Instantiate(Shield, owner.transform.position + new Vector3(0f, 2f, 0f), owner.transform.rotation);
        shieldInstance.AddComponent<ShieldBehaviour>().SetOwner(owner);
        return shieldInstance;
    }
    public GameObject CreateFire(DamageableEntity owner)
    {
        GameObject fireInstance = Instantiate(Fire);
        fireInstance.AddComponent<FireBehaviour>().Init(owner);
        return fireInstance;
    }
    public GameObject CreateMeleeCollider(DamageableEntity owner)
    {
        GameObject meleeCollider = Instantiate(MeleeCollider);
        meleeCollider.AddComponent<MeleeColliderBehaviour>().Init(owner);
        return meleeCollider;
    }
    public GameObject CreateParryCollider(DamageableEntity owner)
    {
        GameObject parryCollider = Instantiate(ParryCollider);
        parryCollider.AddComponent<ParryColliderBehaviour>().Init(owner);
        return parryCollider;
    }
    public GameObject CreateManaDrainer(DamageableEntity owner, DamageInfo damageInfo)
    {
        GameObject manaDrainer = Instantiate(ManaDrainer);
        manaDrainer.AddComponent<ManaDrainerBehaviour>().Init(owner, damageInfo);
        return manaDrainer;
    }
    public GameObject CreateBulletDetector(DamageableEntity owner, int nBulletsToTrigger)
    {
        GameObject bulletDetector = Instantiate(BulletDetector, owner.gameObject.transform.position, owner.transform.rotation);
        bulletDetector.AddComponent<DetectBulletsCollisionHandler>().Init(owner, nBulletsToTrigger);
        return bulletDetector;
    }
    public GameObject CreateArmorAura(DamageableEntity owner)
    {
        GameObject armorAura = Instantiate(ArmorAura);
        armorAura.AddComponent<ArmorAura>().Init(owner);
        return armorAura;
    }
    public GameObject CreateSuperRegenAura(DamageableEntity owner)
    {
        GameObject superRegenAura = Instantiate(SuperRegenAura);
        superRegenAura.AddComponent<RegenAura>().Init(owner);
        return superRegenAura;
    }
    public GameObject CreateWall(DamageableEntity owner, int memberCount, float memberWidth, float memberHP)
    {
        GameObject wall = new GameObject();
        wall.AddComponent<WallBehaviour>().Init(owner.Team, memberCount, memberWidth, memberHP);
        return wall;
    }
    public GameObject CreateMover(DamageableEntity owner, DamageInfo damageInfo, float poisonDamage)
    {
        GameObject Mover = Instantiate(MoveCollider);
        Mover.AddComponent<OffsideMover>().Init(owner, damageInfo, poisonDamage);
        return Mover;
    }
}