using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorAura : MonoBehaviour
{
    private CapsuleCollider aura;
    private DamageableEntity Owner;
    public void Init(DamageableEntity owner)
    {
        aura = GetComponent<CapsuleCollider>();
        Owner = owner;
        ArmorAuraCollisionHandler armorAuraCollisionHandler = gameObject.AddComponent<ArmorAuraCollisionHandler>();
        armorAuraCollisionHandler.Init(owner);
    }
    void Update()
    {
        transform.position = Owner.transform.position;
    }
}